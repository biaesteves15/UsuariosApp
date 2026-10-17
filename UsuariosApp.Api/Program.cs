using UsuariosApp.Application.Services;
using UsuariosApp.Domain.Contracts;
using UsuariosApp.Infra.Data.Contexts;
using UsuariosApp.Infra.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

#region Configuração das injeções de dependência

builder.Services.AddScoped<UsuarioService>(); //APPLICATION
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); //DOMAIN + INFRA.DATA
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>(); //DOMAIN + INFRA.DATA
builder.Services.AddScoped<DataContext>(); //INFRA

#endregion

#region Configuração do Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region Executando o Swagger

app.UseSwagger();
app.UseSwaggerUI();

#endregion

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
