using banco_vb.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Registra os Controllers da aplicação
builder.Services.AddControllers();

// 2. Registra o nosso serviço de contas
builder.Services.AddSingleton<ContaService>();

var app = builder.Build();

// Rotas informativas gerais (opcional)
app.MapGet("/api/status", () => 
{
    return new 
    { 
        mensagem = "API do Banco VB rodando com sucesso!", 
        status = "Ativo",
        horario = DateTime.Now
    };
});

// 3. Mapeia automaticamente todas as rotas criadas nos Controllers
app.MapControllers();

app.Run();