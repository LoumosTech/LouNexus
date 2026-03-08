using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Core
{
    public class WorkStation
    {
        public WorkStation() { }
        public WorkStation(int workStationId, string name, int factoryId, int workStationTypeId, bool isActive, DateTime createdUtc)
        {
            WorkStationId = workStationId;
            WorkStationName = name;
            FactoryId = factoryId;
            WorkStationTypeId = workStationTypeId;
            IsActive = isActive;
            CreatedUtc = createdUtc;
        }
        public int WorkStationId { get; set; }
        public string WorkStationName { get; set; } = string.Empty;
        public string WorkStationCode { get; set; } = string.Empty;
        public string WorkStationMode{ get; set; } = string.Empty;
        public int FactoryId { get; set; }
        public int WorkStationTypeId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
