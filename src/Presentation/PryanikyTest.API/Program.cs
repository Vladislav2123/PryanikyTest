using PryanikyTest.API.ExceptionsHandling;
using PryanikyTest.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseGlobalExceptionsHandling();

app.UseAuthorization();

app.MapControllers();

app.Run();
