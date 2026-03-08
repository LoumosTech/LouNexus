using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Core
{
    public class PartMeasurement
    {
        public PartMeasurement() { }

        public PartMeasurement(int partMeasurementId, string partMeasurementName, bool isActive, DateTime createdUtc)
        {
            PartMeasurementId = partMeasurementId;
            PartMeasurementName = partMeasurementName;
            IsActive = isActive;
            CreatedUtc = createdUtc;
        }

        public int PartMeasurementId { get; set; }
        public string PartMeasurementName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
