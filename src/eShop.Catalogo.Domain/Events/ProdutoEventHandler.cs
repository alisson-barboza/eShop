using eShop.Core.Communication.Mediator;
using eShop.Core.Messages.CommomMessages.IntegrationEvents;
using eShop.Vendas.Application.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Catalogo.Domain.Events
{
    public class ProdutoEventHandler :
        INotificationHandler<ProdutoAbaixoEstoqueEvent>,
        INotificationHandler<PedidoIniciadoEvent>,
        INotificationHandler<PedidoProcessamentoCanceladoEvent>

    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMediatrHandler _mediatrHandler;

        public ProdutoEventHandler(IProdutoRepository produtoRepository,
                                    IEstoqueService estoqueService,
                                    IMediatrHandler mediatrHandler)
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mediatrHandler = mediatrHandler;
        }

        public async Task Handle(ProdutoAbaixoEstoqueEvent mensagem, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterPorId(mensagem.AggregateId);

            // Enviar um email para aquisicao de mais produtos.
        }

        public async Task Handle(PedidoIniciadoEvent notification, CancellationToken cancellationToken)
        {
            var result = await _estoqueService.DebitarEstoque(notification.ItensPedido);
            if (result)
            {
                await _mediatrHandler.PublishEvent(new PedidoEstoqueConfirmadoEvent(notification.PedidoId, notification.ClienteId, notification.Total, notification.ItensPedido, notification.NomeCartao, notification.NumeroCartao, notification.ExpiracaoCartao, notification.CvvCartao));
                return;
            }
            await _mediatrHandler.PublishEvent(new PedidoEstoqueRejeitadoEvent(notification.PedidoId, notification.ClienteId));
        }

        public async Task Handle(PedidoProcessamentoCanceladoEvent notification, CancellationToken cancellationToken)
        {
            await _estoqueService.ReporEstoque(notification.ItensPedido);
        }
    }
}