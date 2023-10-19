using FilmesAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FilmeConnection");

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //Título e versão da API:
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FilmesAPI", Version = "v1" });
    //Gerando arquivo XML a partir de uma lib (para não precisar gerar de forma manual):
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //Gerando um caminho para esse arquivo XML:
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //Incluindo comentários na documentação da nossa API a partir do XML:
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddDbContext<FilmeContext>(opts => opts.UseLazyLoadingProxies().UseSqlServer(connectionString));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
