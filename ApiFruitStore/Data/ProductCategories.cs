using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFruitStore.Data
{

    [Table("ProductCategories")]
    public class ProductCategories
    {
        [Key]
        public int Id { get; set; }

        public string productcategory { get; set; }
        public string Image {  get; set; }

        public string? Description { get; set; }
    }
}
