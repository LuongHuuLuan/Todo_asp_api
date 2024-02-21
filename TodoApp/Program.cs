using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using TodoApp.Contexts;
using TodoApp.Handlers;
using TodoApp.Models.JWT;
using TodoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoAPI", Version = "v1" });
//    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "basic",
//        In = ParameterLocation.Header,
//        Description = "Basic Authorization header using the Bearer scheme."
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//            {
//                {
//                      new OpenApiSecurityScheme
//                        {
//                            Reference = new OpenApiReference
//                            {
//                                Type = ReferenceType.SecurityScheme,
//                                Id = "basic"
//                            }
//                        },
//                        new string[] {}
//                }
//            });
//});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TodoDBContext>(options =>
{
    options.UseMySQL(connectionString);

});
//var connectionStringSqlServer = builder.Configuration.GetConnectionString("SqlServerConnection");
//builder.Services.AddDbContext<TodoDBContextSqlServer>(options =>
//{
//    options.UseSqlServer(connectionStringSqlServer);
//});

//var connectionStringPostgreSql = builder.Configuration.GetConnectionString("PostgreSqlConnection");
//builder.Services.AddDbContext<TodoDBContextPostgreSQL>(options =>
//{
//    options.UseNpgsql(connectionStringPostgreSql);
//});

//builder.Services.AddAuthentication("BasicAuthentication")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

// add authentication bearer
var key = builder.Configuration["Jwt:AccessKey"]; // lấy key từ app settings
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // yêu cầu có kiểm tra issue default = true
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            // Yêu cầu kiểm tra về audience default = true
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            // chỉ ra token phải cấu hình expire
            RequireExpirationTime = true,
            ValidateLifetime = true,
            // Chỉ ra key mà token sẽ dùng sau này
            IssuerSigningKey = signingKey,
            RequireSignedTokens = true
        };
    });

builder.Services.AddTransient<IJWTTokenServices, JWTTokenServices>();

builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br/>
                      Enter your token in the text input below.
                      <br/>Example: '12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        { jwtSecurityScheme, new List<string>() }
      });
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
