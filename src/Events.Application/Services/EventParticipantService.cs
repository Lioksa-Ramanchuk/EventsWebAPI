using AutoMapper;
using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Common;
using Events.Application.Models.EventParticipant.EventParticipant;
using Events.Domain.Entities;
using Events.Domain.Exceptions.EventExceptions;
using Events.Domain.Exceptions.EventParticipantExceptions;
using Events.Domain.Exceptions.ParticipantExceptions;
using Events.Domain.Interfaces.UnitOfWork;
using Microsoft.Extensions.Options;

namespace Events.Application.Services;

public class EventParticipantService(
    IDbUnitOfWork db,
    IMapper mapper,
    IOptions<AppSettings> appSettings
) : IEventParticipantService
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public async Task<EventParticipantResponseModel> RegisterEventParticipantAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct
    )
    {
        var eventWithEventParticipants =
            await db.Events.GetByIdWithEventParticipantsAsync(eventId, ct)
            ?? throw new EventNotFoundException(eventId);
        if (await db.Participants.GetByIdAsync(participantId, ct) is null)
        {
            throw new ParticipantNotFoundException(participantId);
        }

        if (
            eventWithEventParticipants.MaxParticipantsCount is int eventMaxParticipantsCount
            && eventWithEventParticipants.EventParticipants.Count
                >= eventWithEventParticipants.MaxParticipantsCount
        )
        {
            throw new EventMaxParticipantsReachedException(eventId, eventMaxParticipantsCount);
        }

        if (await db.EventParticipants.GetByKeyAsync(eventId, participantId, ct) is not null)
        {
            throw new EventParticipantAlreadyRegisteredException(participantId, eventId);
        }

        var eventParticipant = new EventParticipant
        {
            EventId = eventId,
            ParticipantId = participantId,
            RegistrationDate = DateTime.UtcNow,
        };

        db.EventParticipants.Add(eventParticipant);
        await db.SaveChangesAsync(ct);

        return mapper.Map<EventParticipantResponseModel>(eventParticipant);
    }

    public async Task<EventParticipantResponseModel> GetEventParticipantByKeyAsModelAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct
    )
    {
        if (await db.Events.GetByIdAsync(eventId, ct) is null)
        {
            throw new EventNotFoundException(eventId);
        }

        if (await db.Participants.GetByIdAsync(participantId, ct) is null)
        {
            throw new ParticipantNotFoundException(participantId);
        }

        var eventParticipantWithParticipant =
            await db.EventParticipants.GetByKeyWithParticipantAsync(eventId, participantId, ct)
            ?? throw new EventParticipantNotFoundException(eventId, participantId);

        return mapper.Map<EventParticipantResponseModel>(eventParticipantWithParticipant);
    }

    public async Task<
        PagedResponseModel<EventParticipantResponseModel>
    > GetEventParticipantsAsModelsAsync(
        Guid eventId,
        EventParticipantFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        if (await db.Events.GetByIdAsync(eventId, ct) is null)
        {
            throw new EventNotFoundException(eventId);
        }

        var offset = filterModel.Offset ?? 0;
        var limit = filterModel.Limit ?? _validationSettings.MaxFilterLimit;

        var (eventParticipantsList, totalCount) =
            await db.EventParticipants.GetPagedAllWithParticipantByEventIdAsync(
                eventId,
                offset,
                limit,
                ct
            );

        var eventParticipantModels = mapper.Map<List<EventParticipantResponseModel>>(
            eventParticipantsList
        );

        return new PagedResponseModel<EventParticipantResponseModel>
        {
            TotalCount = totalCount,
            Offset = offset,
            Limit = limit,
            Data = eventParticipantModels,
        };
    }

    public async Task UnregisterEventParticipantAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct
    )
    {
        if (await db.Events.GetByIdAsync(eventId, ct) is null)
        {
            throw new EventNotFoundException(eventId);
        }

        if (await db.Participants.GetByIdAsync(participantId, ct) is null)
        {
            throw new ParticipantNotFoundException(participantId);
        }

        var eventParticipant =
            await db.EventParticipants.GetByKeyAsTrackingAsync(eventId, participantId, ct)
            ?? throw new EventParticipantNotRegisteredException(participantId, eventId);

        db.EventParticipants.Remove(eventParticipant);
        await db.SaveChangesAsync(ct);
    }
}
