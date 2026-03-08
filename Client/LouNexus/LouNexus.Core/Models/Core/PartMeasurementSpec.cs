using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Core
{
    public class PartMeasurementSpec
    {
        public PartMeasurementSpec() { }
        public PartMeasurementSpec(int partMeasurementSpecId, int partId, int workStationTypeId, string measurementName, decimal targetValue, decimal upperLimit, decimal lowerLimit, bool isActive, DateTime createdUtc)
        {
            PartMeasurementSpecId = partMeasurementSpecId;
            PartId = partId;
            WorkStationTypeId = workStationTypeId;
            MeasurementName = measurementName;
            TargetValue = targetValue;
            UpperLimit = upperLimit;
            LowerLimit = lowerLimit;
            IsActive = isActive;
            CreatedUtc = createdUtc;
        }

        public int PartMeasurementSpecId { get; set; }
        public int PartId { get; set; }
        public int WorkStationTypeId { get; set; }
        public string MeasurementName { get; set; } = string.Empty;
        public decimal TargetValue { get; set; }
        public decimal UpperLimit { get; set; }
        public decimal LowerLimit { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
