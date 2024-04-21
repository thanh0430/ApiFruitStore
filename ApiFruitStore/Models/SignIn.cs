using System.ComponentModel.DataAnnotations;

namespace ApiFruitStore.Models
{
    public class SignIn
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
