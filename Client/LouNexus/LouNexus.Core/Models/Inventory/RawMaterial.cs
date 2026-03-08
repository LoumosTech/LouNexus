using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Models.Inventory
{
    public class RawMaterial
    {
        public RawMaterial() { }

        public RawMaterial(int rawMaterialId, int partId, decimal quantity, string lotNumber, string materialDescription, int? factoryId, bool isActive, DateTime createdUtc)
        {
            RawMaterialId = rawMaterialId;
            PartId = partId;
            Quantity = quantity;
            LotNumber = lotNumber;
            MaterialDescription = materialDescription;
            FactoryId = factoryId;
            IsActive = isActive;
            CreatedUtc = createdUtc;
        }

        public int RawMaterialId { get; set; }
        public int PartId { get; set; }
        public decimal Quantity { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public string MaterialDescription { get; set; } = string.Empty;
        public int? FactoryId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
