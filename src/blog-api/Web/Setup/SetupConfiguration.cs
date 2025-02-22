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
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace BlogApi.Web.Setup;

public static class StartupConfiguration
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
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

            ConfigureSwaggerAuth(c);
        });

        ConfigureAuth(builder);
        ConfigureDatabase(builder);
        ConfigureAppServices(builder);

        return builder;
    }

    private static void ConfigureSwaggerAuth(SwaggerGenOptions c)
    {
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
    }

    private static void ConfigureAuth(WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var signingKey = builder.Configuration.GetValue<string>("JWT:SigningKey") 
                    ?? throw new InvalidOperationException("Signing key não está configurada.");

                var key = Encoding.ASCII.GetBytes(signingKey);

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
    }

    private static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BlogContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PgSqlConnection") 
                ?? throw new InvalidOperationException("Connection string não está configurada.")
            ));
    }

    private static void ConfigureAppServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBlogRepository, BlogRepository>();
        builder.Services.AddScoped<IBlogService, BlogService>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateBlogPostDtoValidator>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
    }

    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
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

        return app;
    }
}