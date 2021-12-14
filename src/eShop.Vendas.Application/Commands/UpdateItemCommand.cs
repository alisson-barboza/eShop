using eShop.Core.Messages;
using FluentValidation;
using System;

namespace eShop.Vendas.Application.Commands
{
    public class UpdateItemCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }

        public UpdateItemCommand(Guid clienteId, Guid pedidoId, Guid produtoId, int quantidade)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateItemValidation : AbstractValidator<UpdateItemCommand>
    {
        public UpdateItemValidation()
        {
            // Rules
        }
    }
}