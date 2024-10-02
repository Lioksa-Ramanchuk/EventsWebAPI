using Microsoft.AspNetCore.Http;

namespace Events.Application.Interfaces.Infrastructure;

public interface IMediaService
{
    string GetImagesUploadPath();
    string GetImageUrlInHttpContext(string imageFileName);
    void RemoveImage(string imageFileName);
    Task<string> UploadFileAsync(
        IFormFile file,
        string? subPath = null,
        CancellationToken ct = default
    );
    Task<string> UploadImageAsync(IFormFile imageFile, CancellationToken ct = default);
}
