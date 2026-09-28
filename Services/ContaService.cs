using banco_vb.Models;

namespace banco_vb.Services;

public class ContaService
{
    private readonly List<Conta> _contas = new()
    {
        new Conta { Id = 1, Titular = "Vitor Brenguere", Saldo = 1500.50m },
        new Conta { Id = 2, Titular = "Neymar Jr", Saldo = 7200.00m },
        new Conta { Id = 3, Titular = "Allen Iverson", Saldo = 5450.75m }
    };

    public List<Conta> ObterTodas()
    {
        return _contas;
    }

    public Conta? ObterPorId(int id)
    {
        return _contas.FirstOrDefault(c => c.Id == id);
    }

    // Novo método: Cria uma nova conta e gera o ID automaticamente
    public Conta Adicionar(Conta novaConta)
    {
        // Pega o maior ID atual e soma 1 (ou começa em 1 se a lista estiver vazia)
        novaConta.Id = _contas.Any() ? _contas.Max(c => c.Id) + 1 : 1;
        _contas.Add(novaConta);
        return novaConta;
    }

    // Novo método: Remove uma conta pelo ID
    public bool Remover(int id)
    {
        var conta = ObterPorId(id);
        if (conta == null) return false;

        _contas.Remove(conta);
        return true;
    }
}