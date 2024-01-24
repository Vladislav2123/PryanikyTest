using MediatR;
using PryanikyTest.API.ExceptionsHandling;
using PryanikyTest.Application;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddDal(builder.Configuration);

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

await InitializeDb();

app.Run();

async Task InitializeDb()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider
            .GetService<IApplicationDbContext>();

        var mediator = scope.ServiceProvider
            .GetService<IMediator>();

        await DbInitializer.InitializeAsync(
            dbContext, 
            mediator, 
            app.Environment);
    }
}