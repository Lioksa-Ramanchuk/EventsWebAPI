using AutoMapper;
using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Common;
using Events.Application.Models.EventParticipant.ParticipantEvent;
using Events.Domain.Exceptions.EventExceptions;
using Events.Domain.Exceptions.EventParticipantExceptions;
using Events.Domain.Exceptions.ParticipantExceptions;
using Events.Domain.Interfaces.UnitOfWork;
using Microsoft.Extensions.Options;

namespace Events.Application.Services;

public class ParticipantEventService(
    IDbUnitOfWork db,
    IMapper mapper,
    IEventService eventService,
    IOptions<AppSettings> appSettings
) : IParticipantEventService
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public async Task<ParticipantEventResponseModel> GetParticipantEventByKeyAsModelAsync(
        Guid participantId,
        Guid eventId,
        CancellationToken ct
    )
    {
        if (await db.Participants.GetByIdAsync(participantId, ct) is null)
        {
            throw new ParticipantNotFoundException(participantId);
        }

        if (await db.Events.GetByIdAsync(eventId, ct) is null)
        {
            throw new EventNotFoundException(eventId);
        }

        var eventParticipantWithEvent =
            await db.EventParticipants.GetByKeyWithEventAsync(eventId, participantId, ct)
            ?? throw new EventParticipantNotFoundException(eventId, participantId);

        var participantEventModel = mapper.Map<ParticipantEventResponseModel>(
            eventParticipantWithEvent
        );
        eventService.AddEventResponseModelImageUrl(participantEventModel.Event);
        return participantEventModel;
    }

    public async Task<
        PagedResponseModel<ParticipantEventResponseModel>
    > GetParticipantEventsAsModelsAsync(
        Guid participantId,
        ParticipantEventFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        if (await db.Participants.GetByIdAsync(participantId, ct) is null)
        {
            throw new ParticipantNotFoundException(participantId);
        }

        var offset = filterModel.Offset ?? 0;
        var limit = filterModel.Limit ?? _validationSettings.MaxFilterLimit;

        var (participantEventsList, totalCount) =
            await db.EventParticipants.GetPagedAllWithEventByParticipantIdAsync(
                participantId,
                offset,
                limit,
                ct
            );

        var participantEventsModels = mapper.Map<List<ParticipantEventResponseModel>>(
            participantEventsList
        );
        participantEventsModels.ForEach(pe => eventService.AddEventResponseModelImageUrl(pe.Event));

        return new PagedResponseModel<ParticipantEventResponseModel>
        {
            TotalCount = totalCount,
            Offset = offset,
            Limit = limit,
            Data = participantEventsModels,
        };
    }
}
