using banco_vb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ContaService>();

// 1. Adiciona os serviços do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Ativa a interface visual do Swagger no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/status", () => 
{
    return new 
    { 
        mensagem = "API do Banco VB rodando com sucesso!", 
        status = "Ativo",
        horario = DateTime.Now
    };
});

app.MapControllers();

app.Run();