using Microsoft.EntityFrameworkCore;

namespace QBankApi.Models
{
    public class CadastroUsuario
    {
        // Propriedades da classe
        public int Id { get; set; } // Chave primária (opcional, pode ser gerada pelo EF)
        public string Nome { get; set; } = string.Empty; // Adicionando valor padrão
        public string CPF { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string TipoConta { get; set; } = string.Empty;

        // Construtor padrão (necessário para o Entity Framework Core)
        public CadastroUsuario() { }

        // Construtor para inicializar as propriedades
        public CadastroUsuario(string nome, string cpf, string email, string senha, string tipoConta)
        {
            Nome = nome;
            CPF = cpf;
            Email = email ?? throw new ArgumentNullException(nameof(email)); // Garantir que Email não seja nulo
            Senha = senha ?? throw new ArgumentNullException(nameof(senha)); // Garantir que Senha não seja nulo
            TipoConta = tipoConta;
        }
    }
}
