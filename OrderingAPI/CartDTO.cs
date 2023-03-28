using Domain.Entities;
using System.Runtime.Serialization;

namespace OrderingAPI
{
    [DataContract]
    public class CartDTO
    {
        [DataMember]
        public int Id { get;  set; }
        [DataMember]
        public List<CartItemDTO> CartItems { get; set; }
        public CartDTO()
        {
            CartItems= new List<CartItemDTO>();
        }

    }
}
