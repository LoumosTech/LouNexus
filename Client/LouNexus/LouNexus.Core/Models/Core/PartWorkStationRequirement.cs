using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Core
{
    public class PartWorkStationRequirement
    {
        public PartWorkStationRequirement() { }
        public PartWorkStationRequirement(int partWorkStationRequirementId, int partId, int workStationTypeId, int sequenceOrder, bool isRequired, DateTime createdUtc)
        {
            PartWorkStationRequirementId = partWorkStationRequirementId;
            PartId = partId;
            WorkStationTypeId = workStationTypeId;
            SequenceOrder = sequenceOrder;
            IsRequired = isRequired;
            CreatedUtc = createdUtc;
        }
        public int PartWorkStationRequirementId { get; set; }
        public int PartId { get; set; }
        public int WorkStationTypeId { get; set; }
        public int SequenceOrder { get; set; }
        public bool IsRequired { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
