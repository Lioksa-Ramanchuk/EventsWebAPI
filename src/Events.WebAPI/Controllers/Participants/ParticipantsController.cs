using System.Net.Mime;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Common;
using Events.Application.Models.Participant;
using Events.Application.Models.System;
using Events.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Events.WebAPI.Controllers.Participants;

[Route("api/participants")]
[ApiController]
public class ParticipantsController(IParticipantService participantService) : ControllerBase
{
    /// <summary>
    /// Retrieves a participant by their ID.
    /// </summary>
    /// <param name="participantId">The ID of the participant to retrieve.</param>
    /// <returns>The details of the specified participant.</returns>
    [HttpGet($"{{{RouteKeys.ParticipantId}:guid}}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ParticipantResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetParticipantById(
        [FromRoute] Guid participantId,
        CancellationToken ct
    )
    {
        return Ok(await participantService.GetParticipantByIdAsModelAsync(participantId, ct));
    }

    /// <summary>
    /// Retrieves all participants, optionally filtered.
    /// </summary>
    /// <param name="filterModel">The filter criteria for retrieving participants.</param>
    /// <returns>A list of participants matching the filter criteria.</returns>
    [HttpGet]
    [SwaggerOperation("GetAllParticipants")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(PagedResponseModel<ParticipantResponseModel>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAllParticipantsAsync(
        [FromQuery] ParticipantFilterRequestModel filterModel,
        CancellationToken ct
    )
    {
        return Ok(await participantService.GetAllParticipantsAsModelsAsync(filterModel, ct));
    }

    /// <summary>
    /// Updates the details of a specified participant.
    /// </summary>
    /// <param name="participantId">The ID of the participant to update.</param>
    /// <param name="updateModel">The updated participant details.</param>
    /// <returns>The updated participant information.</returns>
    [HttpPatch($"{{{RouteKeys.ParticipantId}:guid}}")]
    [Authorize(Policy = AuthPolicies.ParticipantOwnerOrAdministratorPolicy)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ParticipantResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateParticipantAsync(
        [FromRoute] Guid participantId,
        [FromBody] ParticipantUpdateRequestModel updateModel,
        CancellationToken ct
    )
    {
        return Ok(await participantService.UpdateParticipantAsync(participantId, updateModel, ct));
    }
}
