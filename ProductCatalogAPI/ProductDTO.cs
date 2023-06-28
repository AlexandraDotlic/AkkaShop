using System.Runtime.Serialization;

namespace ProductCatalogAPI
{
    [DataContract]
    public class ProductDTO
    {
        [DataMember]
        public int productId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public int Inventory { get; set; }

    }
}
