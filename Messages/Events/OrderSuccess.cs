using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class OrderSuccess
    {
        private Guid Id;

        public OrderSuccess(Guid id)
        {
            Id = id;
        }

        public Guid OrderId { get; set; }
    }
}
