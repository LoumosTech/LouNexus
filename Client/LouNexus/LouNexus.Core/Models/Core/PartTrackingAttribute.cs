using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Core
{
    public class PartTrackingAttribute
    {
        public PartTrackingAttribute() { }

        public PartTrackingAttribute(int partTrackingAttributeId, int partId, int workStationTypeId, string attributeName, bool isRequired, int sortOrder, bool isActive, DateTime createdUtc)
        {
            PartTrackingAttributeId = partTrackingAttributeId;
            PartId = partId;
            WorkStationTypeId = workStationTypeId;
            AttributeName = attributeName;
            IsRequired = isRequired;
            SortOrder = sortOrder;
            IsActive = isActive;
            CreatedUtc = createdUtc;
        }

        public int PartTrackingAttributeId { get; set; }
        public int PartId { get; set; }
        public int WorkStationTypeId { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
