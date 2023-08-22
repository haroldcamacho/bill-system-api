using System.Collections.Generic;
using System.Linq;
using BasicBilling.API.Controllers;
using BasicBilling.API.Models;
using BasicBilling.API.Models.Enums;
using BasicBilling.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace BasicBilling.API.Tests
{
    [TestFixture]
    public class BillingControllerTests
    {
        private Mock<IBillingService>? _mockBillingService; 
        private BillingController? _controller; 

        [SetUp]
        public void Setup()
        {
            _mockBillingService = new Mock<IBillingService>();
            _controller = new BillingController(_mockBillingService.Object);
        }

        [Test]
        public void GetPendingBillsByClientIdNoPendingBillsReturnsNotFound()
        {
            var clientId = 100;
            _mockBillingService!.Setup(service => service.GetPendingBillsByClientId(clientId))
                                .Returns(new List<Bill>());
                                
            var result = _controller!.GetPendingBillsByClientId(clientId);


            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.AreEqual("No pending bills found for the specified client.", notFoundResult.Value);
        }
        [Test]
        public void GetPendingBillsByClientIdReturnsValidPayload()
        {
            var clientId = 100;
            var category = "Electricity";
            var pendingBills = new List<Bill>{
                new Bill {Id = 1, ClientId = clientId, Category = category, State = BillState.Pending },
                new Bill {Id = 2, ClientId = clientId, Category = category, State = BillState.Pending }
            };
            _mockBillingService!.Setup(service => service.GetPendingBillsByClientId(clientId))
                    .Returns(pendingBills);

            var result = _controller!.GetPendingBillsByClientId(clientId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(pendingBills, okResult.Value);
        }

         [Test]
        public void CreateClient_ValidRequest_ReturnsCreated()
        {
            var request = new ClientCreationRequest
            {
                ClientId = 101,
                Name = "Test Client"
            };
            var newClient = new Client
            {
                Id = request.ClientId,
                Name = request.Name
            };
            _mockBillingService.Setup(service => service.CreateClient(request))
                               .Returns(newClient);
            var result = _controller!.CreateClient(request);

            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = (CreatedAtActionResult)result;
            Assert.AreEqual(nameof(BillingController.CreateClient), createdResult.ActionName);
            Assert.AreEqual(newClient.Id, createdResult.RouteValues["id"]);
            Assert.AreEqual(newClient, createdResult.Value);
        }

        [Test]
        public void ProcessPayment_ExistingBill_ReturnsOk()
        {
            var request = new BillPaymentRequest
            {
                ClientId = 101,
                Period = 202312,
                Category = "Electricity"
            };
            _mockBillingService.Setup(service => service.ProcessPayment(request))
                .Returns(true);

            var result = _controller.ProcessPayment(request);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual("Payment processed successfully.", okResult.Value);
        }

        [Test]
        public void ProcessPayment_NonExistingBill_ReturnsBadRequest()
        {
            var request = new BillPaymentRequest
            {
                ClientId = 101,
                Period = 202312,
                Category = "Electricity"
            };
            _mockBillingService.Setup(service => service.ProcessPayment(request))
                .Returns(false);

            var result = _controller.ProcessPayment(request);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual("No pending bill found for the specified criteria.", badRequestResult.Value);
        }

        [Test]
        public void CreateBill_ValidRequest_ReturnsCreated()
        {
            var request = new BillCreationRequest
            {
                ClientId = 101,
                Category = "Electricity",
                Period = 202312,
                Amount = 100.0m
            };
            var newBill = new Bill
            {
                Id = 1,
                ClientId = request.ClientId,
                Category = request.Category,
                MonthYear = new DateTime(2023, 12, 1),
                State = BillState.Pending,
                Amount = request.Amount
            };
            _mockBillingService.Setup(service => service.CreateBill(request))
                               .Returns(newBill);

            var result = _controller.CreateBill(request);

            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = (CreatedAtActionResult)result;
            Assert.AreEqual(nameof(BillingController.CreateBill), createdResult.ActionName);
            Assert.AreEqual(newBill.Id, createdResult.RouteValues["id"]);
            Assert.AreEqual(newBill, createdResult.Value);
        }

        [Test]
        public void SearchBillsByCategory_ValidCategory_ReturnsOkWithBills()
        {
            var category = "Electricity";
            var bills = new List<Bill>
            {
                new Bill { Id = 1, ClientId = 101, Category = category, State = BillState.Pending, Amount = 100.0m },
                new Bill { Id = 2, ClientId = 102, Category = category, State = BillState.Pending, Amount = 150.0m }
            };
            _mockBillingService.Setup(service => service.SearchBillsByCategory(category))
                .Returns(bills);

            var result = _controller.SearchBillsByCategory(category);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(bills, okResult.Value);
        }

        [Test]
        public void SearchBillsByCategory_NonExistingCategory_ReturnsEmptyList()
        {
            var nonExistingCategory = "NonExistingCategory";
            _mockBillingService.Setup(service => service.SearchBillsByCategory(nonExistingCategory))
                .Returns(new List<Bill>());

            var result = _controller.SearchBillsByCategory(nonExistingCategory);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsEmpty((List<Bill>)okResult.Value);
        }

        [Test]
        public void GetBillsByClientId_ReturnsValidPayload()
        {
            var clientId = 100;
            var bills = new List<Bill>
            {
                new Bill { Id = 1, ClientId = clientId, Category = "Electricity", State = BillState.Pending },
                new Bill { Id = 2, ClientId = clientId, Category = "Water", State = BillState.Paid },
            };
            _mockBillingService.Setup(service => service.GetBillsByClientId(clientId))
                .Returns(bills);

            var result = _controller.GetBillsByClientId(clientId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(bills, okResult.Value);
        }
        [Test]
        public void CreateBill_NonExistingClient_ReturnsBadRequest()
        {
            var clientId = 999; 
            var request = new BillCreationRequest
            {
                ClientId = clientId,
                Category = "Electricity",
                Period = 202311, 
                Amount = 50.0m
            };

            _mockBillingService.Setup(service => service.CreateBill(request))
                .Throws(new ArgumentException("Client not found."));

            var result = _controller!.CreateBill(request);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.AreEqual("Client not found.", badRequestResult.Value);
        }

    }
}
