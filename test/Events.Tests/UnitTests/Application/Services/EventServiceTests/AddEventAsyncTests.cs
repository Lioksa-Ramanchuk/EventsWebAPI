using Events.Application.Models.Event;
using Events.Domain.Entities;
using Events.Domain.Exceptions.EventExceptions;
using Events.Tests.Common.Mocking.EntityCreators;
using Events.Tests.Common.Mocking.ModelCreators;
using Moq;

namespace Events.Tests.UnitTests.Application.Services.EventServiceTests;

public class AddEventAsyncTests : EventServiceTests
{
    [Fact]
    public async Task AddEventAsync_ShouldAddEvent_WhenEventDoesNotExist()
    {
        // Arrange
        var addModel = EventModelCreator.CreateEventAddRequestModel(title: "New Event");
        var newEvent = EventEntityCreator.Create(title: addModel.Title);
        var responseModel = EventModelCreator.CreateEventResponseModel(title: newEvent.Title);

        _mockMapper.Setup(m => m.Map<Event>(addModel)).Returns(newEvent);

        _mockMapper.Setup(m => m.Map<EventResponseModel>(newEvent)).Returns(responseModel);

        _mockDbUnitOfWork
            .Setup(u => u.Events.GetByTitleAsync(addModel.Title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Event);

        // Act
        var result = await _eventService.AddEventAsync(addModel, CancellationToken.None);

        // Assert
        _mockDbUnitOfWork.Verify(u => u.Events.Add(It.IsAny<Event>()), Times.Once);
        _mockDbUnitOfWork.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
        Assert.NotNull(result);
        Assert.Equal(addModel.Title, result.Title);
    }

    [Fact]
    public async Task AddEventAsync_ShouldThrowEventAlreadyExistsException_WhenEventExists()
    {
        // Arrange
        var existingEvent = EventEntityCreator.Create(title: "Existing Event");
        var addModel = EventModelCreator.CreateEventAddRequestModel(title: existingEvent.Title);
        var newEvent = EventEntityCreator.Create(title: addModel.Title);

        _mockMapper.Setup(m => m.Map<Event>(addModel)).Returns(newEvent);

        _mockDbUnitOfWork
            .Setup(u =>
                u.Events.GetByTitleAsync(existingEvent.Title, It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(existingEvent);

        // Act & Assert
        await Assert.ThrowsAsync<EventAlreadyExistsException>(
            () => _eventService.AddEventAsync(addModel, CancellationToken.None)
        );
    }
}
