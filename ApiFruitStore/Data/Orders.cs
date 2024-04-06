using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ApiFruitStore.Data
{
    [Table("Orders")]
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int  IdCustomers { get; set; }
        [ForeignKey("IdCustomers")]
        public Customers CustomerId { get; set; }
        public float PriceOrders { get; set; }
        public DateTime Orderdate  { get; set; }

    }
}
