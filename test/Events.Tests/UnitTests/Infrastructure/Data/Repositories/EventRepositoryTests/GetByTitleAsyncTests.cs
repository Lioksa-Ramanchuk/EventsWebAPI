using Events.Tests.Common.Fixtures;
using Events.Tests.Common.Fixtures.Collections;
using Events.Tests.Common.Mocking.EntityCreators;

namespace Events.Tests.UnitTests.Infrastructure.Data.Repositories.EventRepositoryTests;

[Collection(nameof(DbCollection))]
public class GetByTitleAsyncTests(DbFixture dbFixture) : EventRepositoryTests(dbFixture)
{
    [Fact]
    public async Task GetByTitleAsync_ReturnsEvent_WhenTitleExists()
    {
        // Arrange
        var title = "Event";

        _dbUnitOfWork.Events.Add(EventEntityCreator.Create(title: title));
        await _dbUnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await _dbUnitOfWork.Events.GetByTitleAsync(title, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(title, result.Title);
    }

    [Fact]
    public async Task GetByTitleAsync_ReturnsNull_WhenTitleDoesNotExist()
    {
        // Act
        var nonExistentTitle = "Event 404";

        var result = await _dbUnitOfWork.Events.GetByTitleAsync(
            nonExistentTitle,
            CancellationToken.None
        );

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByTitleAsync_IsCaseInSensitive_WhenSearchingTitle()
    {
        // Arrange
        var title = "Event";
        var uppercaseTitle = title.ToUpper();
        var lowercaseTitle = title.ToLower();

        _dbUnitOfWork.Events.Add(EventEntityCreator.Create(title: uppercaseTitle));
        await _dbUnitOfWork.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await _dbUnitOfWork.Events.GetByTitleAsync(
            lowercaseTitle,
            CancellationToken.None
        );

        // Assert
        Assert.NotNull(result);
        Assert.Equal(uppercaseTitle, result.Title);
    }
}
