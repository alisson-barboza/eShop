using eShop.Core.Messages;
using System;

namespace eShop.Vendas.Application.Events
{
    public class ItemRemovidoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }

        public ItemRemovidoEvent(Guid clienteId, Guid pedidoId, Guid produtoId)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
        }
    }
}