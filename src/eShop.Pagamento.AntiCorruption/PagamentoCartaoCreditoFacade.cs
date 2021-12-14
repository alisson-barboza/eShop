using eShop.Pagamentos.Business;

namespace eShop.Pagamentos.AntiCorruption
{
    public class PagamentoCartaoCreditoFacade : IPagamentoCartaoCreditoFacade
    {
        private readonly IPaypalGateway _paypalGateway;
        private readonly IConfigurationManager _configurationManager;

        public PagamentoCartaoCreditoFacade(IPaypalGateway paypalGateway, IConfigurationManager configurationManager)
        {
            _paypalGateway = paypalGateway;
            _configurationManager = configurationManager;
        }

        public Transacao RealizarPagamento(Pedido pedido, Business.Pagamento pagamento)
        {
            var apiKey = _configurationManager.GetValue("apiKey");
            var encriptionKey = _configurationManager.GetValue("encriptionKey");

            var serviceKey = _paypalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
            var cardHashKey = _paypalGateway.GetCardHashKey(serviceKey, pagamento.NumeroCartao);

            var pagamentoResult = _paypalGateway.CommitTransaction(cardHashKey, pedido.Id.ToString(), pagamento.Valor);

            // TODO: O gateway de pagamentos que deve retornar o objeto transacao
            var transacao = new Transacao
            {
                PedidoId = pedido.Id,
                Total = pedido.Valor,
                PagamentoId = pagamento.Id
            };

            if (pagamentoResult)
            {
                transacao.Status = StatusTransacao.Pago;
                return transacao;
            }
            transacao.Status = StatusTransacao.Recusado;
            return transacao;
        }
    }
}