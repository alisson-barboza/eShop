using eShop.Vendas.Application.Queries.Dtos;
using eShop.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Vendas.Application.Queries
{
    public class PedidoQueries : IPedidoQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<CarrinhoDto> GetCarrinho(Guid clienteId)
        {
            var pedido = await _pedidoRepository.GetPedidoRascunhoBy(clienteId);
            if (pedido is null) return null;

            var carrinho = new CarrinhoDto
            {
                ClienteId = pedido.ClienteId,
                ValorTotal = pedido.ValorTotal,
                PedidoId = pedido.Id,
                ValorDesconto = pedido.Desconto,
                SubTotal = pedido.Desconto + pedido.ValorTotal
            };

            if (pedido.VoucherId is not null)
                carrinho.VoucherCodigo = pedido.Voucher.Codigo;

            foreach (var item in pedido.PedidoItens)
            {
                carrinho.Itens.Add(new CarrinhoItemDto
                {
                    ProdutoId = item.ProdutoId,
                    ProdutoNome = item.ProdutoNome,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    ValorTotal = item.CalcularValor()
                });
            }

            return carrinho;
        }

        public async Task<IEnumerable<PedidoDto>> GetPedidos(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.GetListBy(clienteId);

            pedidos = pedidos.Where(p => p.PedidoStatus == PedidoStatus.Pago || p.PedidoStatus == PedidoStatus.Cancelado)
                .OrderBy(p => p.DataCadastro);

            var pedidosDto = new List<PedidoDto>();

            if (!pedidos.Any()) return pedidosDto;

            foreach (var pedido in pedidos)
            {
                pedidosDto.Add(new PedidoDto
                {
                    ValorTotal = pedido.ValorTotal,
                    PedidoStatus = pedido.PedidoStatus,
                    Codigo = pedido.Codigo,
                    DataCadastro = pedido.DataCadastro
                });
            }

            return pedidosDto;
        }
    }
}