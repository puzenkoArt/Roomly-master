using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Roomly.Rooms.Extensions;
using Roomly.Rooms.Infrastructure.Rabbit;
using Roomly.Rooms.Services;
using Roomly.Shared.Data;
using Roomly.Shared.Options;
using Roomly.Users.Infrastructure.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRoomService, RoomService>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Room API",
    });

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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"), sqlOptions => sqlOptions.MigrationsAssembly("Roomly.Rooms"));
});

builder.Services.AddAutoMapper(typeof(RoomProfile));

var rabbitOptions = builder.Configuration.GetSection("RabbitOptions").Get<RabbitOptions>();

builder.Services.AddMassTransit(m =>
{
    m.AddConsumers(typeof(RoomAvailabilityConsumer));
    m.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.ReceiveEndpoint("room-availability-queue", e =>
        {
            e.ConfigureConsumer<RoomAvailabilityConsumer>(ctx);
        });

        cfg.Host(rabbitOptions.HostName, "/", c =>
        {
            c.Username(rabbitOptions.UserName);
            c.Password(rabbitOptions.Password);
        });
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

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