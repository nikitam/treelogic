using System.Text.Json;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.PostgreSQL;
using LinqToDB.Mapping;
using Microsoft.OpenApi.Models;
using Npgsql;
using TestDomain;
using TestDomain.Linq2Db;
using TreeLogic.Core;
using TreeLogic.Core.Abstractions;
using TreeLogic.Core.Common;
using TreeLogic.Core.Common.Abstractions;
using TreeLogic.Core.Data;
using TreeLogic.Core.Data.Linq2Db;
using TreeLogic.Core.Data.Postgres;
using TreeLogic.Core.Services;
using TreeLogic.Processing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// builder.Services.AddSwaggerGen(c =>
// {
// 	c.SwaggerDoc("v1", new OpenApiInfo
// 	{
// 		Title = "My API",
// 		Version = "v1",
// 		Description = "An example of ASP.NET Core Web API with Swagger",
// 		Contact = new OpenApiContact
// 		{
// 			Name = "Developer",
// 			Email = "developer@example.com"
// 		},
// 		License = new OpenApiLicense
// 		{
// 			Name = "MIT"
// 		}
// 	});
// });

Console.WriteLine(Environment.ProcessorCount);

MapDomain.Map();

builder.Services.AddTreeLogic()
	.WithCommon(new DefaultRoutineAssemblyProvider(typeof(GetUsersByAgeRoutine).Assembly))
	.WithData()
	.WithProcessing(new RoutineProcessingServiceOptions
	{
		ThreadCount = Environment.ProcessorCount
	})
	.WithLinq2Db(() => new DataConnection(ProviderName.PostgreSQL, "Server=localhost;Port=5432;Database=postgres;User Id=sa;Password=1;", MapDomain.MappingSchema));
	//.WithPostgres("cs");

builder.Host.ConfigureServices(services =>
{
	services.AddHostedService<RoutineProcessingService>();
});

var app = builder.Build();

app.MapGet("/ping", async context =>
{
	await context.Response.WriteAsync("PONG");
});

app.MapPost("/exec", async context =>
{
	var routineFactory = app.Services.GetService<IRoutineFactory>();
	var routine = await routineFactory.GetRoutineAsync(context.Request.Body);
	var writer = app.Services.GetService<RoutineProcessingWriter>();

	var afterExecute = async (StageRoutineResult result) =>
	{
		await context.Response.WriteAsJsonAsync(result.Result);
	};
	
	await writer.WriteRoutineAsync(routine, afterExecute);
});

app.Run();