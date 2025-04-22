using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PrimeCare.Application.Helpers;
using PrimeCare.Application.Services.Interfaces;

namespace PrimeCare.Application.Services.Implementations;

/// <summary>
/// Service responsible for handling photo uploads and deletions using Cloudinary.
/// </summary>
public class PhotoService : IPhotoServies
{
    private readonly Cloudinary _Cloudinary;

    /// <summary>
    /// Initializes a new instance of the <see cref="PhotoService"/> class.
    /// </summary>
    /// <param name="config">Cloudinary configuration settings injected via IOptions.</param>
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _Cloudinary = new Cloudinary(acc);
    }

    /// <summary>
    /// Uploads a photo file to Cloudinary.
    /// </summary>
    /// <param name="file">The photo file to be uploaded.</param>
    /// <returns>The result of the image upload.</returns>
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if (file.Length > 0)
        {
            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation()
                    .Height(500)
                    .Width(500)
                    .Crop("fill")
                    .Gravity("face")
            };

            uploadResult = await _Cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }

    /// <summary>
    /// Deletes a photo from Cloudinary using its public ID.
    /// </summary>
    /// <param name="publicId">The public ID of the photo to be deleted.</param>
    /// <returns>The result of the deletion operation.</returns>
    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _Cloudinary.DestroyAsync(deleteParams);
        return result;
    }
}
