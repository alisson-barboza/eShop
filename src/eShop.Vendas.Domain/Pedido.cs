using eShop.Core.DomainObjects;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eShop.Vendas.Domain
{
    public class Pedido : Entity, IAggregateRoot
    {
        public int Codigo { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid? VoucherId { get; private set; }
        public bool VoucherUtilizado { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }

        private readonly List<Item> _pedidoItens;
        public IReadOnlyCollection<Item> PedidoItens => _pedidoItens;

        // EF Relation
        public Voucher Voucher { get; private set; }

        public Pedido(Guid clienteId, bool voucherUtilizado, decimal desconto, decimal valorTotal)
        {
            ClienteId = clienteId;
            VoucherUtilizado = voucherUtilizado;
            Desconto = desconto;
            ValorTotal = valorTotal;

            _pedidoItens = new List<Item>();
        }

        protected Pedido()
        {
            _pedidoItens = new List<Item>();
        }

        public void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;
            decimal desconto = 0;
            var valor = ValorTotal;
            switch (Voucher.TipoDescontoVoucher)
            {
                case TipoDescontoVoucher.Porcentagem:
                    if (Voucher.Percentual.HasValue)
                    {
                        desconto = (valor * Voucher.Percentual.Value) / 100;
                        valor -= desconto;
                    }
                    break;

                case TipoDescontoVoucher.Valor:
                    if (Voucher.ValorDesconto.HasValue)
                    {
                        desconto = Voucher.ValorDesconto.Value;
                        valor -= desconto;
                    }
                    break;

                default:
                    return;
            }
            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }

        public void CalcularValorPedido()
        {
            ValorTotal = PedidoItens.Sum(p => p.CalcularValor());
            CalcularValorTotalDesconto();
        }

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var validationResult = voucher.CanUseVoucher();
            if (!validationResult.IsValid) return validationResult;

            Voucher = voucher;
            VoucherUtilizado = true;
            CalcularValorPedido();

            return validationResult;
        }

        public bool PedidoItemExistente(Item item)
        {
            return _pedidoItens.Any(p => p.ProdutoId == item.ProdutoId);
        }

        public void AdicionarItem(Item item)
        {
            if (!item.IsValid()) return;

            item.AssociarPedido(Id);

            if (PedidoItemExistente(item))
            {
                var itemExistente = _pedidoItens.SingleOrDefault(p => p.ProdutoId == item.ProdutoId);
                itemExistente.AdicionarUnidades(item.Quantidade);
                item = itemExistente;

                _pedidoItens.Remove(itemExistente);
            }

            item.CalcularValor();
            _pedidoItens.Add(item);
            CalcularValorPedido();
        }

        public void RemoverItem(Item item)
        {
            if (!item.IsValid()) return;

            var itemExistente = PedidoItens.SingleOrDefault(p => p.ProdutoId == item.ProdutoId);

            if (itemExistente is null) throw new DomainException("O item não pertence ao pedido");
            _pedidoItens.Remove(itemExistente);

            CalcularValorPedido();
        }

        public void AtualizarItem(Item item)
        {
            RemoverItem(item);
            AdicionarItem(item);

            CalcularValorPedido();
        }

        public void AtualizarUnidades(Item item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public void IniciarPedido()
        {
            PedidoStatus = PedidoStatus.Iniciado;
        }

        public void FinalizarPedido()
        {
            PedidoStatus = PedidoStatus.Pago;
        }

        public void CancelarPedido()
        {
            PedidoStatus = PedidoStatus.Cancelado;
        }

        public static class PedidoFactory
        {
            public static Pedido GerarPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido { ClienteId = clienteId };
                pedido.TornarRascunho();
                return pedido;
            }
        }
    }
}