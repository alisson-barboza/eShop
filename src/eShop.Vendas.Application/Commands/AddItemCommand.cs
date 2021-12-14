using eShop.Core.Messages;
using FluentValidation;
using System;

namespace eShop.Vendas.Application.Commands
{
    public class AddItemCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public AddItemCommand(Guid clienteId, Guid produtoId, string nome, int quantidade, decimal valorUnitario)
        {
            ClienteId = clienteId;
            ProdutoId = produtoId;
            ProdutoNome = nome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddItemValidation : AbstractValidator<AddItemCommand>
    {
        public AddItemValidation()
        {
            // Rules
        }
    }
}