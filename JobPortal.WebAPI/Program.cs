using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("MainDb");
//builder.Services.AddDbContext<JobPortalDbContext>(options =>
//    options.UseSqlServer(connectionString));

// Add debug for SQL stmts
builder.Services.AddDbContext<JobPortalDbContext>(options =>
    options.UseSqlServer(connectionString)
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CVService>();
builder.Services.AddScoped<JobAdService>();
builder.Services.AddScoped<CoverLetterService>();
builder.Services.AddScoped<JobApplicationService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService>(sp => new JwtService(builder.Configuration["Jwt:Secret"]));
builder.Services.AddScoped<IAICoverLetterService, OllamaAIService>();
builder.Services.AddHttpClient<OllamaAIService>(c => { c.BaseAddress = new Uri("http://localhost:11434"); });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// ToDo: temp, remove in production or use proxy server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// HTTPS is done via Apache Reverse Proxy


/*if (app.Environment.IsDevelopment())
{
    
}*/
app.UseCors("AllowAngular");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();