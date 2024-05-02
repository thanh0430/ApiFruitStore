using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFruitStore.Data
{
    [Table("Customers")]
    public class Customers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? PhoneNumber { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password {  get; set; }
        public string? Role { get; set;}
    }
}
