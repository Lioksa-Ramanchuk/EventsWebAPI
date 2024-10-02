using Events.Application.Models.Common;
using Events.Application.Models.Event;
using Events.Application.Models.Notification;
using Microsoft.AspNetCore.Http;

namespace Events.Application.Interfaces.Services;

public interface IEventService
{
    Task<EventResponseModel> AddEventAsync(
        EventAddRequestModel addModel,
        CancellationToken ct = default
    );
    EventResponseModel AddEventResponseModelImageUrl(EventResponseModel eventModel);
    Task<PagedResponseModel<EventResponseModel>> GetAllEventsAsModelsAsync(
        EventFilterRequestModel filterModel,
        CancellationToken ct = default
    );
    Task<EventResponseModel> GetEventByIdAsModelAsync(Guid eventId, CancellationToken ct = default);
    Task<string> GetEventImageUrlAsync(Guid eventId, CancellationToken ct = default);
    Task<EventResponseModel> GetSingleEventBySearchAsModelAsync(
        EventSearchRequestModel searchModel,
        CancellationToken ct = default
    );
    Task NotifyEventParticipantsAsync(
        Guid eventId,
        NotificationSendRequestModel sendModel,
        CancellationToken ct = default
    );
    Task RemoveEventAsync(Guid eventId, CancellationToken ct = default);
    Task RemoveEventImageAsync(Guid eventId, CancellationToken ct = default);
    Task<EventResponseModel> UpdateEventAsync(
        Guid eventId,
        EventUpdateRequestModel updateModel,
        CancellationToken ct = default
    );
    Task<string> UploadEventImageAsync(
        Guid eventId,
        IFormFile imageFile,
        CancellationToken ct = default
    );
}
