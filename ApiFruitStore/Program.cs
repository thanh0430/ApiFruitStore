using ApiFruitStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiFruitStore.Repository;
using ApiFruitStore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<FruitStoreContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(Option =>
{
    Option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    Option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(Option =>
{
    Option.SaveToken = true;
    Option.RequireHttpsMetadata = false;
    Option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["jwt:ValidAudience"],
        ValidIssuer = builder.Configuration["jwt:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Secret"]))
    };
});

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("FruitStorePolicy", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => origin.StartsWith("http://") || origin.StartsWith("https://"))
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<IVnpayServices, VnpayServices>();

builder.Services.AddDbContext<FruitStoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("FruitStore"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

// Apply the "FruitStorePolicy" CORS policy to all requests
app.UseCors("FruitStorePolicy");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "image")),
    RequestPath = "/Image"
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
