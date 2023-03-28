using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Commands
{
    public class CreateOrder
    {
        public CreateOrder(Cart cart)
        {
            Cart = cart;
        }

        public Cart Cart { get; }
       
    }
}
