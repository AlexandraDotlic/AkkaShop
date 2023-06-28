using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class OrderResult
    {
        public OrderResult(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; set; }
    }
}
