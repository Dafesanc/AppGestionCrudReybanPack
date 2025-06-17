using BackEndApi.Data;
using BackEndApi.Interfaces;
using BackEndApi.Mappings;
using BackEndApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuración de Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Configuración de repositorios
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();

// Configuración de controladores con validación de modelo
builder.Services.AddControllers(options =>
{
    // Configuraciones adicionales para los controladores
})
.ConfigureApiBehaviorOptions(options =>
{
    // Personalizar respuestas de validación automática
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var response = new BackEndApi.Common.ApiResponse<object>
        {
            Success = false,
            Message = "Datos de entrada inválidos",
            Errors = errors
        };

        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(response);
    };
});

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CRUD API - Persons & Pets",
        Version = "v1",
        Description = "API para gestión de personas y mascotas con operaciones CRUD completas",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Desarrollo Backend",
            Email = "dev@ejemplo.com"
        }
    });

    // Incluir comentarios XML si existen
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRUD API v1");
        c.RoutePrefix = string.Empty; // Para acceder a Swagger en la raíz
    });
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Endpoint de salud
app.MapGet("/health", () => new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow,
    Version = "1.0.0",
    Environment = app.Environment.EnvironmentName
});

app.Run();
