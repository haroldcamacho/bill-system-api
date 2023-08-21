namespace BasicBilling.API.Models
{
    public class BillCreationRequest
    {
        public int ClientId { get; set; }
        public string Category { get; set; } = null!;
        public int Period { get; set; }
        public decimal Amount { get; set; }
    }
}
