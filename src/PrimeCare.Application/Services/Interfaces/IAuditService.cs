using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface IAuditService
    {
        Task LogAIInteractionAsync(string userId, string query, string assistantType);
        Task LogAIResponseAsync(string userId, string query, string response);
    }
}
