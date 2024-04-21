using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFruitStore.Data
{
    [Table("Warehouse")]
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }
        public int Productname { get; set;}
        public string? Description { get; set;}
        public string Image { get; set; }
        public int Quantity { get; set;}
        public float? Price { get; set;}
        public string? Unit {  get; set;}
        public DateTime? Datereceived {  get; set; }    
    }
}
