using AutoMapper;
using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Account;
using Events.Application.Models.Common;
using Events.Application.Models.Participant;
using Events.Domain.Entities;
using Events.Domain.Exceptions.AuthExceptions;
using Events.Domain.Interfaces.UnitOfWork;
using Events.Domain.Resources;
using Microsoft.Extensions.Options;
using SmartFormat;

namespace Events.Application.Services;

public class AccountService(
    IDbUnitOfWork db,
    ICryptoService cryptoService,
    IMapper mapper,
    IOptions<AppSettings> appSettings,
    IRoleManagementHelperService roleManagementHelperService
) : IAccountService
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public async Task<ParticipantResponseModel> RegisterParticipantAsync(
        ParticipantSignUpRequestModel signUpModel,
        CancellationToken ct
    )
    {
        ArgumentNullException.ThrowIfNull(signUpModel, nameof(signUpModel));

        var participant = mapper.Map<Participant>(signUpModel);

        if (await db.Accounts.AnyWithUsernameAsync(participant.Username, ct))
        {
            throw new AccountAlreadyExistsException(
                Smart.Format(
                    ExceptionMessages.AccountWithUsernameAlreadyExists,
                    new { accountUsername = participant.Username }
                )
            );
        }

        participant.Password = cryptoService.HashPassword(signUpModel.Password);

        participant.AccountRoles = [];

        var initialRoleTitles =
            await roleManagementHelperService.GetInitialParticipantRoleTitlesAsync(ct);
        foreach (var roleTitle in initialRoleTitles)
        {
            var role =
                await db.Roles.GetByTitleAsync(roleTitle, ct)
                ?? throw new RoleNotFoundException(
                    Smart.Format(ExceptionMessages.RoleWithTitleNotFound, new { roleTitle })
                );

            participant.AccountRoles.Add(
                new()
                {
                    AccountId = participant.Id,
                    RoleId = role.Id,
                    AssignedAt = DateTime.UtcNow,
                }
            );
        }

        db.Participants.Add(participant);
        await db.SaveChangesAsync(ct);

        return mapper.Map<ParticipantResponseModel>(participant);
    }

    public async Task<Account> AuthenticateAsync(
        AccountSignInRequestModel signInModel,
        CancellationToken ct
    )
    {
        ArgumentNullException.ThrowIfNull(signInModel, nameof(signInModel));

        var accountWithRoles = await db.Accounts.GetByUsernameWithRolesAsync(
            signInModel.Username.Trim(),
            ct
        );
        if (
            accountWithRoles is null
            || !cryptoService.VerifyPassword(accountWithRoles.Password, signInModel.Password)
        )
        {
            throw new BadCredentialsException(ExceptionMessages.BadCredentials);
        }
        return accountWithRoles;
    }

    public async Task<AccountResponseModel> GetAccountByIdAsModelAsync(
        Guid accountId,
        CancellationToken ct
    )
    {
        var account =
            await db.Accounts.GetByIdAsync(accountId, ct)
            ?? throw new AccountNotFoundException(accountId);
        return mapper.Map<AccountResponseModel>(account);
    }

    public async Task<PagedResponseModel<AccountResponseModel>> GetAllAccountsAsModelsAsync(
        AccountFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        var offset = filterModel.Offset ?? 0;
        var limit = filterModel.Limit ?? _validationSettings.MaxFilterLimit;

        var (accountsList, totalCount) = await db.Accounts.GetPagedAllAsync(offset, limit, ct);
        var accountModels = mapper.Map<ICollection<AccountResponseModel>>(accountsList);

        return new PagedResponseModel<AccountResponseModel>
        {
            TotalCount = totalCount,
            Offset = offset,
            Limit = limit,
            Data = accountModels,
        };
    }

    public async Task<AccountResponseModel> UpdateAccountAsync(
        Guid accountId,
        AccountUpdateRequestModel updateModel,
        CancellationToken ct
    )
    {
        ArgumentNullException.ThrowIfNull(updateModel, nameof(updateModel));

        var account =
            await db.Accounts.GetByIdAsTrackingAsync(accountId, ct)
            ?? throw new AccountNotFoundException(accountId);

        if (
            updateModel.Username is not null
            && updateModel.Username != account.Username
            && await db.Accounts.AnyWithUsernameAsync(updateModel.Username, ct)
        )
        {
            throw new AccountAlreadyExistsException(
                Smart.Format(
                    ExceptionMessages.AccountWithUsernameAlreadyExists,
                    new { accountUsername = updateModel.Username }
                )
            );
        }

        mapper.Map(updateModel, account);

        if (!string.IsNullOrWhiteSpace(updateModel.Password))
        {
            account.Password = cryptoService.HashPassword(updateModel.Password);
        }

        await db.SaveChangesAsync(ct);
        return mapper.Map<AccountResponseModel>(account);
    }

    public async Task RemoveAccountAsync(Guid accountId, CancellationToken ct)
    {
        var account =
            await db.Accounts.GetByIdAsTrackingAsync(accountId, ct)
            ?? throw new AccountNotFoundException(accountId);

        if (await roleManagementHelperService.IsLastAdministratorByIdAsync(accountId, ct))
        {
            throw new LastAdministratorRemovalException(accountId);
        }

        db.Accounts.Remove(account);
        await db.SaveChangesAsync(ct);
    }
}
