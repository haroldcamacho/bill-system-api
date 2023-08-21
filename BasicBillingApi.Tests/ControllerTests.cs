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
        public void GetPendingBillsByClientId_NoPendingBills_ReturnsNotFound()
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
        public void GetGetPendingBillsByClientId_ReturnsValidPayload()
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
    }
}
