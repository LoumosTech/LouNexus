using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Core
{
    public class Part
    {
        public Part() { }
        public Part(int partId, string partNumber, string partName, string partDescription, string printUrl, bool isActive, DateTime createdUtc)
        {
            PartId = partId;
            PartNumber = partNumber;
            PartName = partName;
            PartDescription = partDescription;
            PrintUrl = printUrl;
            IsActive = isActive;
            CreatedUtc = createdUtc;
        }
        public int PartId { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string PartName { get; set; } = string.Empty;
        public string PartDescription { get; set; } = string.Empty;
        public string PrintUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
