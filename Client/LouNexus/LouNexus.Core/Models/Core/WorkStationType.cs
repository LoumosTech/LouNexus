using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Core
{
    public class WorkStationType
    {
        public WorkStationType() { }

        public WorkStationType(int workStationTypeId, string name, string code, bool supportsCpk, bool supportsRejectEntry, bool supportsTrackingAttributes, bool supportsClockOut, bool isActive, DateTime createdUtc)
        {
            WorkStationTypeId = workStationTypeId;
            WorkStationTypeName = name;
            WorkStationTypeCode = code;
            SupportsCpk = supportsCpk;
            SupportsRejectEntry = supportsRejectEntry;
            SupportsTrackingAttributes = supportsTrackingAttributes;
            SupportsClockOut = supportsClockOut;
            IsActive = isActive;
            CreatedUtc = createdUtc;
        }

        public int WorkStationTypeId { get; set; }
        public string WorkStationTypeName { get; set; } = string.Empty;
        public string WorkStationTypeCode { get; set; } = string.Empty;
        public bool SupportsCpk { get; set; } = false;
        public bool SupportsRejectEntry { get; set; } = true;
        public bool SupportsTrackingAttributes { get; set; } = false;
        public bool SupportsClockOut { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    }
}
