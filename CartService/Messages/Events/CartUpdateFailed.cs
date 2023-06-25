using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Messages.Events
{
    public class CartUpdateFailed
    {
        public string ErrorMessage { get; }

        public CartUpdateFailed(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
