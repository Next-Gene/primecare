
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface IPhotoServies
    {


        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);


    }
}
