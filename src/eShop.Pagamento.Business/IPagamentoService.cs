using eShop.Core.Messages.Dtos;
using System.Threading.Tasks;

namespace eShop.Pagamentos.Business
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamento(PagamentoPedido pagamentoPedido);
    }
}