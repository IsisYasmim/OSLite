// Entidade (Aggregate Root)
namespace OSLite.Domain
{
    public class OrdemDeServico
    {
        private readonly List<ItemDeServico> _itens = [];

        public int Id { get; init; } // Id (para persistência)
        public int ClienteId { get; private set; }
        public Cliente Cliente { get; private set; } // Navegabilidade de volta
        public DateOnly DataAbertura { get; init; }
        public StatusOS Status { get; private set; }

        // Total derivado da coleção de itens (invariante)
        public Money Total => new(_itens.Sum(i => i.SubTotal.Value));

        // Coleção encapsulada
        public IReadOnlyCollection<ItemDeServico> Itens => _itens.AsReadOnly();

        // Construtor privado para forçar criação coesa via método de fábrica (para TDD)
        private OrdemDeServico(int id, Cliente cliente)
        {
            Id = id;
            DataAbertura = DateOnly.FromDateTime(DateTime.Now);
            Status = StatusOS.Aberta;
            // O vínculo bidirecional será estabelecido no método de fábrica
        }

        // Método de Fábrica (simula a criação coesa de uma OS já ligada ao Cliente)
        public static OrdemDeServico AbrirOS(int id, Cliente cliente)
        {
            var os = new OrdemDeServico(id, cliente);
            cliente.AdicionarOrdem(os); // Sincronização bidirecional
            return os;
        }

        public void AdicionarItem(ItemDeServico item)
        {
            if (Status is StatusOS.Concluida or StatusOS.Cancelada)
            {
                throw new InvalidOperationException($"Não é possível adicionar itens a uma OS com status '{Status}'.");
            }
            _itens.Add(item);
        }

        public void IniciarExecucao()
        {
            if (Status != StatusOS.Aberta)
            {
                throw new InvalidOperationException($"A execução só pode ser iniciada a partir do status '{StatusOS.Aberta}'.");
            }
            if (_itens.Count == 0)
            {
                throw new InvalidOperationException("Não é possível iniciar a execução de uma OS sem itens de serviço.");
            }
            Status = StatusOS.EmExecucao;
        }

        public void Concluir()
        {
            if (Status != StatusOS.EmExecucao)
            {
                throw new InvalidOperationException($"A OS só pode ser concluída a partir do status '{StatusOS.EmExecucao}'.");
            }
            Status = StatusOS.Concluida;
        }

        public void Cancelar()
        {
            if (Status is StatusOS.Concluida or StatusOS.Cancelada)
            {
                throw new InvalidOperationException($"Não é possível cancelar uma OS com status '{Status}'.");
            }
            Status = StatusOS.Cancelada;
        }

        // Método de sincronização do vínculo bidirecional (para uso interno por Cliente)
        internal void DefinirCliente(Cliente cliente)
        {
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
            ClienteId = cliente.Id;
        }
    }
}