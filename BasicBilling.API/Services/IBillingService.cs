using System.Collections.Generic;
using BasicBilling.API.Models;

namespace BasicBilling.API.Services
{
    public interface IBillingService
    {
        List<Bill> GetPendingBillsByClientId(int clientId);
        Bill CreateBill(BillCreationRequest request);
        bool ProcessPayment(BillPaymentRequest request);
        List<Bill> SearchBillsByCategory(string category);
        List<Bill> GetBillsByClientId(int clientId);
        Bill GetBillById(int id);
        Client CreateClient(ClientCreationRequest request);
        List<Client> GetAllClients();
        List<string> GetUniqueCategories();

    }
}
