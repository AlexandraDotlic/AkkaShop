using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using OrderingService;
using OrderingService.Actors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IOrderingService, OrderingSvc>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var config = ConfigurationFactory.ParseString(File.ReadAllText("akkaConfig.json"));
using var system = ActorSystem.Create("CartAPI", config);
var cartActorRef = system.ActorOf(Props.Create(() => new OrderingActor()), "orderingActor");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
