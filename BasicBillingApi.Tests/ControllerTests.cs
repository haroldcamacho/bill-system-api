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

    }
}
