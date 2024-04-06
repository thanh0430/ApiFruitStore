using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFruitStore.Data
{
    public class OrderDetails
    {
        public int Id { get; set; }

        public int IdOrder { get; set; }
        
        public int  IdProduct { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("IdOrder")]
        public Orders Orders { get; set;}

        [ForeignKey("IdProduct")]
        public Products Products { get; set; }
    }
}
