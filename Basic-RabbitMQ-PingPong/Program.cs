using Basic_RabbitMQ_PingPong.ClientsHub;
using Basic_RabbitMQ_PingPong.ConfigService;
using Basic_RabbitMQ_PingPong.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string? rabbitMqServiceHost = builder.Configuration.GetConnectionString("Host");

if (rabbitMqServiceHost == null)
{
    throw new Exception();
}


builder.Services.AddSingleton<IConfigService, ConfigService>(_=> new ConfigService(rabbitMqServiceHost));
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

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

app.MapHub<ClientsHub>("/Chat");

app.Run();
