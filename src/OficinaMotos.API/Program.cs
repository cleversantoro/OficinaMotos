using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OficinaMotos.API.Middlewares;
using OficinaMotos.Application.IoC;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.IoC;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração de LOGS (Serilog) - Observabilidade (Logs Estruturados)
// Isso permite enviar logs para ElasticSearch, Seq ou Arquivo Texto
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Configuracoes OpenTelemetry (service name e exporters)
var otelConfig = builder.Configuration.GetSection("OpenTelemetry");
var otlpEndpoint = otelConfig["OtlpEndpoint"];
var enablePrometheus = otelConfig.GetValue<bool>("EnablePrometheus");
var serviceName = otelConfig["ServiceName"] ?? "OficinaMotos.API";
// 2. Configuração de TELEMETRIA (OpenTelemetry) - Observabilidade (Métricas e Tracing)
// Prepara a API para monitoramento profissional (Prometheus, Grafana, Jaeger)
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation(); // Monitora queries do banco

        if (!string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            tracing.AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint));
        }
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();

        if (enablePrometheus)
        {
            metrics.AddPrometheusExporter();
        }

        if (!string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            metrics.AddOtlpExporter(options => options.Endpoint = new Uri(otlpEndpoint));
        }
    });

// 3. Banco de Dados (MySQL)
var connectionString = builder.Configuration.GetConnectionString("OficinaDb");
builder.Services.AddDbContext<OficinaContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 4. CORS (Segurança)
// Permite que o Angular (localhost:4200) converse com a API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // URL do Frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// 5. Autenticação JWT (Segurança)
var jwtKey = builder.Configuration["Jwt:Key"]; // Chave secreta no appsettings.json
if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("Configuração obrigatória ausente: Jwt:Key. Defina a chave JWT por variável de ambiente ou configuração externa antes de iniciar a API.");
}

var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Em produção, mude para true
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// 6. Injeção de Dependência (IoC) - SOLID (DIP)
// Aqui registraremos nossos Services e Repositories
builder.Services.AddInfrastructure(builder.Configuration); // Método de extensão criado na camada Infra
builder.Services.AddApplication(); // Método de extensão criado na camada Application
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// 7. Swagger com Suporte a JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Oficina Motos API", Version = "v1" });

    // Configura o botão "Authorize" no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// --- PIPELINE DE REQUISIÇÃO (Middleware) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); // Loga cada requisição HTTP automaticamente
app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve arquivos de wwwroot (uploads, etc.)
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception is null)
        {
            return;
        }

        var handler = context.RequestServices.GetRequiredService<IExceptionHandler>();
        await handler.TryHandleAsync(context, exception, context.RequestAborted);
    });
});

app.UseCors("AllowAngularApp"); // Aplica o CORS

app.UseAuthentication(); // Quem eh voce?
app.UseAuthorization();  // O que voce pode fazer?

if (enablePrometheus)
{
    app.MapPrometheusScrapingEndpoint();
}

app.MapControllers();

app.Run();




