using eShop.Core.Communication.Mediator;
using eShop.Core.Extensions;
using eShop.Core.Messages;
using eShop.Core.Messages.CommomMessages.IntegrationEvents;
using eShop.Core.Messages.CommomMessages.Notifications;
using eShop.Core.Messages.Dtos;
using eShop.Vendas.Application.Events;
using eShop.Vendas.Domain;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static eShop.Vendas.Domain.Pedido;

namespace eShop.Vendas.Application.Commands
{
    public class PedidoCommandHandler :
        IRequestHandler<AddItemCommand, bool>,
        IRequestHandler<UpdateItemCommand, bool>,
        IRequestHandler<RemoveItemCommand, bool>,
        IRequestHandler<SetVoucherCommand, bool>,
        IRequestHandler<IniciarPedidoCommand, bool>,
        IRequestHandler<FinalizarPedidoCommand, bool>,
        IRequestHandler<CancelarProcessamentoPedidoEstormarEstoqueCommand, bool>,
        IRequestHandler<CancelarProcessamentoPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatrHandler _mediatorHandler;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediatrHandler mediatorHandler)
        {
            _pedidoRepository = pedidoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AddItemCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) return false;
            var pedido = await _pedidoRepository.GetPedidoRascunhoBy(command.ClienteId);
            var pedidoItem = new Domain.Item(command.ProdutoId, command.ProdutoNome, command.Quantidade, command.ValorUnitario);
            if (pedido is null)
            {
                pedido = PedidoFactory.GerarPedidoRascunho(command.ClienteId);
                pedido.AdicionarItem(pedidoItem);
                _pedidoRepository.Add(pedido);
                pedido.AddEvent(new PedidoRascunhoGeradoEvent(pedido.ClienteId, pedido.Id));
            }
            else
            {
                pedido.AdicionarItem(pedidoItem);
                pedido.AddEvent(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            }

            pedido.AddEvent(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, command.ProdutoId,
                                                            command.ProdutoNome, command.Quantidade, command.ValorUnitario));

            await _pedidoRepository.UnitOfWork.Commit();
            return true;
        }

        public async Task<bool> Handle(SetVoucherCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) return false;

            var pedido = await _pedidoRepository.GetBy(command.PedidoId);
            if (pedido is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            var voucher = await _pedidoRepository.GetVoucherBy(command.CodigoVoucher);
            if (voucher is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("voucher", "Voucher não encontrado"));
                return false;
            }

            var voucherValidation = pedido.AplicarVoucher(voucher);
            if (!voucherValidation.IsValid)
            {
                foreach (var error in voucherValidation.Errors)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(error.ErrorCode, error.ErrorMessage));
                }
                return false;
            }

            pedido.AddEvent(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AddEvent(new VoucherAplicadoEvent(command.ClienteId, pedido.Id, voucher.Id));

            _pedidoRepository.Update(pedido);
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(RemoveItemCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) return false;

            var pedido = await _pedidoRepository.GetBy(command.PedidoId);
            if (pedido is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }
            var pedidoItem = await _pedidoRepository.GetItemByPedido(pedido.Id, command.ProdutoId);
            if (pedidoItem is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("item", "Item não encontrado"));
                return false;
            }

            pedido.RemoverItem(pedidoItem);
            pedido.AddEvent(new ItemRemovidoEvent(command.ClienteId, pedido.Id, command.ProdutoId));
            pedido.AddEvent(new PedidoAtualizadoEvent(command.ClienteId, pedido.Id, pedido.ValorTotal));

            _pedidoRepository.Update(pedido);
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) return false;

            var pedido = await _pedidoRepository.GetBy(command.PedidoId);
            if (pedido is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }
            var pedidoItem = await _pedidoRepository.GetItemByPedido(pedido.Id, command.ProdutoId);
            if (pedidoItem is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("item", "Item não encontrado"));
                return false;
            }

            pedido.AtualizarUnidades(pedidoItem, command.Quantidade);

            pedido.AddEvent(new PedidoAtualizadoEvent(command.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AddEvent(new ItemAtualizadoEvent(command.ClienteId, pedido.Id, command.ProdutoId, command.Quantidade));

            _pedidoRepository.Update(pedido);
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(IniciarPedidoCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) return false;

            var pedido = await _pedidoRepository.GetPedidoRascunhoBy(command.ClienteId);
            pedido.IniciarPedido();

            var itens = new List<Core.Messages.Dtos.Item>();
            pedido.PedidoItens.ForEach(i => itens.Add(new Core.Messages.Dtos.Item { Id = i.Id, Quantidade = i.Quantidade }));
            var itensPedido = new ItensPedido { PedidoId = pedido.Id, Itens = itens };

            pedido.AddEvent(new PedidoIniciadoEvent(command.ClienteId, pedido.Id, pedido.ValorTotal, itensPedido, command.NomeCartao, command.NumeroCartao, command.ExpiracaoCartao, command.CvvCartao));

            _pedidoRepository.Update(pedido);
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(CancelarProcessamentoPedidoEstormarEstoqueCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) return false;

            var pedido = await _pedidoRepository.GetBy(command.PedidoId);
            if (pedido is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            var itens = new List<Core.Messages.Dtos.Item>();
            pedido.PedidoItens.ForEach(i => itens.Add(new Core.Messages.Dtos.Item { Id = i.Id, Quantidade = i.Quantidade }));
            var itensPedido = new ItensPedido { PedidoId = pedido.Id, Itens = itens };

            pedido.AddEvent(new PedidoProcessamentoCanceladoEvent(pedido.Id, pedido.ClienteId, itensPedido));
            pedido.TornarRascunho();

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(FinalizarPedidoCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) return false;

            var pedido = await _pedidoRepository.GetBy(command.PedidoId);
            if (pedido is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            pedido.FinalizarPedido();
            // Evento poderia ser de integração caso quisesse entrar no contexto fiscal
            pedido.AddEvent(new PedidoFinalizadoEvent(command.PedidoId));
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(CancelarProcessamentoPedidoCommand command, CancellationToken cancellationToken)
        {
            if (!ValidarComando(command)) return false;

            var pedido = await _pedidoRepository.GetBy(command.PedidoId);
            if (pedido is null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado"));
                return false;
            }

            pedido.TornarRascunho();

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        private bool ValidarComando(Command command)
        {
            if (command.IsValid()) return true;

            foreach (var erro in command.ValidationResult.Errors)
            {
                _mediatorHandler.PublishNotification(new DomainNotification(command.MessageType, erro.ErrorMessage));
            }

            return false;
        }
    }
}