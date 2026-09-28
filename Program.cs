using banco_vb.Models;
using banco_vb.Services;

var builder = WebApplication.CreateBuilder(args);

// Registra o serviço no container de Injeção de Dependências
builder.Services.AddSingleton<ContaService>();

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

// READ (Obter todas)
app.MapGet("/api/conta", (ContaService service) =>
{
    return service.ObterTodas();
});

// READ (Obter por ID)
app.MapGet("/api/conta/{id}", (int id, ContaService service) =>
{
    var conta = service.ObterPorId(id);

    if (conta == null)
    {
        return Results.NotFound(new { mensagem = "Conta não encontrada!" });
    }

    return Results.Ok(conta);
});

// CREATE (Criar nova conta)
app.MapPost("/api/conta", (Conta novaConta, ContaService service) =>
{
    var contaCriada = service.Adicionar(novaConta);
    return Results.Created($"/api/conta/{contaCriada.Id}", contaCriada);
});

// DELETE (Remover conta por ID)
app.MapDelete("/api/conta/{id}", (int id, ContaService service) =>
{
    var removido = service.Remover(id);

    if (!removido)
    {
        return Results.NotFound(new { mensagem = "Conta não encontrada para remoção!" });
    }

    return Results.NoContent(); // Código 204: Sucesso sem conteúdo de retorno
});

app.Run();