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
public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    /// <summary>
    /// Initializes a new instance of the <see cref="PhotoService"/> class.
    /// </summary>
    /// <param name="config">Cloudinary configuration settings injected via IOptions.</param>
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    /// <summary>
    /// Uploads a photo file to Cloudinary.
    /// </summary>
    /// <param name="file">The photo file to be uploaded.</param>
    /// <returns>The result of the image upload.</returns>
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
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

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            return new ImageUploadResult
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.Url
            };
        }
        return null!;
    }

    /// <summary>
    /// Deletes a photo from Cloudinary using its public ID.
    /// </summary>
    /// <param name="publicId">The public ID of the photo to be deleted.</param>
    /// <returns>The result of the deletion operation.</returns>
    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result;
    }
}
