namespace BasicBilling.API.Models
{
    public class BillPaymentRequest
    {
        public int ClientId { get; set; }
        public int Period { get; set; }
        public string Category { get; set; } = null!;
    }
}