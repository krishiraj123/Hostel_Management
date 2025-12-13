using System.Reflection;
using System.Text;
using Azure.Identity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using myapi;
using myapi.Controllers;
using myapi.Data;
using Razorpay.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddFluentValidation(f => f.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddEndpointsApiExplorer();

// --- REPOSITORIES ---
builder.Services.AddScoped<GlobalsDBUtils>();
builder.Services.AddScoped<Globals>();
builder.Services.AddScoped<HostelRepository>();
builder.Services.AddScoped<StudentRepository>();
builder.Services.AddScoped<ComplaintsRepository>();
builder.Services.AddScoped<RoomRepository>();
builder.Services.AddScoped<NotificationRepository>();
builder.Services.AddScoped<EmailServiceController>();
builder.Services.AddScoped<FoodTimeTableRepository>();
builder.Services.AddScoped<DashboardRepository>();
builder.Services.AddScoped<PaymentRepository>();

builder.Services.AddSwaggerGen(options =>
{    
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Enter your API Key below.\r\n\r\nExample: MySecretKey123",
        Type = SecuritySchemeType.ApiKey,
        Name = "ApiKey",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.\r\n\r\nExample: '12345abcdef'",
        Name = "Authorization", 
        In = ParameterLocation.Header, 
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new List<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"], 
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddSingleton<RazorpayClient>(serviceProvider => {
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return new RazorpayClient(
        configuration["Razorpay:KeyId"],
        configuration["Razorpay:KeySecret"]);
});

builder.Services.AddAuthorization();
builder.Services.AddExceptionHandler<GlobalExceptionsHandlerMiddleware>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200") 
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyAuthenticationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();