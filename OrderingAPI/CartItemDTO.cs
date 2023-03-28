using System.Runtime.Serialization;

namespace OrderingAPI
{
    [DataContract]
    public class CartItemDTO
    {
        [DataMember]
        public int ProductId { get;  set; }
        [DataMember]

        public int Quantity { get;  set; }
        [DataMember]

        public decimal Price { get; set; }

    }
}
