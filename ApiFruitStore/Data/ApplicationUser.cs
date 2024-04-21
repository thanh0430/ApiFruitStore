using Microsoft.AspNetCore.Identity;

namespace ApiFruitStore.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Email {  get; set; }
        public string Password { get; set; }
    }
}
