namespace QBankApi.DTOs
{
    public class AccountDTO
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public decimal Balance { get; set; }
    
    // Inicializando variáveis 
        public AccountDTO(string accountNumber, string accountHolder)
        {
            AccountNumber = accountNumber;
            AccountHolder = accountHolder;
        }

    }
}