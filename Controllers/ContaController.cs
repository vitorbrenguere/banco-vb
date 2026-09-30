using banco_vb.DTOs;
using banco_vb.Models;
using banco_vb.Services;
using Microsoft.AspNetCore.Mvc;

namespace banco_vb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaController : ControllerBase
{
    private readonly ContaService _contaService;

    public ContaController(ContaService contaService)
    {
        _contaService = contaService;
    }

    // GET: api/Conta
    [HttpGet]
    public ActionResult<List<Conta>> ObterTodas()
    {
        return Ok(_contaService.ObterTodas());
    }

    // GET: api/Conta/1
    [HttpGet("{id}")]
    public ActionResult<Conta> ObterPorId(int id)
    {
        var conta = _contaService.ObterPorId(id);
        if (conta == null)
            return NotFound(new { mensagem = $"Conta com ID {id} não encontrada." });

        return Ok(conta);
    }

    // POST: api/Conta
    [HttpPost]
    public ActionResult<Conta> Adicionar([FromBody] ContaCriacaoDto dto)
    {
        var novaConta = _contaService.Adicionar(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = novaConta.Id }, novaConta);
    }

    // PUT: api/Conta/1
    [HttpPut("{id}")]
    public ActionResult<Conta> Atualizar(int id, [FromBody] ContaCriacaoDto dto)
    {
        var contaAtualizada = _contaService.Atualizar(id, dto);
        if (contaAtualizada == null)
            return NotFound(new { mensagem = $"Conta com ID {id} não encontrada." });

        return Ok(contaAtualizada);
    }

    // DELETE: api/Conta/1
    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        var removido = _contaService.Remover(id);
        if (!removido)
            return NotFound(new { mensagem = $"Conta com ID {id} não encontrada." });

        return NoContent();
    }

// --- NOVOS ENDPOINTS DA FASE 4 ---

    // POST: api/Conta/1/deposito
    [HttpPost("{id}/deposito")]
    public ActionResult<Conta> Depositar(int id, [FromBody] OperacaoDto dto)
    {
        var conta = _contaService.Depositar(id, dto.Valor);
        if (conta == null)
            return NotFound(new { mensagem = $"Conta com ID {id} não encontrada." });

        return Ok(conta);
    }

    // POST: api/Conta/1/saque
    [HttpPost("{id}/saque")]
    public ActionResult<Conta> Sacar(int id, [FromBody] OperacaoDto dto)
    {
        try
        {
            var conta = _contaService.Sacar(id, dto.Valor);
            if (conta == null)
                return NotFound(new { mensagem = $"Conta com ID {id} não encontrada." });

            return Ok(conta);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }
}