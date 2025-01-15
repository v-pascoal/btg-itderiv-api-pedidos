using BtgItDerivApiPedidos.Configurations;
using BtgItDerivApiPedidos.Data;
using BtgItDerivApiPedidos.Consumers;
using BtgItDerivApiPedidos.Services;
using MassTransit;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Verificando ambiente
var environment = builder.Environment.EnvironmentName;
Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {environment}");

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BTG - IT Derivativos - Pedidos API",
        Version = "v1"
    });
});

// Carregando configs do RabbitMQ
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQ"));

// Adicionando MassTransit (RabbitMQ)
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PedidoConsumer>();
    x.UsingRabbitMq((context, config) =>
    {
        var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQConfiguration>();
        config.Host(rabbitMQConfig.HostName, h =>
        {
            h.Username(rabbitMQConfig.UserName);
            h.Password(rabbitMQConfig.Password);
        });
        config.ReceiveEndpoint(rabbitMQConfig.QueueName, e =>
        {
            e.Durable = true;
            e.AutoDelete = false;
            e.ConfigureConsumer<PedidoConsumer>(context);
        });

        // Ajuste nos serializadores padrões do masstransit;
        config.ClearMessageDeserializers();
        config.UseRawJsonSerializer();
    });
});

// Configurando MongoDB
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDBConfiguration>();
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDBConfiguration>();
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Registrando os repositórios
builder.Services.AddScoped<PedidoRepository>();
builder.Services.AddScoped<ClienteRepository>();

// Registrando os serviços da camada de negócios
builder.Services.AddScoped<PedidoService>();

// Registrando os controllers
builder.Services.AddControllers();

var app = builder.Build();

// Verificando ambiente do app
var appEnvironment = app.Environment.EnvironmentName;
Console.WriteLine($"APP ENVIRONMENT: {appEnvironment}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(s => {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "BTG - IT Derivativos - Pedidos API");
        s.RoutePrefix = string.Empty;
    });

}

//Endpoint do healthcheck
app.MapGet("/health", () => Results.Ok("Healthy"));

//Removendo o redirect https para swagger redirect funcionar no docker.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();
app.Run();
