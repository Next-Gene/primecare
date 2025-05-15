using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace PrimeCare.Application.Services.Interfaces;

/// <summary>
/// Defines the contract for photo-related operations such as uploading and deleting photos using Cloudinary.
/// </summary>
public interface IPhotoService
{
    /// <summary>
    /// Asynchronously uploads a photo to Cloudinary.
    /// </summary>
    /// <param name="file">The photo file to upload.</param>
    /// <returns>An <see cref="ImageUploadResult"/> containing the result of the upload operation.</returns>
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

    /// <summary>
    /// Asynchronously deletes a photo from Cloudinary by its public identifier.
    /// </summary>
    /// <param name="publicId">The public identifier of the photo to delete.</param>
    /// <returns>A <see cref="DeletionResult"/> containing the result of the deletion operation.</returns>
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}
