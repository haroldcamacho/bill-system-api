namespace BasicBilling.API.Models
{
    public class BillSearchRequest
    {
        public int ClientId { get; set; }
        public int Period { get; set; }
        public string? Category { get; set; }
    }
}
