using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Events.Tests.Common.Configuration;

public class TestWebHostEnvironment : IWebHostEnvironment
{
    public TestWebHostEnvironment()
    {
        WebRootPath = Path.Combine(AppContext.BaseDirectory, "TestWebRoot");
        Directory.CreateDirectory(WebRootPath);
        WebRootFileProvider = new PhysicalFileProvider(WebRootPath);

        ContentRootPath = Path.Combine(AppContext.BaseDirectory, "TestContentRoot");
        Directory.CreateDirectory(ContentRootPath);
        ContentRootFileProvider = new PhysicalFileProvider(ContentRootPath);
    }

    public string ApplicationName { get; set; } = $"{nameof(Events)}.{nameof(Tests)}";
    public string WebRootPath { get; set; }
    public string ContentRootPath { get; set; }
    public string EnvironmentName { get; set; } = "Development";
    public IFileProvider WebRootFileProvider { get; set; }
    public IFileProvider ContentRootFileProvider { get; set; }
}
