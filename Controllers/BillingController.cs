using Microsoft.AspNetCore.Mvc;
using BasicBilling.API.Data;
using BasicBilling.API.Models;
using System;
using System.Linq;
using BasicBilling.API.Models.Enums;


namespace BasicBilling.API.Controllers
{
    [Route("billing")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly DataContext _context;

        public BillingController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("pending")]
        public IActionResult GetPendingBillsByClientId(int clientId)
        {
            var pendingBills = _context.Bills
                .Where(b => b.ClientId == clientId && b.State == BillState.Pending)
                .ToList();

            if (pendingBills == null || pendingBills.Count == 0)
            {
                return NotFound("No pending bills found for the specified client.");
            }

            return Ok(pendingBills);
        }


        [HttpPost("bills")]
        public IActionResult CreateBill([FromBody] BillCreationRequest request)
        {
            var year = request.Period / 100;
            var month = request.Period % 100;

            if (year < 1 || year > 9999 || month < 1 || month > 12)
            {
                return BadRequest("Invalid period format. Year must be between 1 and 9999, and month must be between 1 and 12.");
            }

            var newBill = new Bill
            {
                ClientId = request.ClientId,
                Category = request.Category,
                MonthYear = new DateTime(year, month, 1),
                State = BillState.Pending,
                Amount = request.Amount 
            };

            _context.Bills.Add(newBill);
            _context.SaveChanges();

            return CreatedAtAction(nameof(CreateBill), new { id = newBill.Id }, newBill);
        }


        [HttpPost("pay")]
        public IActionResult ProcessPayment([FromBody] BillPaymentRequest request)
        {
            var billToPay = _context.Bills
                .FirstOrDefault(b => b.ClientId == request.ClientId &&
                                    b.MonthYear.Year == request.Period / 100 &&
                                    b.MonthYear.Month == request.Period % 100 &&
                                    b.Category == request.Category && 
                                    b.State == BillState.Pending);

            if (billToPay == null)
            {
                return BadRequest("No pending bill found for the specified criteria.");
            }

            billToPay.State = BillState.Paid;
            _context.SaveChanges();

            return Ok("Payment processed successfully.");
        }

        [HttpGet("search")]
        public IActionResult SearchBillsByCategory(string category)
        {
            var bills = _context.Bills
                .Where(b => b.Category == category)
                .ToList();

            return Ok(bills);
        }
        
        [HttpGet("client/{clientId}")]
        public IActionResult GetBillsByClientId(int clientId)
        {
        var bills = _context.Bills
        .Where(b => b.ClientId == clientId)
        .ToList();

        return Ok(bills);
        }
        [HttpGet("{id}")]
        public IActionResult GetBillById(int id)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.Id == id);

            if (bill == null)
            {
                return NotFound();
            }

            return Ok(bill);
        }
        [HttpPost("clients")]
        public IActionResult CreateClient([FromBody] ClientCreationRequest request)
        {
            var newClient = new Client
            {
                Id = request.ClientId,
                Name = request.Name
            };

            _context.Clients.Add(newClient);
            _context.SaveChanges();

            return CreatedAtAction(nameof(CreateClient), new { id = newClient.Id }, newClient);
        }

    }
}
