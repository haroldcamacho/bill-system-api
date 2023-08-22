using BasicBilling.API.Data;
using BasicBilling.API.Models;
using BasicBilling.API.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicBilling.API.Services
{
    public class BillingService : IBillingService
    {
        private readonly DataContext _context;

        public BillingService(DataContext context)
        {
            _context = context;
        }

        public List<Bill> GetPendingBillsByClientId(int clientId)
        {
            return _context.Bills
                .Where(b => b.ClientId == clientId && b.State == BillState.Pending)
                .ToList();
        }

        private void ValidatePeriod(int year, int month)
        {
            if (year < 1 || year > 9999 || month < 1 || month > 12)
            {
                throw new ArgumentException("Invalid period format. Year must be between 1 and 9999, and month must be between 1 and 12.");
            }
        }

        public Bill CreateBill(BillCreationRequest request)
        {
            var year = request.Period / 100;
            var month = request.Period % 100;

            ValidatePeriod(year, month);

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

            return newBill;
        }

        public bool ProcessPayment(BillPaymentRequest request)
        {
            var billToPay = _context.Bills
                .FirstOrDefault(b => b.ClientId == request.ClientId &&
                                    b.MonthYear.Year == request.Period / 100 &&
                                    b.MonthYear.Month == request.Period % 100 &&
                                    b.Category == request.Category &&
                                    b.State == BillState.Pending);

            if (billToPay == null)
            {
                return false;
            }

            billToPay.State = BillState.Paid;
            _context.SaveChanges();

            return true;
        }

        public List<Bill> SearchBillsByCategory(string category)
        {
            return _context.Bills
                .Where(b => b.Category == category)
                .ToList();
        }

        public List<Bill> GetBillsByClientId(int clientId)
        {
            return _context.Bills
                .Where(b => b.ClientId == clientId)
                .ToList();
        }

        public Bill GetBillById(int id)
        {
            return _context.Bills.FirstOrDefault(b => b.Id == id);
        }

        public Client CreateClient(ClientCreationRequest request)
        {
            var newClient = new Client
            {
                Id = request.ClientId,
                Name = request.Name
            };

            _context.Clients.Add(newClient);
            _context.SaveChanges();

            return newClient;
        }
        public List<Client> GetAllClients()
        {
            return _context.Clients.ToList();
        }

        public List<string> GetUniqueCategories()
        {
            return _context.Bills
                .Select(b => b.Category)
                .Distinct()
                .ToList();
        }
        public List<int> GetUniquePendingBillDates()
        {
            return _context.Bills
                .Where(b => b.State == BillState.Pending)
                .Select(b => b.MonthYear.Year * 100 + b.MonthYear.Month)
                .Distinct()
                .ToList();
        }


    }
}
