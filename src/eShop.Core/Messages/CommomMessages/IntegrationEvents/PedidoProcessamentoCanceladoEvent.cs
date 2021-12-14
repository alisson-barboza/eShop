using eShop.Core.Messages.Dtos;
using System;

namespace eShop.Core.Messages.CommomMessages.IntegrationEvents
{
    public class PedidoProcessamentoCanceladoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public ItensPedido ItensPedido { get; private set; }

        public PedidoProcessamentoCanceladoEvent(Guid clienteId, Guid pedidoId, ItensPedido itensPedido)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ItensPedido = itensPedido;
        }
    }
}