using System.Collections.Generic;
using System.Linq;
using BasicBilling.API.Controllers;
using BasicBilling.API.Data;
using BasicBilling.API.Models;
using BasicBilling.API.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace BasicBilling.API.Tests
{
    [TestFixture]
    public class BillingControllerTests
    {
        [Test]
        public void GetPendingBillsByClientId_NoPendingBills_ReturnsNotFound()
        {
            // Arrange
            var clientId = 100;
            var bills = new List<Bill>().AsQueryable();
            
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            
            using var context = new DataContext(dbContextOptions);
            context.Database.EnsureCreated();
            
            context.Bills.AddRange(bills);
            context.SaveChanges();

            var controller = new BillingController(context);

            // Act
            var result = controller.GetPendingBillsByClientId(clientId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.AreEqual("No pending bills found for the specified client.", notFoundResult.Value);
        }

        // You can write more test cases for different scenarios
    }
}
