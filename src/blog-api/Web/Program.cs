using System.Text;
using BlogApi.Application.Interfaces;
using BlogApi.Application.Services;
using BlogApi.Application.Validators;
using BlogApi.Domain.Interface;
using BlogApi.Infra.Data;
using BlogApi.Infra.Exceptions;
using BlogApi.Infra.Repositories;
using BlogApi.Web.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(n => n.FullName);
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Blog API", 
        Version = "v1",
        Description = "A simple blog API"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme 
            {
                Reference = new OpenApiReference 
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var singingKey = builder.Configuration.GetValue<string>("JWT:SigningKey");
        if (string.IsNullOrEmpty(singingKey))
        {
            throw new InvalidOperationException("Signing key não está configurada.");
        }

        var key = Encoding.ASCII.GetBytes(singingKey!);

        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<BlogContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PgSqlConnection") ?? string.Empty));

builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBlogPostDtoValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog API V1");
    });
}

app.MapEndpoints();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePages();
app.UseExceptionHandler();

app.Run();