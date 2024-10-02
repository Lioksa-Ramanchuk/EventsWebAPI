using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Domain.Exceptions.MediaExceptions;
using Events.Domain.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SmartFormat;

namespace Events.Infrastructure.Services;

public class MediaService(
    IOptions<AppSettings> appSettings,
    IWebHostEnvironment webHostEnvironment,
    IHttpContextAccessor httpContextAccessor
) : IMediaService
{
    private readonly MediaSettings _mediaSettings = appSettings.Value.MediaSettings;

    public string GetImagesUploadPath()
    {
        return Path.Combine(_mediaSettings.UploadPath, _mediaSettings.ImagesUploadSubPath);
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile, CancellationToken ct)
    {
        if (imageFile is null || imageFile.Length == 0)
        {
            throw new InvalidFileUploadException(ExceptionMessages.NoFileUploaded);
        }

        var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        if (!_mediaSettings.ValidImageExtensions.Contains(fileExtension))
        {
            throw new InvalidFileUploadException(
                Smart.Format(
                    ExceptionMessages.InvalidImageFileExtensionWithValue,
                    new { fileExtension }
                )
            );
        }

        var fileContentType = imageFile.ContentType;
        if (!fileContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidFileUploadException(
                Smart.Format(
                    ExceptionMessages.InvalidImageFileContentTypeWithValue,
                    new { fileContentType }
                )
            );
        }

        return await UploadFileAsync(imageFile, _mediaSettings.ImagesUploadSubPath, ct);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string? subPath, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
        {
            throw new InvalidFileUploadException(ExceptionMessages.NoFileUploaded);
        }

        var targetUploadPath = Path.Combine(
            webHostEnvironment.WebRootPath,
            _mediaSettings.UploadPath
        );
        if (!string.IsNullOrWhiteSpace(subPath))
        {
            targetUploadPath = Path.Combine(targetUploadPath, subPath);
        }

        if (!Directory.Exists(targetUploadPath))
        {
            Directory.CreateDirectory(targetUploadPath);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var uploadedFilePath = Path.Combine(targetUploadPath, uniqueFileName);

        using var stream = new FileStream(uploadedFilePath, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        return uniqueFileName;
    }

    public void RemoveImage(string imageFileName)
    {
        ArgumentException.ThrowIfNullOrEmpty(imageFileName, nameof(imageFileName));

        var imageFilePath = Path.Combine(
            webHostEnvironment.WebRootPath,
            _mediaSettings.UploadPath,
            _mediaSettings.ImagesUploadSubPath,
            imageFileName
        );

        if (!File.Exists(imageFilePath))
        {
            throw new ImageNotFoundException(
                Smart.Format(ExceptionMessages.ImageWithFileNameNotFound, new { imageFileName })
            );
        }
        File.Delete(imageFilePath);
    }

    public string GetImageUrlInHttpContext(string imageFileName)
    {
        ArgumentException.ThrowIfNullOrEmpty(imageFileName, nameof(imageFileName));

        var request =
            httpContextAccessor.HttpContext?.Request
            ?? throw new SystemException(ExceptionMessages.HttpContextNotAvailable);

        return $"{request.Scheme}://{request.Host}/{Path.Combine(_mediaSettings.UploadPath, _mediaSettings.ImagesUploadSubPath, imageFileName)}";
    }
}
