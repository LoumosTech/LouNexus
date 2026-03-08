using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Core
{
    public class RejectCode
    {
        public RejectCode() { }

        public RejectCode(int rejectCodeId, string code, string description, bool isActive, DateTime createdUtc)
        {
            RejectCodeId = rejectCodeId;
            Code = code;
            Description = description;
            IsActive = isActive;
            CreatedUtc = createdUtc;
        }
        public int RejectCodeId { get; set; }
        public string Code { get; set; } = string.Empty;    
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
