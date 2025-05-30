using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Shared.Dtos.AiDtos
{
    public class MedicalAIResponse
    {
        public string Response { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string QueryId { get; set; } = string.Empty;
        public bool IsEmergencyDetected { get; set; }
        public List<string>? Disclaimers { get; set; }
        public List<string>? RecommendedActions { get; set; }
        public AIConfidenceLevel ConfidenceLevel { get; set; }
    }
}
