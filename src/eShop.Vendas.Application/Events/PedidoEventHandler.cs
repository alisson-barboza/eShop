using eShop.Core.Communication.Mediator;
using eShop.Core.Messages.CommomMessages.IntegrationEvents;
using eShop.Vendas.Application.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace eShop.Vendas.Application.Events
{
    public class PedidoEventHandler :
        INotificationHandler<PedidoAtualizadoEvent>,
        INotificationHandler<PedidoRascunhoGeradoEvent>,
        INotificationHandler<PedidoEstoqueRejeitadoEvent>,
        INotificationHandler<PagamentoRealizadoEvent>,
        INotificationHandler<PagamentoRecusadoEvent>
    {
        private readonly IMediatrHandler _mediatrHandler;

        public PedidoEventHandler(IMediatrHandler mediatrHandler)
        {
            _mediatrHandler = mediatrHandler;
        }

        public async Task Handle(PedidoEstoqueRejeitadoEvent notification, CancellationToken cancellationToken)
        {
            await _mediatrHandler.SendCommand(new CancelarProcessamentoPedidoCommand(notification.PedidoId, notification.ClienteId));
        }

        public Task Handle(PedidoRascunhoGeradoEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Handle(PedidoAtualizadoEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task Handle(PagamentoRecusadoEvent notification, CancellationToken cancellationToken)
        {
            await _mediatrHandler.SendCommand(new FinalizarPedidoCommand(notification.PedidoId, notification.ClienteId));
        }

        public async Task Handle(PagamentoRealizadoEvent notification, CancellationToken cancellationToken)
        {
            await _mediatrHandler.SendCommand(new CancelarProcessamentoPedidoEstormarEstoqueCommand(notification.PedidoId, notification.ClienteId));
        }
    }
}