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
}