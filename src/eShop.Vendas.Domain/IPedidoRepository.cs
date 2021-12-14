using eShop.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Vendas.Domain
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> GetBy(Guid id);

        Task<IEnumerable<Pedido>> GetListBy(Guid clienteId);

        Task<Pedido> GetPedidoRascunhoBy(Guid clienteId);

        void Add(Pedido pedido);

        void Update(Pedido pedido);

        Task<Item> GetItemBy(Guid id);

        Task<Item> GetItemByPedido(Guid pedidoId, Guid produtoId);

        void Add(Item pedidoItem);

        void Update(Item pedidoItem);

        void Remove(Item pedidoItem);

        Task<Voucher> GetVoucherBy(string codigo);
    }
}