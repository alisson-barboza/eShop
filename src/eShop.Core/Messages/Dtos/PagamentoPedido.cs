using System;

namespace eShop.Core.Messages.Dtos
{
    public class PagamentoPedido
    {
        public Guid ClienteId { get; set; }
        public Guid PedidoId { get; set; }
        public decimal Total { get; set; }
        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }
    }
}