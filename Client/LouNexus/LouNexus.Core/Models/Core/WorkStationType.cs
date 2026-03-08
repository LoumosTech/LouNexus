using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Core
{
    public class WorkStationType
    {
            public WorkStationType() { }
        public WorkStationType(int workStationTypeId, string name, string description, bool isActive, DateTime createdUtc)
        {
            WorkStationTypeId = workStationTypeId;
            Name = name;
            Description = description;
            IsActive = isActive;
            CreatedUtc = createdUtc;
        }
        public int WorkStationTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
