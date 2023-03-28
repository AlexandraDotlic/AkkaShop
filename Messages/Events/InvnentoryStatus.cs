﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class InventoryStatus
    {
        public int ProductId { get; }
        public int AvailableQuantity { get; }

        public InventoryStatus(int productId, int availableQuantity)
        {
            ProductId = ProductId;
            AvailableQuantity = availableQuantity;
        }
    }
}