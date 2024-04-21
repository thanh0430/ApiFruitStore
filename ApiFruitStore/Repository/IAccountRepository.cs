using ApiFruitStore.Models;

namespace ApiFruitStore.Repository
{
    public interface IAccountRepository
    {
        public Task<String> SignInAsync(SignIn Models);
    }
}
