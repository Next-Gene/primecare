using PrimeCare.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeCare.Core.Entities.AiEntites
{
    public class AIResponseAudit
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Query { get; set; }
        public string Response { get; set; }
        public DateTime Timestamp { get; set; }


    }
}
