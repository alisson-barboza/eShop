using eShop.Core.DomainObjects;
using System;

namespace eShop.Vendas.Domain
{
    public class Item : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public Pedido Pedido { get; private set; }

        public Item(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        protected Item()
        {
        }

        internal void AssociarPedido(Guid pedidoId)
        {
            PedidoId = pedidoId;
        }

        public decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }

        internal void AdicionarUnidades(int unidades)
        {
            if (unidades < 1) return;
            Quantidade += unidades;
        }

        internal void AtualizarUnidades(int unidades)
        {
            if (unidades < 1) return;
            Quantidade = unidades;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}