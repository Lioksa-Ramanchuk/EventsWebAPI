namespace Events.Application.Configuration.Settings;

public class MediaSettings
{
    public string UploadPath { get; set; } = null!;
    public string ImagesUploadSubPath { get; set; } = null!;
    public string[] ValidImageExtensions { get; set; } = null!;
}
