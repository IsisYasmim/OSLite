using Xunit;
using System.Linq;
using OSLite.Domain;

public class BidirecionalTests
{
    private Cliente GetClienteValido(int id, string nome) => new Cliente(id, nome, $"{nome.ToLower()}@exemplo.com");
    
    [Fact]
    public void Cliente_adiciona_ordem_sincroniza_cliente_na_ordem() // Teste 7: Cliente_adiciona_ordem_sincroniza_cliente_na_ordem 
    {
        // Arrange
        var cliente = GetClienteValido(1, "Isis");
        
        // Act: A criação via Factory Method deve garantir o vínculo bidirecional coeso 
        var os = OrdemDeServico.AbrirOS(1, cliente);
        
        // Assert (Sincronização Bidirecional) 
        
        // 1. Lado Cliente (coleção)
        Assert.Contains(os, cliente.Ordens);
        Assert.Single(cliente.Ordens);
        
        // 2. Lado Ordem (referência de volta)
        Assert.Same(cliente, os.Cliente);
        Assert.Equal(cliente.Id, os.ClienteId);
    }

    [Fact]
    public void OS_trocar_de_cliente_atualiza_colecoes_dos_clientes() // Teste 8: OS_trocar_de_cliente_atualiza_colecoes_dos_clientes 
    {
        // Arrange
        var clienteAntigo = GetClienteValido(1, "Antigo");
        var novoCliente = GetClienteValido(2, "Novo");
        var os = OrdemDeServico.AbrirOS(10, clienteAntigo); // OS vinculada ao Antigo
        
        // Pré-verificação
        Assert.Single(clienteAntigo.Ordens);
        Assert.Empty(novoCliente.Ordens);

        // Act: Trocar o Cliente 
        Cliente.TrocarCliente(os, novoCliente, clienteAntigo);
        
        // Assert (Invariantes Bidirecionais após Troca) 
        
        // 1. O cliente antigo não deve ter a OS na sua coleção
        Assert.DoesNotContain(os, clienteAntigo.Ordens);
        Assert.Empty(clienteAntigo.Ordens);
        
        // 2. O novo cliente deve ter a OS na sua coleção
        Assert.Contains(os, novoCliente.Ordens);
        Assert.Single(novoCliente.Ordens);
        
        // 3. A OS deve apontar para o novo cliente (referência e Id)
        Assert.Same(novoCliente, os.Cliente);
        Assert.Equal(novoCliente.Id, os.ClienteId);
    }
}