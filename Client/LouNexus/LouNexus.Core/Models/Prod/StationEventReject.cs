using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Prod
{
    public class StationEventReject
    {
        public StationEventReject() { }

        public StationEventReject(int stationEventId, int rejectCodeId, int quantity, string notes, DateTime createdUtc)
        {
            StationEventId = stationEventId;
            RejectCodeId = rejectCodeId;
            Quantity = quantity;
            Notes = notes;
            CreatedUtc = createdUtc;
        }

        public int StationEventRejectId { get; set; }
        public int StationEventId { get; set; }
        public int RejectCodeId { get; set; }
        public int Quantity { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
