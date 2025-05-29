using System.Reflection;
using System.Text;
using Azure.Identity;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using myapi;
using myapi.Controllers;
using myapi.Data;
using Razorpay.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(f => f.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuser"],
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
