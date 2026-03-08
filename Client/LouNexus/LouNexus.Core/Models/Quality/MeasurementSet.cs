using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Quality
{
    public class MeasurementSet
    {
        public MeasurementSet() { }

        public MeasurementSet(int measurementSetId, int stationEventId, int partMeasurementSpecId, string notes, DateTime createdUtc)
        {
            MeasurementSetId = measurementSetId;
            StationEventId = stationEventId;
            PartMeasurementSpecId = partMeasurementSpecId;
            Notes = notes;
            CreatedUtc = createdUtc;
        }

        public int MeasurementSetId { get; set; }
        public int StationEventId { get; set; }
        public int PartMeasurementSpecId { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
