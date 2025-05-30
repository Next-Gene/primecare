using PrimeCare.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Core.Entities.AiEntites
{
    public class AIUsageTracking
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public int RequestCount { get; set; }
        public DateTime LastRequestTime { get; set; }

        // Navigation property
    }
}
