using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.Mapping;
using Microsoft.OpenApi.Models;
using Npgsql;
using TreeLogic.Core;
using TreeLogic.Core.Abstractions;
using TreeLogic.Core.Data;
using TreeLogic.Core.Data.Linq2Db;
using TreeLogic.Core.Data.Postgres;
using TreeLogic.Core.Services;
using TreeLogic.Processing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "My API",
		Version = "v1",
		Description = "An example of ASP.NET Core Web API with Swagger",
		Contact = new OpenApiContact
		{
			Name = "Developer",
			Email = "developer@example.com"
		},
		License = new OpenApiLicense
		{
			Name = "MIT"
		}
	});
});


builder.Services.AddTreeLogic()
	.WithData()
	.WithProcessing(new RoutineProcessingServiceOptions
	{
		ThreadCount = 2
	})
	.WithLinq2Db(() => new DataConnection(LinqToDB.ProviderName.PostgreSQL, "cs", MappingSchema.Default));
	//.WithPostgres("cs");

builder.Host.ConfigureServices(services =>
{
	services.AddHostedService<RoutineProcessingService>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

var summaries = new[]
{
	"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
	{
		var forecast = Enumerable.Range(1, 5).Select(index =>
				new WeatherForecast
				(
					DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
					Random.Shared.Next(-20, 55),
					summaries[Random.Shared.Next(summaries.Length)]
				))
			.ToArray();
		return forecast;
	})
	.WithName("GetWeatherForecast");

app.MapGet("/exec", async context =>
{
	var routineProvider = app.Services.GetService<IRoutineProvider>();
	var echoRoutine = routineProvider.GetRoutine<ComplexRoutine>(null);
	
	var writer = app.Services.GetService<RoutineProcessingWriter>();
	writer.WriteRoutine(echoRoutine);
	
	await context.Response.WriteAsync("DONE");
});

app.MapPost("/query", async context =>
{
	await context.Response.WriteAsync("DONE");
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}