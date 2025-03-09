using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Roomly.Booking.Services;
using Roomly.Rooms.Extensions;
using Roomly.Shared.Data;
using Roomly.Shared.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"), sqlOptions => sqlOptions.MigrationsAssembly("Roomly.Users"));
});

builder.Services.Configure<RabbitOptions>(builder.Configuration.GetSection("RabbitOptions"));

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

var rabbitOptions = builder.Configuration.GetSection("RabbitOptions").Get<RabbitOptions>();

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddMassTransit(x=>
{ 
    x.UsingRabbitMq((ctx,cfg)=>
    { 
        cfg.Host(rabbitOptions.HostName,"/" , c=> 
        { 
            c.Username(rabbitOptions.UserName);
            c.Password(rabbitOptions.Password); 
        }); 
        cfg.ConfigureEndpoints(ctx); 
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

app.UseCors("AllowSpecificOrigin");

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.None
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();