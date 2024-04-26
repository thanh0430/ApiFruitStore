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

        public float PriceSum { get; set; }

        public DateTime Orderdate  { get; set; }  

        public string? Image { get; set; } 

        public string? ProductName { get; set; }

        public string Fullname { get; set; }

        public string Address { get; set; }

        public int PhoneNumber { get; set; }
    }
}
