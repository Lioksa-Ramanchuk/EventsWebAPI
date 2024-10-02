using Events.Application.Models.Account;
using Events.Application.Models.Common;
using Events.Application.Models.Participant;
using Events.Domain.Entities;

namespace Events.Application.Interfaces.Services;

public interface IAccountService
{
    Task<Account> AuthenticateAsync(
        AccountSignInRequestModel signInModel,
        CancellationToken ct = default
    );
    Task<AccountResponseModel> GetAccountByIdAsModelAsync(
        Guid accountId,
        CancellationToken ct = default
    );
    Task<PagedResponseModel<AccountResponseModel>> GetAllAccountsAsModelsAsync(
        AccountFilterRequestModel filterModel,
        CancellationToken ct = default
    );
    Task<ParticipantResponseModel> RegisterParticipantAsync(
        ParticipantSignUpRequestModel signUpModel,
        CancellationToken ct = default
    );
    Task RemoveAccountAsync(Guid accountId, CancellationToken ct = default);
    Task<AccountResponseModel> UpdateAccountAsync(
        Guid accountId,
        AccountUpdateRequestModel updateModel,
        CancellationToken ct = default
    );
}
