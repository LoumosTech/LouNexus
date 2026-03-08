using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Prod
{
    public class StationEventAttribute
    {
        public StationEventAttribute() { }

        public StationEventAttribute(int stationEventId, int partTrackingAttributeId, string attributeValue, string notes)
        {
            StationEventId = stationEventId;
            PartTrackingAttributeId = partTrackingAttributeId;
            AttributeValue = attributeValue;
            Notes = notes;
        }

        public int StationEventAttributeId { get; set; }
        public int StationEventId { get; set; }
        public int PartTrackingAttributeId { get; set; }
        public string AttributeValue { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
