using ApiFruitStore.Data;
using ApiFruitStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiFruitStore.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountRepository( UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration) 
        { 
            this.userManager = userManager; ;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }      
        public async Task<string> SignInAsync(SignIn Models)
        {
            var result = await signInManager.PasswordSignInAsync
                 (Models.Email, Models.Password, false, false);
            if(result.Succeeded)
            {
                return string.Empty;
            }
            var authenClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, Models.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
           
            };
            var authenkey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(configuration["jwt:Secret"]));
            var token = new JwtSecurityToken(
                issuer: configuration["jwt:ValidIssuer"],
                audience: configuration["jwt:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authenClaims,
                signingCredentials: new SigningCredentials(authenkey, 
                SecurityAlgorithms.HmacSha256Signature)
               );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
 
    }
}
