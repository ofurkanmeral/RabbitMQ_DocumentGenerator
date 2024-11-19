using RabbitMQ.Client;
using System.Runtime.Intrinsics.X86;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConnection>(opt =>
{
    var factory = new ConnectionFactory()
    {
        Uri = new Uri("amqps://bdscqvep:1lYo-bElOmovxp9jn2i_AL681X_TmnLt@lionfish.rmq.cloudamqp.com/bdscqvep"),
        Ssl = new SslOption
        {
            Enabled = true,
            ServerName = "lionfish.rmq.cloudamqp.com"
        },
        AutomaticRecoveryEnabled = true,
        //NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
    };
    return factory.CreateConnection();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
