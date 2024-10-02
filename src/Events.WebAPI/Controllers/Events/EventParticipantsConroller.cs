using System.Net.Mime;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Common;
using Events.Application.Models.EventParticipant.EventParticipant;
using Events.Application.Models.System;
using Events.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Events.WebAPI.Controllers.Events;

[Route($"api/events/{{{RouteKeys.EventId}:guid}}/participants")]
[ApiController]
public class EventParticipantsController(IEventParticipantService eventParticipantService)
    : ControllerBase
{
    /// <summary>
    /// Registers a participant for a specific event.
    /// </summary>
    /// <param name="eventId">The ID of the event.</param>
    /// <param name="participantId">The ID of the participant to register.</param>
    /// <returns>The created event participant information.</returns>
    [HttpPost($"{{{RouteKeys.ParticipantId}:guid}}")]
    [Authorize(Policy = AuthPolicies.ParticipantOwnerOrAdministratorPolicy)]
    [SwaggerOperation("RegisterEventParticipant")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(EventParticipantResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterEventParticipantAsync(
        [FromRoute] Guid eventId,
        [FromRoute] Guid participantId,
        CancellationToken ct
    )
    {
        var eventParticipantModel = await eventParticipantService.RegisterEventParticipantAsync(
            eventId,
            participantId,
            ct
        );
        return CreatedAtAction(
            nameof(GetEventParticipantByIdAsync),
            new { eventId, participantId },
            eventParticipantModel
        );
    }

    /// <summary>
    /// Retrieves an event participant's information by event ID and participant ID.
    /// </summary>
    /// <param name="eventId">The ID of the event.</param>
    /// <param name="participantId">The ID of the participant.</param>
    /// <returns>The event participant's information.</returns>
    [HttpGet($"{{{RouteKeys.ParticipantId}:guid}}")]
    [SwaggerOperation("GetEventParticipantById")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(EventParticipantResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventParticipantByIdAsync(
        [FromRoute] Guid eventId,
        [FromRoute] Guid participantId,
        CancellationToken ct
    )
    {
        return Ok(
            await eventParticipantService.GetEventParticipantByKeyAsModelAsync(
                eventId,
                participantId,
                ct
            )
        );
    }

    /// <summary>
    /// Retrieves all participants for a specific event.
    /// </summary>
    /// <param name="eventId">The ID of the event.</param>
    /// <param name="filterModel">The filter criteria for retrieving participants.</param>
    /// <returns>A list of participants associated with the event.</returns>
    [HttpGet]
    [SwaggerOperation("GetEventParticipants")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(PagedResponseModel<EventParticipantResponseModel>),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventParticipantsAsync(
        [FromRoute] Guid eventId,
        [FromQuery] EventParticipantFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        var participantModels = await eventParticipantService.GetEventParticipantsAsModelsAsync(
            eventId,
            filterModel,
            ct
        );
        return Ok(participantModels);
    }

    /// <summary>
    /// Unregisters a participant from a specific event.
    /// </summary>
    /// <param name="eventId">The ID of the event.</param>
    /// <param name="participantId">The ID of the participant to unregister.</param>
    /// <returns>No content.</returns>
    [HttpDelete($"{{{RouteKeys.ParticipantId}:guid}}")]
    [Authorize(Policy = AuthPolicies.ParticipantOwnerOrAdministratorPolicy)]
    [SwaggerOperation("UnregisterParticipant")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnregisterParticipantAsync(
        [FromRoute] Guid eventId,
        [FromRoute] Guid participantId,
        CancellationToken ct
    )
    {
        await eventParticipantService.UnregisterEventParticipantAsync(eventId, participantId, ct);
        return NoContent();
    }
}
