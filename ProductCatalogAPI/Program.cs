using Akka.Actor;
using Akka.Cluster;
using Akka.Configuration;
using ProductCatalogService;
using ProductCatalogService.Actors;

var builder = WebApplication.CreateBuilder(args);
var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.conf"));

var system = ActorSystem.Create("ProductCatalogAPI", config);
var cluster =  Cluster.Get(system);
List<Address> addresses = new List<Address>();
addresses.Add(cluster.SelfAddress);
cluster.JoinSeedNodes(addresses);

var productCatalogActorRef = system.ActorOf(Props.Create(() => new ProductCatalogActor()), "productCatalogActor");

// Add services to the container.
var productCatalogSvc = new ProductCatalogSvc(productCatalogActorRef);
builder.Services.AddSingleton<IProductCatalogService>(productCatalogSvc);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//system.WhenTerminated.Wait();

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
