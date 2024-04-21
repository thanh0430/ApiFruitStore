using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFruitStore.Data
{
    public class OrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdOrder { get; set; }
        
        public int  IdProduct { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }

      
       
    }
}
