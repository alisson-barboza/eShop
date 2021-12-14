using eShop.Core.Messages;
using FluentValidation;
using System;

namespace eShop.Vendas.Application.Commands
{
    public class SetVoucherCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string CodigoVoucher { get; private set; }

        public SetVoucherCommand(Guid clienteId, Guid pedidoId, Guid produtoId, string codigoVoucher)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            CodigoVoucher = codigoVoucher;
        }

        public override bool IsValid()
        {
            ValidationResult = new SetVoucherValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class SetVoucherValidation : AbstractValidator<SetVoucherCommand>
    {
        public SetVoucherValidation()
        {
            // Rules
        }
    }
}