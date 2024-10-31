using
namespace QBankApi.Models
{
    public class CadastroUsuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string TipoConta { get; set; }
    }
}
