using Microsoft.Extensions.Logging;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities.AiEntites;
using PrimeCare.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Application.Services.Implementations
{
    public class AuditService : IAuditService
    {
        private readonly PrimeCareContext _context;
        private readonly ILogger<AuditService> _logger;

        public AuditService(PrimeCareContext context, ILogger<AuditService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task LogAIInteractionAsync(string userId, string query, string assistantType)
        {
            var auditLog = new AIInteractionAudit
            {
                UserId = userId,
                Query = query,
                AssistantType = assistantType,
                Timestamp = DateTime.UtcNow,
                IPAddress = GetClientIPAddress()
            };

            _context.AIInteractionAudits.Add(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task LogAIResponseAsync(string userId, string query, string response)
        {
            var responseLog = new AIResponseAudit
            {
                UserId = userId,
                Query = query,
                Response = response,
                Timestamp = DateTime.UtcNow
            };

            _context.AIResponseAudits.Add(responseLog);
            await _context.SaveChangesAsync();
        }

        private string GetClientIPAddress()
        {
            // Implementation to get client IP
            return "127.0.0.1"; // Placeholder
        }
    }

}
