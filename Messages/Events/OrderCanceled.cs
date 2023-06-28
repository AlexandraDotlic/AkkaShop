using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class OrderCanceled : OrderResult
    {
        public OrderCanceled(string id): base(id)
        {
        }

    }
}
