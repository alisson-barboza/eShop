using eShop.Core.Messages;
using FluentValidation;
using System;

namespace eShop.Vendas.Application.Commands
{
    public class RemoveItemCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }

        public RemoveItemCommand(Guid clienteId, Guid pedidoId, Guid produtoId)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RemoveItemValidation : AbstractValidator<RemoveItemCommand>
    {
        public RemoveItemValidation()
        {
            // Rules
        }
    }
}