using EventsAPI.Models;
using JwtAuthenticationManager;
using MongoRepository;
using RabbitMqExtension;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMongo()
    .AddMongoRepository<Device, Guid>("devices")
    .AddMongoRepository<Event, Guid>("events")
    .AddMassTransitRabbitMq();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMvc(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();