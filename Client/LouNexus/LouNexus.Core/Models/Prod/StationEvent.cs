using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Prod
{
    public class StationEvent
    {
        public StationEvent() { }

        public StationEvent(int stationEventId, int inspectionId, int workStationId, string eventType, int goodQuantity, string notes, DateTime? eventTimeUtc, DateTime createdUtc)
        {
            StationEventId = stationEventId;
            InspectionId = inspectionId;
            WorkStationId = workStationId;
            EventType = eventType;
            GoodQuantity = goodQuantity;
            Notes = notes;
            EventTimeUtc = eventTimeUtc;
            CreatedUtc = createdUtc;
        }

        public int StationEventId { get; set; }
        public int InspectionId { get; set; }
        public int WorkStationId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public int GoodQuantity { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime? EventTimeUtc { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
