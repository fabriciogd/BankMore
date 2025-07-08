using BankMore.Account.Application;
using BankMore.Account.Infraestructure;
using BankMore.Core.API;
using BankMore.Core.API.Extensions;
using BankMore.Core.API.Filters;
using BankMore.Core.API.Middleware;
using BankMore.Core.Application;
using BankMore.Core.Application.Iterfaces;
using BankMore.Core.Infraestructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(builder.Configuration));

// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add<JwtFilter>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddBearerAuthentication();
});

builder.Services
    .AddCommandHandlers(BankMore.Account.Application.AssemblyReference.Assembly)
    .AddSqliteContext(builder.Configuration)
    .AddServices()
    .AddBearerAuthentication(builder.Configuration)
    .AddIdempotency()
    .AddRepositories()
    .AddValidators();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
    await db.RunMigrationsAsync(BankMore.Account.API.AssemblyReference.Assembly);
}

app.UseMiddleware<ExceptionMiddleware>();

app.Run();

public partial class Program { }