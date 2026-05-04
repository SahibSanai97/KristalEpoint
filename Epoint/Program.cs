using Epoint.Models;
using Epoint.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<EpointSettings>(
    builder.Configuration.GetSection("Epoint"));


builder.Services.AddHttpClient<IEpointService, EpointService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Epoint.az Payment Gateway API",
        Version = "v1",
        Description = "C# .NET 8 wrapper for the Epoint.az electronic payment platform (v1.0.3). " +
                      "Set your PublicKey and PrivateKey in appsettings.json before use."
    });
    // Include XML comments if generated
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Epoint API v1");
        c.RoutePrefix = string.Empty; // Swagger UI at root
    });


app.UseHttpsRedirection();


app.MapControllers();

app.Run();