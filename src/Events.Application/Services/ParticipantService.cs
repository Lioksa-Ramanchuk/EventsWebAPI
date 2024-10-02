using AutoMapper;
using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Common;
using Events.Application.Models.Participant;
using Events.Domain.Exceptions.ParticipantExceptions;
using Events.Domain.Interfaces.UnitOfWork;
using Microsoft.Extensions.Options;

namespace Events.Application.Services;

public class ParticipantService(
    IDbUnitOfWork db,
    IMapper mapper,
    IAccountService accountService,
    IOptions<AppSettings> appSettings
) : IParticipantService
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public async Task<ParticipantResponseModel> GetParticipantByIdAsModelAsync(
        Guid participantId,
        CancellationToken ct
    )
    {
        var participant =
            await db.Participants.GetByIdAsync(participantId, ct)
            ?? throw new ParticipantNotFoundException(participantId);
        return mapper.Map<ParticipantResponseModel>(participant);
    }

    public async Task<PagedResponseModel<ParticipantResponseModel>> GetAllParticipantsAsModelsAsync(
        ParticipantFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        var offset = filterModel.Offset ?? 0;
        var limit = filterModel.Limit ?? _validationSettings.MaxFilterLimit;

        var (particiantsList, totalCount) = await db.Participants.GetPagedAllAsync(
            offset,
            limit,
            ct
        );
        var participantModels = mapper.Map<ICollection<ParticipantResponseModel>>(particiantsList);

        return new PagedResponseModel<ParticipantResponseModel>
        {
            TotalCount = totalCount,
            Offset = offset,
            Limit = limit,
            Data = participantModels,
        };
    }

    public async Task<ParticipantResponseModel> UpdateParticipantAsync(
        Guid participantId,
        ParticipantUpdateRequestModel updateModel,
        CancellationToken ct
    )
    {
        ArgumentNullException.ThrowIfNull(updateModel, nameof(updateModel));

        await accountService.UpdateAccountAsync(participantId, updateModel, ct);

        var participant =
            await db.Participants.GetByIdAsTrackingAsync(participantId, ct)
            ?? throw new ParticipantNotFoundException(participantId);

        mapper.Map(updateModel, participant);

        await db.SaveChangesAsync(ct);
        return mapper.Map<ParticipantResponseModel>(participant);
    }
}
