using Microsoft.AspNetCore.Mvc;
using BasicBilling.API.Services;
using BasicBilling.API.Models;
using System;

namespace BasicBilling.API.Controllers
{
    [Route("billing")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpGet("pending")]
        public IActionResult GetPendingBillsByClientId(int clientId)
        {
            var pendingBills = _billingService.GetPendingBillsByClientId(clientId);

            if (pendingBills == null || pendingBills.Count == 0)
            {
                return NotFound("No pending bills found for the specified client.");
            }

            return Ok(pendingBills);
        }

        [HttpPost("bills")]
        public IActionResult CreateBill([FromBody] BillCreationRequest request)
        {
            try
            {
                var newBill = _billingService.CreateBill(request);
                return CreatedAtAction(nameof(CreateBill), new { id = newBill.Id }, newBill);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("pay")]
        public IActionResult ProcessPayment([FromBody] BillPaymentRequest request)
        {
            var paymentProcessed = _billingService.ProcessPayment(request);
            if (!paymentProcessed)

            {
                return BadRequest("No pending bill found for the specified criteria.");
            }

            return Ok("Payment processed successfully.");
        }

        [HttpGet("search")]
        public IActionResult SearchBillsByCategory(string category)
        {
            var bills = _billingService.SearchBillsByCategory(category);
            return Ok(bills);
        }

        [HttpGet("client/{clientId}")]
        public IActionResult GetBillsByClientId(int clientId)
        {
            var bills = _billingService.GetBillsByClientId(clientId);
            return Ok(bills);
        }

        [HttpGet("{id}")]
        public IActionResult GetBillById(int id)
        {
            var bill = _billingService.GetBillById(id);

            if (bill == null)
            {
                return NotFound();
            }

            return Ok(bill);
        }

        [HttpPost("clients")]
        public IActionResult CreateClient([FromBody] ClientCreationRequest request)
        {
            var newClient = _billingService.CreateClient(request);
            return CreatedAtAction(nameof(CreateClient), new { id = newClient.Id }, newClient);
        }

        [HttpGet("clients")]
        public IActionResult GetAllClients()
        {
            var clients = _billingService.GetAllClients();
            return Ok(clients);
        }

    }
}
