using IzinTakip.Api.Helpers;
using IzinTakip.Core.Abstractions;
using IzinTakip.Core.DataAccess;
using IzinTakip.Core.Helpers;
using IzinTakip.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("HerkesIzinli", policy =>
    {
        // TODO: Üretimde sadece belirli originlere izin verilmeli
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// TODO: Üretim ortamında LocalDB yerine gerçek SQL Server bağlantısı kullanılacak.
// Bağlantı dizesi bir secrets manager'a (örn. Azure Key Vault) taşınmalı.

// ── Servisler ──────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger'a JWT desteği ekle, authorize butonu çıkacak ve token girebileceğiz.
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT token girin: Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Tatil sağlayıcısını ayarla (WinForms'taki Program.cs ile aynı)
DateHelper.TatilSaglayiciyiAyarla(new NagerPlusHijriTatilRepository());

// Bağımlılık enjeksiyonu
builder.Services.AddSingleton<IDbHelper, SqlHelper>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IIzinService, IzinService>();
builder.Services.AddScoped<IRaporService, RaporService>();
builder.Services.AddScoped<JwtHelper>();

// JWT kimlik doğrulama
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AmirVeyaYonetici", policy =>
        policy.RequireRole("Amir", "Yönetici"));

    options.AddPolicy("YoneticiOnly", policy =>
        policy.RequireRole("Yönetici"));
});

// ── Pipeline ───────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("HerkesIzinli");
// app.UseHttpsRedirection(); // TODO: Üretimde açılmalı, şimdilik emülatör için kapalı
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();