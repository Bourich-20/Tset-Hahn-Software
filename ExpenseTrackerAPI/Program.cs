using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Repositories;
using ExpenseTrackerAPI.Services;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services nécessaires pour l'application
builder.Services.AddControllers();

// Configurer Swagger pour l'API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExpenseTrackerAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
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

// Récupérer les paramètres de la clé secrète et de la configuration du JWT
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("The JwtSettings:SecretKey is not configured.");
}
var jwtKey = Encoding.UTF8.GetBytes(secretKey);

// Configurer l'authentification JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
        };

        // Ajout d'événements pour diagnostiquer les problèmes JWT
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token non fourni ou mal formaté dans l'en-tête Authorization.");
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Échec de l'authentification : {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token JWT validé avec succès.");
                return Task.CompletedTask;
            }
        };
    });

// Configuration de la chaîne de connexion et du DbContext (MySQL dans cet exemple)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

// Enregistrement des services nécessaires
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<PasswordHasher<User>>(); // Gestion des mots de passe
builder.Services.AddScoped<JwtService>();          // Service pour JWT
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

// Configuration de CORS pour permettre l'accès depuis des origines spécifiques
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200", "http://localhost:5062")
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});

var app = builder.Build();

// Utilisation de Swagger pour la documentation de l'API en mode développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpenseTrackerAPI v1");
    });
}

// Configurer les middlewares
app.UseHttpsRedirection();  // Redirection vers HTTPS
app.UseCors("AllowAll");    // Appliquer la politique CORS
app.UseAuthentication();    // Authentification JWT
app.UseAuthorization();     // Autorisation pour les utilisateurs authentifiés

// Mapper les contrôleurs
app.MapControllers();

// Démarrer l'application
app.Run();
