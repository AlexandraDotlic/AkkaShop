using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using OrderingService;
using OrderingService.Actors;

var builder = WebApplication.CreateBuilder(args);
var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));
//Create actor system and get cluster
using var system = ActorSystem.Create("OrderingAPI", config);
var cluster = Cluster.Get(system);
List<Address> addresses = new List<Address>();
addresses.Add(cluster.SelfAddress);
cluster.JoinSeedNodes(addresses);
var mediator = DistributedPubSub.Get(system).Mediator;

//Create OrderingActor
var orderingActorRef = system.ActorOf(Props.Create(() => new OrderingActor()), "orderingActor");
//Get productCatalogActor
var productCatalogActorSelection = system.ActorSelection("akka.tcp://ProductCatalogAPI@127.0.0.1:8082/user/productCatalogActor");
var productCatalogActorRef = await productCatalogActorSelection.ResolveOne(TimeSpan.FromSeconds(25));

//Get CartActor
var cartActorSelection = system.ActorSelection("akka.tcp://CartAPI@127.0.0.1:8081/user/cartActor");
var cartActorRef = await cartActorSelection.ResolveOne(TimeSpan.FromSeconds(25));

var cartCoordinatorActorSelection = system.ActorSelection("akka.tcp://CartAPI@127.0.0.1:8081/user/cartCoordinatorActor");
var cartCoordinatorActorRef = await cartActorSelection.ResolveOne(TimeSpan.FromSeconds(25));

//Create orderingCoordinatorActor
var orderingCoordinatorActorRef = system.ActorOf(Props.Create(() => new OrderingCoordinatorActor(productCatalogActorRef, orderingActorRef, cartCoordinatorActorRef)), "orderingCoordinatorActor");


mediator.Tell(new Put(orderingCoordinatorActorRef));
var orderingSvc = new OrderingSvc(orderingActorRef, orderingCoordinatorActorRef);
builder.Services.AddSingleton<IOrderingService>(orderingSvc);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
