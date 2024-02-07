using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TodoApp.Contexts;
using TodoApp.Handlers;
using TodoApp.Models.JWT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoAPI", Version = "v1" });
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                      new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] {}
                }
            });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionStringSqlServer = builder.Configuration.GetConnectionString("SqlServerConnection");
var connectionStringPostgreSql = builder.Configuration.GetConnectionString("PostgreSqlConnection");
builder.Services.AddDbContext<TodoDBContext>(options =>
{
    options.UseMySQL(connectionString);

});

builder.Services.AddDbContext<TodoDBContextSqlServer>(options =>
{
    options.UseSqlServer(connectionStringSqlServer);

});

builder.Services.AddDbContext<TodoDBContextPostgreSQL>(options =>
{
    options.UseNpgsql(connectionStringPostgreSql);

});

//builder.Services.AddAuthentication("BasicAuthentication")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


// add authentication bearer
var key = builder.Configuration["Jwt:Secret"]; // lấy key từ app settings
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // yêu cầu có kiểm tra issue default = true
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            // Yêu cầu kiểm tra về audience default = true
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            // chỉ ra token phải cấu hình expire
            RequireExpirationTime = true,
            ValidateLifetime = true,
            // Chỉ ra key mà token sẽ dùng sau này
            IssuerSigningKey = signingKey,
            RequireSignedTokens = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
