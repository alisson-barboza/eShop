using eShop.Vendas.Application.Queries.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Vendas.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<CarrinhoDto> GetCarrinho(Guid clienteId);

        Task<IEnumerable<PedidoDto>> GetPedidos(Guid clienteId);
    }
}