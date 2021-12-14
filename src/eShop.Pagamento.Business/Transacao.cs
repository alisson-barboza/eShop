using eShop.Core.DomainObjects;
using System;

namespace eShop.Pagamentos.Business
{
    public class Transacao : Entity
    {
        public Guid PedidoId { get; set; }
        public Guid PagamentoId { get; set; }
        public decimal Total { get; set; }
        public StatusTransacao Status { get; set; }

        public Pagamento Pagamento { get; set; }
    }
}