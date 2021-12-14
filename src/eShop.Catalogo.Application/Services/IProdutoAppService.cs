using eShop.Catalogo.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Catalogo.Application.Services
{
    public interface IProdutoAppService : IDisposable
    {
        Task<IEnumerable<ProdutoDto>> ObterPorCategoria(int codigo);

        Task<ProdutoDto> ObterPorId(Guid id);

        Task<IEnumerable<ProdutoDto>> ObterTodos();

        Task<IEnumerable<CategoriaDto>> ObterCategorias();

        Task AdicionarProduto(ProdutoDto ProdutoDto);

        Task AtualizarProduto(ProdutoDto ProdutoDto);

        Task<ProdutoDto> DebitarEstoque(Guid id, int quantidade);

        Task<ProdutoDto> ReporEstoque(Guid id, int quantidade);
    }
}