using AutoMapper;
using Events.Application.Interfaces.Infrastructure;
using Events.Application.Interfaces.Services;
using Events.Application.Services;
using Events.Domain.Interfaces.UnitOfWork;
using Events.Tests.Common.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace Events.Tests.UnitTests.Application.Services.EventServiceTests;

public abstract class EventServiceTests
{
    protected readonly Mock<IDbUnitOfWork> _mockDbUnitOfWork;
    protected readonly Mock<IMediaService> _mockMediaService;
    protected readonly Mock<INotificationService> _mockNotificationService;
    protected readonly Mock<IMapper> _mockMapper;
    protected readonly EventService _eventService;

    public EventServiceTests()
    {
        _mockDbUnitOfWork = new Mock<IDbUnitOfWork>();
        _mockMediaService = new Mock<IMediaService>();
        _mockNotificationService = new Mock<INotificationService>();
        _mockMapper = new Mock<IMapper>();

        var appSettings = ConfigurationHelper.LoadAppSettings(
            ConfigurationHelper.TestAppSettingsFileName
        );
        _eventService = new EventService(
            _mockDbUnitOfWork.Object,
            _mockMapper.Object,
            _mockMediaService.Object,
            Options.Create(appSettings),
            _mockNotificationService.Object
        );
    }
}
