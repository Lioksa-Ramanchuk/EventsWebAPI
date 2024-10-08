using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Notification;
using Events.Domain.Interfaces.UnitOfWork;
using Events.Domain.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SmartFormat;

namespace Events.Application.Services.Hosted;

public class EventParticipantsNotificationService(IServiceProvider serviceProvider)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        int checkIntervalInSeconds;
        using (var scope = serviceProvider.CreateScope())
        {
            checkIntervalInSeconds = scope
                .ServiceProvider.GetRequiredService<IOptions<AppSettings>>()
                .Value.NotificationSettings.EventParticipantsNotificationCheckIntervalInSeconds;
        }

        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(checkIntervalInSeconds), ct);
            await NotifyParticipantsForUpcomingEvents(ct);
        }
    }

    private async Task NotifyParticipantsForUpcomingEvents(CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<IDbUnitOfWork>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        var notificationSettings = scope
            .ServiceProvider.GetRequiredService<IOptions<AppSettings>>()
            .Value.NotificationSettings;

        var now = DateTime.UtcNow;
        var timeWindowEnd = now.AddSeconds(
            notificationSettings.EventParticipantsNotificationMaxAdvanceTimeInSeconds
        );

        var upcomingEvents = await db.Events.GetUpcomingEventsHavingParticipantsAsync(
            (now, timeWindowEnd),
            ct
        );

        foreach (var evt in upcomingEvents)
        {
            var unnotifiedEventParticipants =
                await db.EventParticipants.GetAllUnnotifiedByEventIdAsTrackingAsync(evt.Id, ct);

            if (unnotifiedEventParticipants.Count == 0)
            {
                continue;
            }

            try
            {
                await db.BeginTransactionAsync(ct: ct);

                await notificationService.NotifyAccountsAsync(
                    unnotifiedEventParticipants.Select(ep => ep.ParticipantId),
                    new NotificationSendRequestModel
                    {
                        Message = Smart.Format(
                            AppMessages.EventComingNotification,
                            new { timeLeft = $"{(evt.EventDate - now).Minutes} minutes" }
                        ),
                    },
                    ct
                );

                unnotifiedEventParticipants.ForEach(ep => ep.IsNotifiedToday = true);
                await db.SaveChangesAsync(ct);

                await db.CommitAsync(ct);
            }
            catch
            {
                await db.RollbackAsync(ct);
                throw;
            }
        }
    }
}
