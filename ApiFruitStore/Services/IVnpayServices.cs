using ApiFruitStore.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ApiFruitStore.Services
{
    public interface IVnpayServices
    {
        Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collection);
    }
}