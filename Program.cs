using CnpjMailingApi.Data;
using CnpjMailingApi.Repos;
using CnpjMailingApi.Services;
using Microsoft.OpenApi.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Default");

// Registra o DbConnectionFactory com a string de conexão
builder.Services.AddSingleton<DbConnectionFactory>(new DbConnectionFactory(connectionString!));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CNPJ Search API",
        Version = "v1",
        Description = "Api para retornar CNPJs com base em filtros como cidades, CNAEs, município, etc. Também é possível gerar um arquivo CSV com os CNPJs retornados e pesquisar um CNPJ para retornar suas informações. "
    });
});

builder.Services.AddScoped<ICnpjMailingRepository, CnpjMailingRepository>();
builder.Services.AddScoped<ICnpjDataRepository, CnpjDataRepository>();
builder.Services.AddScoped<CnpjDataService>();
builder.Services.AddScoped<CnpjMailingService>();


var app = builder.Build();

try
{
    await using var connection = new NpgsqlConnection(connectionString);
    await connection.OpenAsync();
    Console.WriteLine("Conexão bem-sucedida!");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CNPJ Search API v1");
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
