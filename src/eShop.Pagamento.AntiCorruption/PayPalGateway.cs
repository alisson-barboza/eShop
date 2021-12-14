using System;
using System.Linq;

namespace eShop.Pagamentos.AntiCorruption
{
    public class PayPalGateway : IPaypalGateway
    {
        public bool CommitTransaction(string cardHashKey, string orderId, decimal amout)
        {
            return new Random().Next(2) == 0;
        }

        public string GetCardHashKey(string serviceKey, string cartaoCredito)
        {
            return new string(Enumerable.Repeat("ABCEEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                 .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public string GetPayPalServiceKey(string apiKey, string encriptionKey)
        {
            return new string(Enumerable.Repeat("ABCEEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}