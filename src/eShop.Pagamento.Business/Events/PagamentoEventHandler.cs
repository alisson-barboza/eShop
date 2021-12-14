using eShop.Core.Messages.CommomMessages.IntegrationEvents;
using eShop.Core.Messages.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Pagamentos.Business.Events
{
    public class PagamentoEventHandler : INotificationHandler<PedidoEstoqueConfirmadoEvent>
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoEventHandler(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        public async Task Handle(PedidoEstoqueConfirmadoEvent notification, CancellationToken cancellationToken)
        {
            var pagamentoPedido = new PagamentoPedido
            {
                PedidoId = notification.PedidoId,
                ClienteId = notification.ClienteId,
                Total = notification.Total,
                NomeCartao = notification.NomeCartao,
                NumeroCartao = notification.NumeroCartao,
                ExpiracaoCartao = notification.ExpiracaoCartao,
                CvvCartao = notification.CvvCartao
            };

            await _pagamentoService.RealizarPagamento(pagamentoPedido);
        }
    }
}