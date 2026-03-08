using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Prod
{
    public class Inspection
    {
        public Inspection() { }

        public Inspection(int inspectionId, string inspectorNumber, int factoryId, int partId, int initialQuality, string status, string notes, DateTime createdUtc, DateTime? clodedUtc)
            {
                InspectionId = inspectionId;
                InspectorNumber = inspectorNumber;
                FactoryId = factoryId;
                PartId = partId;
                InitialQuality = initialQuality;
                Status = status;
                Notes = notes;
                CreatedUtc = createdUtc;
                ClodedUtc = clodedUtc;
        }

        public int InspectionId { get; set; }
        public string InspectorNumber { get; set; } = string.Empty;
        public int FactoryId { get; set; }
        public int PartId { get; set; }
        public int InitialQuality { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ClodedUtc { get; set; }
    }
}
