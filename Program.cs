using BtgItDerivApiPedidos.Configurations;
using MassTransit;
using BtgItDerivApiPedidos.Consumers;

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

// // Adicionando RabbitMQ
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQ"));

// Adicionando RabbitMQ via MassTransit
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


app.Run();
