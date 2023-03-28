
using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using CartService;
using CartService.Actors;

var builder = WebApplication.CreateBuilder(args);
var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));
var system = ActorSystem.Create("CartAPI", config);
Console.WriteLine($"Akka.NET TCP port: {system.Settings.Config.GetInt("akka.remote.dot-netty.tcp.port")}");

var cluster = Cluster.Get(system);
List<Address> addresses = new List<Address>();
addresses.Add(cluster.SelfAddress);
cluster.JoinSeedNodes(addresses);
var mediator = DistributedPubSub.Get(system).Mediator;
var cartActorRef = system.ActorOf(Props.Create(() => new CartActor()), "cartActor");
//ActorSelection to locate the actor running on another node in the cluster
var productCatalogActorSelection = system.ActorSelection("akka.tcp://ProductCatalogAPI@127.0.0.1:8082/user/productCatalogActor");
var productCatalogActorRef = await productCatalogActorSelection.ResolveOne(TimeSpan.FromSeconds(25));

var cartCoordinatorActorRef = system.ActorOf(Props.Create(() => new CartCoordinatorActor(cartActorRef, productCatalogActorRef)), "cartCoordinatorActor");

//ovo isto ne znam da li treba?
mediator.Tell(new Put(cartCoordinatorActorRef));


var cartSvc = new CartSvc(cartActorRef, cartCoordinatorActorRef);
builder.Services.AddSingleton<ICartService>(cartSvc);


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
