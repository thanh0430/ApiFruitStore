using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFruitStore.Data
{

    [Table("ProductCategories")]
    public class ProductCategories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string productcategory { get; set; }
        public string Image {  get; set; }
        public string? Description { get; set; }
      
    }
}
