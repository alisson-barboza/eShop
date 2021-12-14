using eShop.Vendas.Domain;
using System;

namespace eShop.Vendas.Application.Queries.Dtos
{
    public class PedidoDto
    {
        public int Codigo { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataCadastro { get; set; }
        public PedidoStatus PedidoStatus { get; set; }
    }
}