using Microsoft.EntityFrameworkCore;

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

        // Construtor para inicializar as propriedades
        public CadastroUsuario(string nome, string cpf, string email, string senha, string tipoConta)
        {
            Nome = nome;
            CPF = cpf;
            Email = email;
            Senha = senha;
            TipoConta = tipoConta;
        }
    }
}
