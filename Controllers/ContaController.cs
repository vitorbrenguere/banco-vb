using banco_vb.Models;
using banco_vb.Services;
using Microsoft.AspNetCore.Mvc;

namespace banco_vb.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaController : ControllerBase
{
    private readonly ContaService _contaService;

    // Injeção de Dependência do ContaService via construtor
    public ContaController(ContaService contaService)
    {
        _contaService = contaService;
    }

    [HttpGet]
    public ActionResult<List<Conta>> ObterTodas()
    {
        var contas = _contaService.ObterTodas();
        return Ok(contas);
    }

    [HttpGet("{id}")]
    public ActionResult<Conta> ObterPorId(int id)
    {
        var conta = _contaService.ObterPorId(id);

        if (conta == null)
        {
            return NotFound(new { mensagem = "Conta não encontrada!" });
        }

        return Ok(conta);
    }

    [HttpPost]
    public ActionResult<Conta> Adicionar([FromBody] Conta novaConta)
    {
        var contaCriada = _contaService.Adicionar(novaConta);
        return CreatedAtAction(nameof(ObterPorId), new { id = contaCriada.Id }, contaCriada);
    }

    [HttpDelete("{id}")]
    public IActionResult Remover(int id)
    {
        var removido = _contaService.Remover(id);

        if (!removido)
        {
            return NotFound(new { mensagem = "Conta não encontrada para remoção!" });
        }

        return NoContent();
    }
}