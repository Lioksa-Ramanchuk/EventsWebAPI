using System.Net.Mime;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Common;
using Events.Application.Models.Event;
using Events.Application.Models.Notification;
using Events.Application.Models.System;
using Events.Domain.Constants;
using Events.Domain.Exceptions.EventExceptions;
using Events.WebAPI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Events.WebAPI.Controllers.Events;

[Route("api/events")]
[ApiController]
public class EventsController(IEventService eventService) : ControllerBase
{
    /// <summary>
    /// Adds a new event.
    /// </summary>
    /// <param name="addModel">The event details to add.</param>
    /// <returns>The created event information.</returns>
    [HttpPost]
    [Authorize(Policy = AuthPolicies.AdministratorPolicy)]
    [SwaggerOperation("AddEvent")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(EventResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddEventAsync(
        [FromBody] EventAddRequestModel addModel,
        CancellationToken ct
    )
    {
        var evt = await eventService.AddEventAsync(addModel, ct);
        return CreatedAtAction(nameof(GetEventByIdAsync), new { eventId = evt.Id }, evt);
    }

    /// <summary>
    /// Retrieves an event by ID.
    /// </summary>
    /// <param name="eventId">The ID of the event to retrieve.</param>
    /// <returns>The event information associated with the specified ID.</returns>
    [HttpGet($"{{{RouteKeys.EventId}:guid}}")]
    [SwaggerOperation("GetEventById")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(EventResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventByIdAsync(
        [FromRoute] Guid eventId,
        CancellationToken ct
    )
    {
        return Ok(await eventService.GetEventByIdAsModelAsync(eventId, ct));
    }

    /// <summary>
    /// Searches for a single event based on the specified criteria.
    /// </summary>
    /// <param name="searchModel">The criteria to search for an event.</param>
    /// <returns>The event matching the search criteria.</returns>
    [HttpGet("search")]
    [SwaggerOperation("GetEventBySearch")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(EventResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventBySearchAsync(
        [FromQuery] EventSearchRequestModel searchModel,
        CancellationToken ct
    )
    {
        return Ok(await eventService.GetSingleEventBySearchAsModelAsync(searchModel, ct));
    }

    /// <summary>
    /// Retrieves all events, optionally filtered.
    /// </summary>
    /// <param name="filterModel">The filter criteria for retrieving events.</param>
    /// <returns>A list of events matching the filter criteria.</returns>
    [HttpGet]
    [SwaggerOperation("GetAllEvents")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PagedResponseModel<EventResponseModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEventsAsync(
        [FromQuery] EventFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        return Ok(await eventService.GetAllEventsAsModelsAsync(filterModel, ct));
    }

    /// <summary>
    /// Updates an existing event.
    /// </summary>
    /// <param name="eventId">The ID of the event to update.</param>
    /// <param name="updateModel">The updated event information.</param>
    /// <returns>The updated event information.</returns>
    [HttpPatch($"{{{RouteKeys.EventId}:guid}}")]
    [Authorize(Policy = AuthPolicies.AdministratorPolicy)]
    [SwaggerOperation("UpdateEvent")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(EventResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateEventAsync(
        [FromRoute] Guid eventId,
        [FromBody] EventUpdateRequestModel updateModel,
        CancellationToken ct
    )
    {
        return Ok(await eventService.UpdateEventAsync(eventId, updateModel, ct));
    }

    /// <summary>
    /// Removes an event by ID.
    /// </summary>
    /// <param name="eventId">The ID of the event to remove.</param>
    /// <returns>No content.</returns>
    [HttpDelete($"{{{RouteKeys.EventId}:guid}}")]
    [Authorize(Policy = AuthPolicies.AdministratorPolicy)]
    [SwaggerOperation("RemoveEvent")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveEventAsync(
        [FromRoute] Guid eventId,
        CancellationToken ct
    )
    {
        await eventService.RemoveEventAsync(eventId, ct);
        return NoContent();
    }

    /// <summary>
    /// Uploads an image for a specified event.
    /// </summary>
    /// <param name="eventId">The ID of the event to upload the image for.</param>
    /// <param name="imageFile">The image file to upload.</param>
    /// <returns>The name of the uploaded image file.</returns>
    [HttpPost($"{{{RouteKeys.EventId}:guid}}/image")]
    [Authorize(Policy = AuthPolicies.AdministratorPolicy)]
    [SwaggerOperation("UploadEventImage")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadEventImageAsync(
        [FromRoute] Guid eventId,
        IFormFile imageFile,
        CancellationToken ct
    )
    {
        var imageFileName = await eventService.UploadEventImageAsync(eventId, imageFile, ct);
        return CreatedAtAction(
            nameof(GetEventImageUrlAsync),
            new { eventId },
            new { imageFileName }
        );
    }

    /// <summary>
    /// Retrieves the image URL for a specified event.
    /// </summary>
    /// <param name="eventId">The ID of the event to retrieve the image for.</param>
    /// <returns>The image URL for the specified event.</returns>
    [HttpGet($"{{{RouteKeys.EventId}:guid}}/image")]
    [SwaggerOperation("GetEventImageUrl")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetEventImageUrlAsync(
        [FromRoute] Guid eventId,
        CancellationToken ct
    )
    {
        try
        {
            return Ok(new { imageUrl = await eventService.GetEventImageUrlAsync(eventId, ct) });
        }
        catch (NoEventImageException)
        {
            return NoContent();
        }
    }

    /// <summary>
    /// Removes the image for a specified event.
    /// </summary>
    /// <param name="eventId">The ID of the event to remove the image from.</param>
    /// <returns>No content.</returns>
    [HttpDelete($"{{{RouteKeys.EventId}:guid}}/image")]
    [Authorize(Policy = AuthPolicies.AdministratorPolicy)]
    [SwaggerOperation("RemoveEventImage")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveEventImageAsync(
        [FromRoute] Guid eventId,
        CancellationToken ct
    )
    {
        await eventService.RemoveEventImageAsync(eventId, ct);
        return NoContent();
    }

    /// <summary>
    /// Notifies participants of a specified event.
    /// </summary>
    /// <param name="eventId">The ID of the event to notify participants for.</param>
    /// <param name="sendModel">The notification details.</param>
    /// <returns>Success message upon successful notification.</returns>
    [HttpPost($"{{{RouteKeys.EventId}:guid}}/notify")]
    [Authorize(Policy = AuthPolicies.AdministratorPolicy)]
    [SwaggerOperation("NotifyEventParticipants")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> NotifyEventParticipantsAsync(
        [FromRoute] Guid eventId,
        [FromBody] NotificationSendRequestModel sendModel,
        CancellationToken ct
    )
    {
        await eventService.NotifyEventParticipantsAsync(eventId, sendModel, ct);
        return Ok(ResponseMessages.EventParticipantsNotificationSuccess);
    }
}
