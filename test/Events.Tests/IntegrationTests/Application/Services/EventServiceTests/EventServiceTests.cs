using Events.Application.Interfaces.Services;
using Events.Domain.Exceptions.EventExceptions;
using Events.Domain.Interfaces.UnitOfWork;
using Events.Tests.Common.Fixtures;
using Events.Tests.Common.Fixtures.Collections;
using Events.Tests.Common.Mocking.EntityCreators;
using Events.Tests.Common.Mocking.ModelCreators;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Tests.IntegrationTests.Application.Services.EventServiceTests;

[Collection(nameof(DbCollection))]
public class EventServiceTests(ServiceProviderFixture serviceProviderFixture) : IClassFixture<ServiceProviderFixture>
{
    protected readonly IServiceProvider _serviceProvider = serviceProviderFixture.ServiceProvider;

    [Fact]
    public async Task AddEventAsync_ShouldAddEvent_WhenEventDoesNotExist()
    {
        // Arrange
        var eventService = _serviceProvider.GetRequiredService<IEventService>();
        var addModel = EventModelCreator.CreateEventAddRequestModel(title: "Event");

        // Act
        var result = await eventService.AddEventAsync(addModel, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(addModel.Title, result.Title);

        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDbUnitOfWork>();
        var savedEvent = await db.Events.GetByTitleAsync(addModel.Title, CancellationToken.None);
        Assert.NotNull(savedEvent);
        Assert.Equal(addModel.Title, savedEvent.Title);
    }

    [Fact]
    public async Task AddEventAsync_ShouldThrowEventAlreadyExistsException_WhenEventExists()
    {
        // Arrange
        var eventService = _serviceProvider.GetRequiredService<IEventService>();
        var existingEvent = EventEntityCreator.Create(title: "Existing Event");

        using (var scope = _serviceProvider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<IDbUnitOfWork>();
            db.Events.Add(existingEvent);
            await db.SaveChangesAsync(CancellationToken.None);
        }

        var addModel = EventModelCreator.CreateEventAddRequestModel(title: existingEvent.Title);

        // Act & Assert
        await Assert.ThrowsAsync<EventAlreadyExistsException>(
            () => eventService.AddEventAsync(addModel, CancellationToken.None)
        );
    }
}
