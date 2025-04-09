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

MapDomain.Map();

builder.Services.AddTreeLogic()
	.WithData()
	.WithProcessing(new RoutineProcessingServiceOptions
	{
		ThreadCount = 1
	})
	.WithLinq2Db(() => new DataConnection(ProviderName.PostgreSQL, "Server=localhost;Port=5432;Database=postgres;User Id=postgres;Password=sa;", MapDomain.MappingSchema));
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

app.MapGet("/exec", async context =>
{
	var writer = app.Services.GetService<RoutineProcessingWriter>();
	var routineProvider = app.Services.GetService<IRoutineProvider>();

	Func<IQueryable<USER>, IQueryable<USER>> query = (users) => users.Where(x => x.Age > 25);

	var r = routineProvider.GetRoutine<QueryDataObjectRoutine<USER>>(query);


	var afterExecute = async (StageRoutineResult result) =>
	{
		var users = result.Result as List<USER>;
		if (users != null)
		{
			await context.Response.WriteAsync("USERS COUNT: " + users.Count);
		}
		else
		{
			await context.Response.WriteAsync("ERROR");
		}
	};
	
	// writer.WriteRoutine(r);
	
	var task = writer.WriteRoutineAsync(r, afterExecute);
	
	await task;
});

app.MapGet("/query", async context =>
{
	var writer = app.Services.GetService<RoutineProcessingWriter>();
	var routineProvider = app.Services.GetService<IRoutineProvider>();

	Func<IQueryable<USER>, IQueryable<USER>> query = (users) => users.Where(x => x.Age > 25);

	var r = routineProvider.GetRoutine<QueryDataObjectRoutine<USER>>(query);
	writer.WriteRoutine(r);
	
	await context.Response.WriteAsync("DONE");
});

app.Run();