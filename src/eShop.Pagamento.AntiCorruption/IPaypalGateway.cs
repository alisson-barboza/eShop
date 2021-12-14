namespace eShop.Pagamentos.AntiCorruption
{
    public interface IPaypalGateway
    {
        string GetPayPalServiceKey(string apiKey, string encriptionKey);

        string GetCardHashKey(string serviceKey, string cartaoCredito);

        bool CommitTransaction(string cardHashKey, string orderId, decimal amout);
    }
}