using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Burguer Royale",
		Version = "v1"
	});

	options.IncludeXmlComments
	(
		Path.Combine
		(
			AppContext.BaseDirectory,
			$"{Assembly.GetExecutingAssembly().GetName().Name}.xml"
		)
	);

	options.EnableAnnotations();
});

builder.Services.AddHealthChecks();

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