using Events.Tests.Common.Data;
using Events.Tests.Common.Fixtures;

namespace Events.Tests.UnitTests.Infrastructure.Data.Repositories.EventRepositoryTests;

public abstract class EventRepositoryTests(DbFixture dbFixture) : DbTestBase(dbFixture) { }
