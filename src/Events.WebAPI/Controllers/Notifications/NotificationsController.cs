using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Common;
using Events.Application.Models.Notification;
using Events.Application.Models.System;
using Events.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Events.WebAPI.Controllers.Notifications;

[Route("api/notifications")]
[ApiController]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    /// <summary>
    /// Sends a notification to a specified account.
    /// </summary>
    /// <param name="accountId">The ID of the account to which the notification will be sent.</param>
    /// <param name="sendModel">The notification details.</param>
    /// <returns>The sent notification information.</returns>
    [HttpPost]
    [Authorize(Policy = AuthPolicies.AdministratorPolicy)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(NotificationResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendNotificationAsync(
        [FromQuery] Guid accountId,
        [FromBody] NotificationSendRequestModel sendModel,
        CancellationToken ct
    )
    {
        var notificationModel = await notificationService.SendNotificationAsync(
            accountId,
            sendModel,
            ct
        );
        return CreatedAtAction(
            nameof(GetNotificationByIdAsync),
            new { notificationId = notificationModel.Id },
            notificationModel
        );
    }

    /// <summary>
    /// Retrieves a notification by its ID.
    /// </summary>
    /// <param name="notificationId">The ID of the notification to retrieve.</param>
    /// <returns>The notification information.</returns>
    [HttpGet($"{{{RouteKeys.NotificationId}:guid}}")]
    [Authorize(Policy = AuthPolicies.NotificationOwnerOrAdministratorPolicy)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(NotificationResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotificationByIdAsync(
        [FromRoute] Guid notificationId,
        CancellationToken ct
    )
    {
        return Ok(await notificationService.GetNotificationByIdAsModelAsync(notificationId, ct));
    }

    /// <summary>
    /// Retrieves a list of notifications, optionally filtered by account ID.
    /// </summary>
    /// <param name="filterModel">The filter criteria for notifications.</param>
    /// <param name="accountId">The ID of the account for which to retrieve notifications (optional).</param>
    /// <returns>A list of notifications.</returns>
    [HttpGet]
    [Authorize(Policy = AuthPolicies.NotificationOwnerOrAdministratorPolicy)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(
        typeof(PagedResponseModel<NotificationResponseModel>),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotificationsAsync(
        [FromQuery] NotificationFilterRequestModel filterModel,
        [FromQuery] Guid? accountId,
        CancellationToken ct
    )
    {
        return Ok(
            await notificationService.GetNotificationsAsModelsAsync(filterModel, accountId, ct)
        );
    }

    /// <summary>
    /// Marks a specific notification as read.
    /// </summary>
    /// <param name="notificationId">The ID of the notification to mark as read.</param>
    /// <returns>The updated notification information.</returns>
    [HttpPatch($"{{{RouteKeys.NotificationId}:guid}}/read")]
    [Authorize(Policy = AuthPolicies.NotificationOwnerOrAdministratorPolicy)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(NotificationResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkNotificationAsReadAsync(
        [FromRoute] Guid notificationId,
        CancellationToken ct
    )
    {
        return Ok(await notificationService.MarkNotificationAsReadAsync(notificationId, ct));
    }

    /// <summary>
    /// Marks all notifications for a specified account as read.
    /// </summary>
    /// <param name="accountId">The ID of the account whose notifications will be marked as read.</param>
    /// <returns>The count of notifications marked as read.</returns>
    [HttpPatch("read")]
    [Authorize(Policy = AuthPolicies.NotificationOwnerOrAdministratorPolicy)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAccountNotificationsAsReadAsync(
        [FromQuery, Required] Guid accountId,
        CancellationToken ct
    )
    {
        var readCount = await notificationService.MarkAccountNotificationsAsReadAsync(
            accountId,
            ct
        );
        return Ok(new { readCount });
    }

    /// <summary>
    /// Deletes a specific notification by its ID.
    /// </summary>
    /// <param name="notificationId">The ID of the notification to delete.</param>
    /// <returns>No content upon successful deletion.</returns>
    [HttpDelete($"{{{RouteKeys.NotificationId}:guid}}")]
    [Authorize(Policy = AuthPolicies.NotificationOwnerOrAdministratorPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNotificationAsync(
        [FromRoute] Guid notificationId,
        CancellationToken ct
    )
    {
        await notificationService.DeleteNotificationAsync(notificationId, ct);
        return NoContent();
    }

    /// <summary>
    /// Deletes all notifications for a specified account.
    /// </summary>
    /// <param name="accountId">The ID of the account whose notifications will be deleted.</param>
    /// <returns>The count of notifications deleted.</returns>
    [HttpDelete]
    [Authorize(Policy = AuthPolicies.NotificationOwnerOrAdministratorPolicy)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponseModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAccountNotificationsAsync(
        [FromQuery, Required] Guid accountId,
        CancellationToken ct
    )
    {
        var deletedCount = await notificationService.DeleteAccountNotificationsAsync(accountId, ct);
        return Ok(new { deletedCount });
    }
}
