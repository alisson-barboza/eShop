using eShop.Core.Messages;
using System;

namespace eShop.Vendas.Application.Events
{
    public class VoucherAplicadoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid VoucherId { get; private set; }

        public VoucherAplicadoEvent(Guid clienteId, Guid pedidoId, Guid voucherId)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            VoucherId = voucherId;
        }
    }
}