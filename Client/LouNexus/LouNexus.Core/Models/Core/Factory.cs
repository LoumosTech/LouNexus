using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Core
{
    // factory class to reperesent a factory object.
    public class Factory
    {
        public Factory() { }

        public Factory(int id, string name, bool isActive, DateTime createdUtc)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            CreatedUtc = createdUtc;
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
