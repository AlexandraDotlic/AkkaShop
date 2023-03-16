using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using CartCoordinatorService;
using CartCoordinatorService.Actors;
using CartService;
using CartService.Actors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICartService, CartSvc>();
builder.Services.AddSingleton<ICartCoordinatorService, CartCoordinatorSvc>();

var app = builder.Build();
var config = ConfigurationFactory.ParseString(File.ReadAllText("akkaConfig.json"));
using var system = ActorSystem.Create("CartAPI", config);
//mozda ovo nije potrebno
var mediator = DistributedPubSub.Get(system).Mediator;
var cartActorRef = system.ActorOf(Props.Create(() => new CartActor()), "cartActor");
//This code creates an instance of the ProductCatalogActor using an
//ActorSelection to locate the actor running on another node in the cluster
var productCatalogActorSelection = system.ActorSelection("akka.tcp://ProductCatalogCluster@localhost:2551/user/productCatalogActor");
var productCatalogActorRef = await productCatalogActorSelection.ResolveOne(TimeSpan.FromSeconds(5));
var cartCoordinatorActorRef = system.ActorOf(Props.Create(() => new CartCoordinatorActor(cartActorRef, productCatalogActorRef)), "cartCoordinatorActor");

//ovo isto ne znam da li treba?
mediator.Tell(new Put(cartCoordinatorActorRef));

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
