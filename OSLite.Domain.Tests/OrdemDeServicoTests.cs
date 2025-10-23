using Xunit;
using OSLite.Domain;
using System;

public class OrdemDeServicoTests
{
    // Helpers
    private Cliente GetClienteValido(int id = 1) => new Cliente(id, "Isis Yasmim", "isis@teste.com");
    private ItemDeServico GetItem(decimal preco, int qtd) => new ItemDeServico("Item Teste", qtd, new Money(preco));

    [Fact]
    public void OS_total_soma_subtotais_itens() // Teste 3: OS_total_soma_subtotais_itens
    {
        // Arrange
        var cliente = GetClienteValido();
        var os = OrdemDeServico.AbrirOS(1, cliente);
        os.AdicionarItem(GetItem(100.00m, 1)); // Subtotal 100
        os.AdicionarItem(GetItem(25.00m, 4));  // Subtotal 100
        // Total é derivado
        // Act / Assert
        Assert.Equal(200.00m, os.Total.Value);
    }
    
    [Fact]
    public void OS_aberta_inicia_execucao_quando_tem_itens() // Teste 4 (Feliz): OS_aberta_inicia_execucao_quando_tem_itens
    {
        // Arrange
        var cliente = GetClienteValido();
        var os = OrdemDeServico.AbrirOS(2, cliente);
        os.AdicionarItem(GetItem(10m, 1)); // Cumpre a exigência de >= 1 item

        // Act
        os.IniciarExecucao();

        // Assert
        Assert.Equal(StatusOS.EmExecucao, os.Status);
    }

    [Fact]
    public void OS_aberta_nao_inicia_sem_itens() // Teste 4 (Falha): OS_aberta_nao_inicia_sem_itens
    {
        // Arrange
        var cliente = GetClienteValido();
        var os = OrdemDeServico.AbrirOS(3, cliente);
        // OS está Aberta e SEM itens

        // Act / Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            os.IniciarExecucao(); // Falha: exige >= 1 item [cite: 439]
        });
        Assert.Equal(StatusOS.Aberta, os.Status); 
    }
    
    [Fact]
    public void OS_nao_adiciona_itens_em_concluida_ou_cancelada() // Teste 5: OS_nao_adiciona_itens_em_concluida_ou_cancelada
    {
        // Arrange
        var cliente = GetClienteValido();
        var item = GetItem(10m, 1);
        
        // 1. Concluida
        var osConcluida = OrdemDeServico.AbrirOS(4, cliente);
        osConcluida.AdicionarItem(item); 
        osConcluida.IniciarExecucao();
        osConcluida.Concluir(); // Status = Concluida [cite: 406]
        
        // 2. Cancelada
        var osCancelada = OrdemDeServico.AbrirOS(5, cliente);
        osCancelada.Cancelar(); // Status = Cancelada [cite: 406]

        // Act / Assert
        // Proibição: não adicionar/remover itens em Concluida ou Cancelada
        Assert.Throws<InvalidOperationException>(() => osConcluida.AdicionarItem(item)); 
        Assert.Throws<InvalidOperationException>(() => osCancelada.AdicionarItem(item));
    }

    [Fact]
    public void OS_fluxo_aberta_para_execucao_para_concluida() // Teste 6: OS_fluxo_aberta_para_execucao_para_concluida
    {
        // Arrange
        var cliente = GetClienteValido();
        var os = OrdemDeServico.AbrirOS(6, cliente);
        os.AdicionarItem(GetItem(10, 1));

        // 1. Aberta -> EmExecucao [cite: 438]
        os.IniciarExecucao();
        Assert.Equal(StatusOS.EmExecucao, os.Status);

        // 2. EmExecucao -> Concluida [cite: 441]
        os.Concluir();
        Assert.Equal(StatusOS.Concluida, os.Status);
    }
    
    [Fact]
    public void OS_permite_cancelar_de_aberta_e_execucao() // Teste Adicional
    {
        // Cancelar de Aberta [cite: 443]
        var c1 = GetClienteValido(10);
        var os1 = OrdemDeServico.AbrirOS(10, c1);
        os1.Cancelar();
        Assert.Equal(StatusOS.Cancelada, os1.Status);

        // Cancelar de EmExecucao [cite: 443]
        var c2 = GetClienteValido(11);
        var os2 = OrdemDeServico.AbrirOS(11, c2);
        os2.AdicionarItem(GetItem(1, 1));
        os2.IniciarExecucao();
        os2.Cancelar();
        Assert.Equal(StatusOS.Cancelada, os2.Status);
    }
}