using eShop.Core.Messages.CommomMessages.IntegrationEvents;
using eShop.Core.Messages.Dtos;
using System;

namespace eShop.Vendas.Application.Events
{
    public class PedidoIniciadoEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public decimal Total { get; private set; }
        public ItensPedido ItensPedido { get; private set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }

        public PedidoIniciadoEvent(Guid clienteId, Guid pedidoId, decimal total, ItensPedido itensPedido,
            string nomeCartao, string numeroCartao, string expiracaoCartao, string cvvCartao)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            Total = total;
            ItensPedido = itensPedido;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
        }
    }
}