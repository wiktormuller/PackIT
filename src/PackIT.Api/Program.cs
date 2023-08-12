using PackIT.Application;
using PackIT.Infrastructure;
using PackIT.Shared.Services;
using PackIT.Shared.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAppInitializer();
builder.Services.AddExceptionsHandling();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseReDoc(c => // https://localhost:7250/api-docs/index.html
    {
        c.DocumentTitle = "REDOC API Documentation";
        c.SpecUrl = "/swagger/v1/swagger.json";
    });
}

app.UseHttpsRedirection();
app.UseExceptionsHandling();

app.UseAuthorization();

app.MapControllers();

app.Run();
