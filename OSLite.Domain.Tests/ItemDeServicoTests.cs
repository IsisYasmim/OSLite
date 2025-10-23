using Xunit;
using System;
using OSLite.Domain;

namespace OSLite.Domain.Tests
{
    public class ItemDeServicoTests
    {
        private ItemDeServico GetItem(decimal preco, int qtd, string desc)
            => new ItemDeServico(desc, qtd, new Money(preco));

        // --- Happy Path (Testes de Sucesso) ---

        [Fact]
        public void ItemDeServico_cria_valido_e_calcula_subtotal()
        {
            // Arrange
            decimal preco = 120.00m;
            int qtd = 2;
            string desc = "Troca de Tela";
            decimal expectedSubtotal = 240.00m; // 120.00 * 2

            // Act
            var item = GetItem(preco, qtd, desc);

            // Assert
            Assert.Equal(expectedSubtotal, item.SubTotal.Value);
        }

        // --- Failure Path (Testes de Invariantes) ---

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ItemDeServico_lanca_excecao_quando_quantidade_e_invalida(int qtdInvalida)
        {
            // Arrange
            decimal preco = 10.00m;
            string desc = "Teste";

            // Act & Assert
            // Invariante: Quantidade > 0 deve falhar com ArgumentOutOfRangeException
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                GetItem(preco, qtdInvalida, desc);
            });
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ItemDeServico_lanca_excecao_quando_descricao_e_vazia(string descInvalida)
        {
            // Arrange
            decimal preco = 10.00m;
            int qtd = 1;

            // Act & Assert
            // Invariante: Descricao não vazia deve falhar com ArgumentException
            Assert.Throws<ArgumentException>(() =>
            {
                GetItem(preco, qtd, descInvalida);
            });
        }
    }
}