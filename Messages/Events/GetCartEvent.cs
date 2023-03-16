using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class GetCartEvent
    {
        Cart Cart { get; }

        public GetCartEvent(Cart cart)
        {
            Cart = cart;
        }
    }
}
