using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Quality
{
    public class MeasurementValue
    {
        public MeasurementValue() { }

        public int MeasurementValueId { get; set; }
        public int MeasurementSetId { get; set; }
        public int SampleIndex { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
