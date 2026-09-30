using banco_vb.Data;
using banco_vb.DTOs;
using banco_vb.Models;
using Microsoft.EntityFrameworkCore;

namespace banco_vb.Services;

public class ContaService
{
    private readonly BancoDbContext _context;

    // Injeta o BancoDbContext através do construtor
    public ContaService(BancoDbContext context)
    {
        _context = context;
    }

    public List<Conta> ObterTodas()
    {
        return _context.Contas.ToList();
    }

    public Conta? ObterPorId(int id)
    {
        return _context.Contas.FirstOrDefault(c => c.Id == id);
    }

    public Conta Adicionar(ContaCriacaoDto dto)
    {
        var novaConta = new Conta
        {
            Titular = dto.Titular,
            Saldo = dto.Saldo
        };

        _context.Contas.Add(novaConta);
        _context.SaveChanges(); // Persiste a alteração no SQLite e gera o Id automaticamente

        return novaConta;
    }

    public bool Remover(int id)
    {
        var conta = ObterPorId(id);
        if (conta == null) return false;

        _context.Contas.Remove(conta);
        _context.SaveChanges(); // Persiste a remoção no SQLite

        return true;
    }   
    
    public Conta? Atualizar(int id, ContaCriacaoDto dto)
    {
    var contaExistente = ObterPorId(id);
    if (contaExistente == null) return null;

    contaExistente.Titular = dto.Titular;
    contaExistente.Saldo = dto.Saldo;

    _context.Contas.Update(contaExistente);
    _context.SaveChanges();

    return contaExistente;
    }

    public Conta? Depositar(int id, decimal valor)
{
    var conta = ObterPorId(id);
    if (conta == null) return null;

    conta.Saldo += valor;

    // Registar Transação
    var transacao = new Transacao
    {
        Tipo = "Deposito",
        Valor = valor,
        DataHora = DateTime.UtcNow,
        ContaId = conta.Id
    };

    _context.Transacoes.Add(transacao);
    _context.Contas.Update(conta);
    _context.SaveChanges();

    return conta;
}

    public Conta? Sacar(int id, decimal valor)
    {
        var conta = ObterPorId(id);
        if (conta == null) return null;

        if (conta.Saldo < valor)
        {
            throw new InvalidOperationException("Saldo insuficiente para realizar o saque.");
        }

        conta.Saldo -= valor;

    // Registar Transação
        var transacao = new Transacao
        {
            Tipo = "Saque",
            Valor = valor,
            DataHora = DateTime.UtcNow,
            ContaId = conta.Id
            };

        _context.Transacoes.Add(transacao);
        _context.Contas.Update(conta);
        _context.SaveChanges();

        return conta;
    }

    public bool Transferir(int contaOrigemId, int contaDestinoId, decimal valor)
    {
        if (contaOrigemId == contaDestinoId)
        {
            throw new InvalidOperationException("Não é possível realizar uma transferência para a mesma conta.");
        }

        var contaOrigem = ObterPorId(contaOrigemId);
        var contaDestino = ObterPorId(contaDestinoId);

    if (contaOrigem == null || contaDestino == null)
    {
        return false;
    }

    if (contaOrigem.Saldo < valor)
    {
        throw new InvalidOperationException("Saldo insuficiente para realizar a transferência.");
    }

        contaOrigem.Saldo -= valor;
        contaDestino.Saldo += valor;

        var dataAgora = DateTime.UtcNow;

        // Registar Histórico na Conta de Origem
        _context.Transacoes.Add(new Transacao
        {
            Tipo = "TransferenciaEnviada",
            Valor = valor,
             DataHora = dataAgora,
            ContaId = contaOrigem.Id
        });

        // Registar Histórico na Conta de Destino
        _context.Transacoes.Add(new Transacao
         {
             Tipo = "TransferenciaRecebida",
             Valor = valor,
             DataHora = dataAgora,
             ContaId = contaDestino.Id
        });

        _context.Contas.Update(contaOrigem);
        _context.Contas.Update(contaDestino);
        _context.SaveChanges();

        return true;
    }

    // NOVO MÉTODO: Obter o extrato da conta
    public List<Transacao>? ObterExtrato(int contaId)
    {
        var conta = ObterPorId(contaId);
        if (conta == null) return null;

        return _context.Transacoes
            .Where(t => t.ContaId == contaId)
            .OrderByDescending(t => t.DataHora)
            .ToList();
}
}