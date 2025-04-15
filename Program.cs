using System.Text;
using asp_net_core8_webApiSample.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 設定 Serilog 並取代預設 logging
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

// 加入 Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "asp_net_core8_webApiSample", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "請在此處輸入以 Bearer 開頭的 JWT Token，如：Bearer {token}"
    });
    // 只針對有 [Authorize] 的 API 顯示鎖頭
    options.OperationFilter<asp_net_core8_webApiSample.AuthorizeCheckOperationFilter>();
});

// 加入 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// 加入 Controller 支援
builder.Services.AddControllers();

// JWT 設定（範例金鑰，實務請放 appsettings）
var jwtKey = "ThisIsASecretKeyForJwtToken123456!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// 註冊 JwtTokenService
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// 註冊 HttpClient 與 HttpService 以支援相依性注入
builder.Services.AddHttpClient<HttpService>();

// 註冊 LdapService
var ldapConfig = builder.Configuration.GetSection("LDAP");
builder.Services.AddScoped<LdapService>(sp =>
    new LdapService(
        ldapConfig["LdapPath"] ?? string.Empty,
        ldapConfig["Username"] ?? string.Empty,
        ldapConfig["Password"] ?? string.Empty
    )
);

builder.Services.AddAuthorization();

var app = builder.Build();

// 啟用 Swagger
app.UseSwagger();
app.UseSwaggerUI();

// 啟用全域例外處理
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
