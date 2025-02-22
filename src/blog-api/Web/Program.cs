using BlogApi.Infra.Data;
using BlogApi.Infra.Extensions;
using BlogApi.Web.Setup;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.ConfigureServices();

var app = builder.Build();

await app.MigrateDatabase<BlogContext>(Log.Logger);

app.ConfigureMiddleware();

app.Run();