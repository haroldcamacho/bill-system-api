using System;
using System.ComponentModel.DataAnnotations;
using BasicBilling.API.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicBilling.API.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")] 
        public int ClientId { get; set; } 

        public Client? Client { get; set; } 

        public string Category { get; set; } = null!;
        public DateTime MonthYear { get; set; }
        public BillState State { get; set; }
        public decimal Amount { get; set; }
    }

}
