using System.ComponentModel.DataAnnotations;

namespace ApiFruitStore.Models
{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }

    }
    public class VnPaymentRequestModel
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public Double Amount { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
    }

}
