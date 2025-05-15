using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PrimeCare.Application.Helpers;
using PrimeCare.Application.Services.Interfaces;

namespace PrimeCare.Application.Services.Implementations;

/// <summary>
/// Service for handling photo uploads and deletions using Cloudinary.
/// </summary>
public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    /// <summary>
    /// Initializes a new instance of the <see cref="PhotoService"/> class.
    /// </summary>
    /// <param name="config">The Cloudinary settings configuration.</param>
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
    /// Asynchronously uploads a photo to Cloudinary.
    /// </summary>
    /// <param name="file">The photo file to upload.</param>
    /// <returns>An <see cref="ImageUploadResult"/> containing the result of the upload operation.</returns>
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
            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }
        return uploadResult;
    }

    /// <summary>
    /// Asynchronously deletes a photo from Cloudinary by its public identifier.
    /// </summary>
    /// <param name="publicId">The public identifier of the photo to delete.</param>
    /// <returns>A <see cref="DeletionResult"/> containing the result of the deletion operation.</returns>
    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result;
    }
}
