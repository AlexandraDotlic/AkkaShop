using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartCoordinatorService
{
    public class OrderCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string OrderId { get; set; }
    }
}
