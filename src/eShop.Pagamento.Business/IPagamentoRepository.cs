using eShop.Core.Data;

namespace eShop.Pagamentos.Business
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        void Add(Pagamento pagamento);

        void Add(Transacao transacao);
    }
}