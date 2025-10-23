// Entidade (Agregado)
namespace OSLite.Domain
{
    public class Cliente
    {
        private readonly List<OrdemDeServico> _ordens = [];

        public int Id { get; init; }
        public string Nome { get; init; }
        public string Email { get; init; }
        public IReadOnlyCollection<OrdemDeServico> Ordens => _ordens.AsReadOnly();

        public Cliente(int id, string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome do cliente não pode ser vazio.");
            }
            // Validações adicionais para email podem ser feitas aqui ou em um VO Email.

            Id = id;
            Nome = nome;
            Email = email;
        }

        public void AdicionarOrdem(OrdemDeServico ordem)
        {
            if (_ordens.Contains(ordem)) return;

            // 1. Sincroniza o Cliente na Ordem
            ordem.DefinirCliente(this);

            // 2. Adiciona a Ordem na coleção do Cliente
            _ordens.Add(ordem);
        }

        public void RemoverOrdem(OrdemDeServico ordem)
        {
            if (ordem == null) throw new ArgumentNullException(nameof(ordem));
            _ordens.Remove(ordem);
        }

        // Lógica de troca de cliente (Bidirecional: remover do antigo, adicionar no novo)
        public static void TrocarCliente(OrdemDeServico os, Cliente novoCliente, Cliente clienteAntigo)
        {
            if (clienteAntigo != null)
            {
                clienteAntigo.RemoverOrdem(os);
            }
            novoCliente.AdicionarOrdem(os); // Já faz a sincronização bidirecional
        }
    }
}