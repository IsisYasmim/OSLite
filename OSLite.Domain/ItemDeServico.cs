// Value Object/Parte da Composição
namespace OSLite.Domain
{
    public record ItemDeServico
    {
        public string Descricao { get; init; }
        public int Quantidade { get; init; }
        public Money PrecoUnitario { get; init; }

        // Propriedade Derivada
        public Money SubTotal => PrecoUnitario * Quantidade;

        public ItemDeServico(string descricao, int quantidade, Money precoUnitario)
        {
            if (string.IsNullOrWhiteSpace(descricao))
            {
                throw new ArgumentException("A descrição do item de serviço não pode ser vazia.");
            }
            if (quantidade <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser maior que zero.");
            }

            Descricao = descricao;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }
    }
}