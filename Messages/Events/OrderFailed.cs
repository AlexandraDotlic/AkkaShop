using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class OrderFailed
    {
        public OrderFailed(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
