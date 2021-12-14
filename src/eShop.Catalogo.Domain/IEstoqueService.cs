using eShop.Core.Messages.Dtos;
using System;
using System.Threading.Tasks;

namespace eShop.Catalogo.Domain
{
    public interface IEstoqueService : IDisposable
    {
        Task<bool> DebitarEstoque(ItensPedido itensPedido);

        Task<bool> DebitarEstoque(Guid produtoId, int quantidade);

        Task<bool> ReporEstoque(ItensPedido itensPedido);

        Task<bool> ReporEstoque(Guid produtoId, int quantidade);
    }
}