using AutoMapper;
using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Common;
using Events.Application.Models.Event;
using Events.Application.Models.Notification;
using Events.Domain.Entities;
using Events.Domain.Exceptions.EventExceptions;
using Events.Domain.Exceptions.MediaExceptions;
using Events.Domain.Interfaces.UnitOfWork;
using Events.Domain.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SmartFormat;

namespace Events.Application.Services;

public class EventService(
    IDbUnitOfWork db,
    IMapper mapper,
    IMediaService mediaService,
    IOptions<AppSettings> appSettings,
    INotificationService notificationService
) : IEventService
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public async Task<EventResponseModel> AddEventAsync(
        EventAddRequestModel addModel,
        CancellationToken ct
    )
    {
        ArgumentNullException.ThrowIfNull(addModel, nameof(addModel));

        var newEvent = mapper.Map<Event>(addModel);

        if (await db.Events.GetByTitleAsync(newEvent.Title, ct) is not null)
        {
            throw new EventAlreadyExistsException(
                Smart.Format(
                    ExceptionMessages.EventWithTitleAlreadyExists,
                    new { eventTitle = newEvent.Title }
                )
            );
        }

        db.Events.Add(newEvent);
        await db.SaveChangesAsync(ct);

        return AddEventResponseModelImageUrl(mapper.Map<EventResponseModel>(newEvent));
    }

    public async Task<EventResponseModel> GetEventByIdAsModelAsync(
        Guid eventId,
        CancellationToken ct
    )
    {
        var evt =
            await db.Events.GetByIdAsync(eventId, ct) ?? throw new EventNotFoundException(eventId);
        return AddEventResponseModelImageUrl(mapper.Map<EventResponseModel>(evt));
    }

    public async Task<EventResponseModel> GetSingleEventBySearchAsModelAsync(
        EventSearchRequestModel searchModel,
        CancellationToken ct
    )
    {
        ArgumentNullException.ThrowIfNull(searchModel, nameof(searchModel));

        Event? searchResult = null;

        if (!string.IsNullOrWhiteSpace(searchModel.Title))
        {
            var eventFound = await db.Events.GetByTitleAsync(searchModel.Title.Trim(), ct);
            if (eventFound is null || searchResult is not null && searchResult.Id != eventFound.Id)
            {
                throw new EventNotFoundException(ExceptionMessages.EventBySearchNotFound);
            }
            searchResult = eventFound;
        }

        return AddEventResponseModelImageUrl(mapper.Map<EventResponseModel>(searchResult));
    }

    public async Task<PagedResponseModel<EventResponseModel>> GetAllEventsAsModelsAsync(
        EventFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        var offset = filterModel.Offset ?? 0;
        var limit = filterModel.Limit ?? _validationSettings.MaxFilterLimit;

        var (eventsList, totalCount) = await db.Events.GetPagedAllByFilterAsync(
            filterModel.StartEventDate,
            filterModel.EndEventDate,
            filterModel.Location,
            filterModel.Category,
            offset,
            limit,
            ct
        );

        var eventModels = mapper.Map<List<EventResponseModel>>(eventsList);
        eventModels.ForEach(em => AddEventResponseModelImageUrl(em));

        return new PagedResponseModel<EventResponseModel>
        {
            TotalCount = totalCount,
            Offset = offset,
            Limit = limit,
            Data = eventModels,
        };
    }

    public async Task<EventResponseModel> UpdateEventAsync(
        Guid eventId,
        EventUpdateRequestModel updateModel,
        CancellationToken ct
    )
    {
        var evt =
            await db.Events.GetByIdAsTrackingAsync(eventId, ct)
            ?? throw new EventNotFoundException(eventId);

        bool eventTitleChanged =
            !string.IsNullOrWhiteSpace(updateModel.Title) && !updateModel.Title.Equals(evt.Title);

        if (eventTitleChanged && await db.Events.AnyWithTitleAsync(updateModel.Title!, ct))
        {
            throw new EventAlreadyExistsException(
                Smart.Format(
                    ExceptionMessages.EventWithTitleAlreadyExists,
                    new { eventTitle = updateModel.Title }
                )
            );
        }

        var oldEventTitle = evt.Title;

        mapper.Map(updateModel, evt);
        await db.SaveChangesAsync(ct);

        string eventTitleForNotification = $"'{oldEventTitle}'";
        if (eventTitleChanged)
        {
            eventTitleForNotification += $" (now '{evt.Title}')";
        }
        var notificationModel = new NotificationSendRequestModel
        {
            Message = Smart.Format(
                AppMessages.EventChangedNotification,
                new { eventTitle = eventTitleForNotification }
            ),
        };
        await NotifyEventParticipantsAsync(eventId, notificationModel, ct);

        return AddEventResponseModelImageUrl(mapper.Map<EventResponseModel>(evt));
    }

    public async Task<string> UploadEventImageAsync(
        Guid eventId,
        IFormFile imageFile,
        CancellationToken ct
    )
    {
        var evt =
            await db.Events.GetByIdAsTrackingAsync(eventId, ct)
            ?? throw new EventNotFoundException(eventId);

        var uploadedImageFileName = await mediaService.UploadImageAsync(imageFile, ct);

        if (!string.IsNullOrWhiteSpace(evt.ImageFileName))
        {
            mediaService.RemoveImage(evt.ImageFileName);
        }

        evt.ImageFileName = uploadedImageFileName;

        await db.SaveChangesAsync(ct);

        return uploadedImageFileName;
    }

    public async Task<string> GetEventImageUrlAsync(Guid eventId, CancellationToken ct)
    {
        var evt =
            await db.Events.GetByIdAsync(eventId, ct) ?? throw new EventNotFoundException(eventId);

        if (string.IsNullOrWhiteSpace(evt.ImageFileName))
        {
            throw new NoEventImageException();
        }

        return mediaService.GetImageUrlInHttpContext(evt.ImageFileName);
    }

    public async Task RemoveEventImageAsync(Guid eventId, CancellationToken ct)
    {
        var evt =
            await db.Events.GetByIdAsTrackingAsync(eventId, ct)
            ?? throw new EventNotFoundException(eventId);

        if (string.IsNullOrWhiteSpace(evt.ImageFileName))
        {
            return;
        }

        try
        {
            mediaService.RemoveImage(evt.ImageFileName);
        }
        catch (ImageNotFoundException) { }

        evt.ImageFileName = null;

        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveEventAsync(Guid eventId, CancellationToken ct)
    {
        var evt =
            await db.Events.GetByIdAsTrackingAsync(eventId, ct)
            ?? throw new EventNotFoundException(eventId);

        if (!string.IsNullOrWhiteSpace(evt.ImageFileName))
        {
            mediaService.RemoveImage(evt.ImageFileName);
        }

        var notificationModel = new NotificationSendRequestModel
        {
            Message = Smart.Format(
                AppMessages.EventDeletedNotification,
                new { eventTitle = $"'{evt.Title}'" }
            ),
        };
        await NotifyEventParticipantsAsync(eventId, notificationModel, ct);

        db.Events.Remove(evt);
        await db.SaveChangesAsync(ct);
    }

    public async Task NotifyEventParticipantsAsync(
        Guid eventId,
        NotificationSendRequestModel sendModel,
        CancellationToken ct
    )
    {
        if (await db.Events.GetByIdAsync(eventId, ct) is null)
        {
            throw new EventNotFoundException(eventId);
        }

        var eventParticipantAccountIds =
            await db.EventParticipants.GetAllParticipantIdsByEventIdAsync(eventId, ct);

        await notificationService.NotifyAccountsAsync(eventParticipantAccountIds, sendModel, ct);
    }

    public EventResponseModel AddEventResponseModelImageUrl(EventResponseModel eventModel)
    {
        if (!string.IsNullOrWhiteSpace(eventModel.ImageFileName))
        {
            eventModel.ImageUrl = mediaService.GetImageUrlInHttpContext(eventModel.ImageFileName);
        }

        return eventModel;
    }
}
