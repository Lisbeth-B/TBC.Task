using Microsoft.EntityFrameworkCore;
using TBC.Task.Api.Middleware;
using TBC.Task.Core.Interfaces;
using TBC.Task.Infrastructure.Database;
using TBC.Task.Infrastructure.Interfaces;
using TBC.Task.Infrastructure.Mappers;
using TBC.Task.Infrastructure.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddLogging();

builder.Services.AddTransient<AcceptLanguageFromHeaderMiddleware>();

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddScoped<ICityRepository, CityRepository>();

builder.Services.AddScoped<IPersonMapper, PersonMapper>();


WebApplication app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseMiddleware<AcceptLanguageFromHeaderMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
