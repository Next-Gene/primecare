using PrimeCare.Core.Entities.Identity;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface ITokenService
    {

        string CreateToken(ApplicationUser applicationUser);
        Task<string> CreateTokenAsync(ApplicationUser applicationUser);
    }
}
