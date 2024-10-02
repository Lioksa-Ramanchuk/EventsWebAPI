using System.Net.Mime;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Common;
using Events.Application.Models.EventParticipant.ParticipantEvent;
using Events.Application.Models.System;
using Events.Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Events.WebAPI.Controllers.Participants;

[Route($"api/participants/{{{RouteKeys.ParticipantId}:guid}}/events")]
[ApiController]
public class ParticipantEventsController(IParticipantEventService eventParticipantService)
    : ControllerBase
{
    /// <summary>
    /// Retrieves a specific event associated with a participant by its ID.
    /// </summary>
    /// <param name="participantId">The ID of the participant.</param>
    /// <param name="eventId">The ID of the event to retrieve.</param>
    /// <returns>The event information associated with the specified participant and event IDs.</returns>
    [HttpGet("{eventId:guid}")]
    [SwaggerOperation("GetParticipantEventById")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ParticipantEventResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetParticipantEventByIdAsync(
        [FromRoute] Guid participantId,
        [FromRoute] Guid eventId,
        CancellationToken ct
    )
    {
        return Ok(
            await eventParticipantService.GetParticipantEventByKeyAsModelAsync(
                eventId,
                participantId,
                ct
            )
        );
    }

    /// <summary>
    /// Retrieves all events associated with a participant, optionally filtered.
    /// </summary>
    /// <param name="participantId">The ID of the participant whose events to retrieve.</param>
    /// <param name="filterModel">The filter criteria for retrieving participant events.</param>
    /// <returns>A list of events associated with the specified participant, filtered by the criteria.</returns>
    [HttpGet]
    [SwaggerOperation("GetParticipantEvents")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(PagedResponseModel<ParticipantEventResponseModel>),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetParticipantEventsAsync(
        [FromRoute] Guid participantId,
        [FromQuery] ParticipantEventFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        var participantModels = await eventParticipantService.GetParticipantEventsAsModelsAsync(
            participantId,
            filterModel,
            ct
        );
        return Ok(participantModels);
    }
}
