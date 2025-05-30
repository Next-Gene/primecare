using PrimeCare.Shared.Dtos.AiDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface IMedicalAIService
    {
        Task<MedicalAIResponse> GetMedicalAssistanceAsync(string userQuery, string userId);
        Task<bool> ValidateQuerySafetyAsync(string query);
    }
}
