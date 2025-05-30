using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Shared.Dtos.AiDtos
{
    
        public class AIUsageStatusDto
        {
            public bool Allowed { get; set; }
            public int DailyLimit { get; set; }
            public int CurrentCount { get; set; }
            public string Message { get; set; }
        }
    }


