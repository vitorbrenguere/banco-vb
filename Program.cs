using banco_vb.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/status", () => 
{
    return new 
    { 
        mensagem = "API do Banco VB rodando com sucesso!", 
        status = "Ativo",
        horario = DateTime.Now
    };
});

// Novo endpoint: Retorna os dados de uma conta bancária
app.MapGet("/api/conta", () =>
{
var contas = new List<Conta>
    {
        new Conta { Id = 1, Titular = "Vitor Brenguere", Saldo = 1500.50m },
        new Conta { Id = 2, Titular = "Neymar Jr", Saldo = 7200.00m },
        new Conta { Id = 3, Titular = "Allen Iverson", Saldo = 5450.75m }
    };

    return contas;
});

// Endpoint para buscar uma conta específica pelo ID
app.MapGet("/api/conta/{id}", (int id) =>
{
    var contas = new List<Conta>
    {
        new Conta { Id = 1, Titular = "Vitor Brenguere", Saldo = 1500.50m },
        new Conta { Id = 2, Titular = "Neymar Jr", Saldo = 7200.00m },
        new Conta { Id = 3, Titular = "Allen Iverson", Saldo = 5450.75m }
    };

    // Procura a conta que tem o mesmo ID recebido na URL
    var contaEncontrada = contas.FirstOrDefault(c => c.Id == id);

    if (contaEncontrada == null)
    {
        return Results.NotFound(new { mensagem = "Conta não encontrada!" });
    }

    return Results.Ok(contaEncontrada);
});

app.Run();