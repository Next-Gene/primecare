using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Shared.Dtos.AiDtos
{
    public class EmergencyContactsResponse
    {
        public string EmergencyNumber { get; set; }
        public string PoisonControl { get; set; }
        public string MentalHealthCrisis { get; set; }
        public string Message { get; set; }
    }
}
