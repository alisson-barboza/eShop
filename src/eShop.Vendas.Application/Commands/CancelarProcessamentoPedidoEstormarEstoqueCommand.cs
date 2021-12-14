using eShop.Core.Messages;
using System;

namespace eShop.Vendas.Application.Commands
{
    public class CancelarProcessamentoPedidoEstormarEstoqueCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }

        public CancelarProcessamentoPedidoEstormarEstoqueCommand(Guid clienteId, Guid pedidoId)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
        }
    }
}