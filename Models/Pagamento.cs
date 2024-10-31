namespace QBankApi.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public string ContaOrigem { get; set; }
        public string Destinatario { get; set; }
        public DateTime DataAgendamento { get; set; }
        public bool IsPagamentoAgendado { get; set; }
        public string Comprovante { get; set; }

        public void EfetuarPagamento(decimal valor, string contaOrigem, string destinatario)
        {
            // codigo
        }

        public void AgendarPagamento(DateTime data)
        {
            // codigo
        }

        public void DebitoAutomatico(string contaOrigem, decimal valor)
        {
            // codigo
        }

        public string ConsultarComprovante(int idPagamento)
        {
            // codigo
            return Comprovante; // Retornar o comprovante
        }

        public void EditarPagamentoAgendado(int idPagamento, DateTime novaData)
        {
            // codigo
        }

        public void CancelarPagamentoAgendado(int idPagamento)
        {
           // codigo
        }

        public void HistoricoDePagamentos()
        {
            // codigo
        }

        public void RelatoriosDeDespesas()
        {
            // codigo
        }
    }
}