using banco_vb.Data;
using banco_vb.Models;
using banco_vb.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace banco_vb.Tests;

public class ContaServiceTests
{
    private BancoDbContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<BancoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new BancoDbContext(options);
    }

    [Fact]
    public void Sacar_ComSaldoSuficiente_DeveAtualizarSaldoEGravarTransacao()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();
        var conta = new Conta { Id = 1, Titular = "Vitor", Saldo = 500 };
        context.Contas.Add(conta);
        context.SaveChanges();

        var service = new ContaService(context);

        // Act
        var resultado = service.Sacar(1, 200);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(300, resultado.Saldo);
        Assert.Single(context.Transacoes);
    }

    [Fact]
    public void Sacar_ComSaldoInsuficiente_DeveLancarExcecao()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();
        var conta = new Conta { Id = 1, Titular = "Vitor", Saldo = 100 };
        context.Contas.Add(conta);
        context.SaveChanges();

        var service = new ContaService(context);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.Sacar(1, 200));
    }

    [Fact]
    public void Transferir_ParaMesmaConta_DeveLancarExcecao()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();
        var conta = new Conta { Id = 1, Titular = "Vitor", Saldo = 500 };
        context.Contas.Add(conta);
        context.SaveChanges();

        var service = new ContaService(context);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => service.Transferir(1, 1, 100));
    }

    [Fact]
    public void Transferir_ComDadosValidos_DeveAtualizarSaldosEGravarNomesNoExtrato()
    {
        // Arrange
        using var context = CriarContextoEmMemoria();
        var contaOrigem = new Conta { Id = 1, Titular = "Vitor Brenguere", Saldo = 1000 };
        var contaDestino = new Conta { Id = 2, Titular = "Maria Silva", Saldo = 200 };
        context.Contas.AddRange(contaOrigem, contaDestino);
        context.SaveChanges();

        var service = new ContaService(context);

        // Act
        var sucesso = service.Transferir(1, 2, 300);

        // Assert
        Assert.True(sucesso);
        Assert.Equal(700, context.Contas.Find(1)!.Saldo);
        Assert.Equal(500, context.Contas.Find(2)!.Saldo);

        // Valida se gravou as duas transações (uma para cada conta)
        Assert.Equal(2, context.Transacoes.Count());

        // Valida o nome do titular na transação de quem enviou
        var transacaoOrigem = context.Transacoes.FirstOrDefault(t => t.ContaId == 1);
        Assert.NotNull(transacaoOrigem);
        Assert.Equal("Maria Silva", transacaoOrigem.ContaRelacionadaTitular);
    }
}