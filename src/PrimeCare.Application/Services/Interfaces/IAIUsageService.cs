using PrimeCare.Shared.Dtos.AiDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface IAIUsageService
    {
        Task<bool> CheckAIUsageLimitAsync(string userId);
        Task UpdateAIUsageAsync(string userId);
        Task<int> GetTodayUsageCountAsync(string userId);
        Task<AIUsageStatusDto> GetUsageStatusAsync(string userId);
    }

}
