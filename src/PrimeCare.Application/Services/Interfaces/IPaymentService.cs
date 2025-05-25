using PrimeCare.Core.Entities;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerCart> CreateOrUpdatePaymentIntent(string userId);
        Task<string> CreateCheckoutSessionAsync(string userId);


    }
}
