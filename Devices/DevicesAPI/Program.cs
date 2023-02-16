using DevicesAPI.Models;
using JwtAuthenticationManager;
using MongoRepository;
using Newtonsoft.Json.Serialization;
using RabbitMqExtension;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMongo()
    .AddMongoRepository<Device, Guid>("devices")
    .AddMassTransitRabbitMq();

builder.Services.AddControllers().AddNewtonsoftJson(s =>
{
    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMvc(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddCustomJwtAuthentication();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();
