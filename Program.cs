using banco_vb.Services;
using banco_vb.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ContaService>();

// 1. Adiciona os serviços do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2.Registra o BancoDbContext usando o SQLite
builder.Services.AddDbContext<BancoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 3. Ativa a interface visual do Swagger no ambiente de desenvolvimento
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