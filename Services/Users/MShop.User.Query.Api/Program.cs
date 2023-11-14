using MShop.Infrastructure.EventBus;
using MShop.Infrastructure.Security;
using MShop.Infrastructure.Mongo;
using MShop.User.DataProvider.Repositories;
using MShop.User.DataProvider.Services;
using MShop.User.Query.Api.Handlers;
using MassTransit;
using MShop.Infrastructure.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMongoDb(builder.Configuration);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEncrypter, Encrypter>();
builder.Services.AddScoped<LoginUserHandler>();

var rabbitOptions = new RabbitMqOptions();
builder.Configuration.GetSection("rabbitmq").Bind(rabbitOptions);

// establish connection with rabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LoginUserHandler>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host(new Uri(rabbitOptions.ConnectionString), hostcf =>
        {
            hostcf.Username(rabbitOptions.Username);
            hostcf.Password(rabbitOptions.Password);
        });
        cfg.ConfigureEndpoints(provider);
    }));
});

builder.Services.AddControllers();
builder.Services.AddJwt(builder.Configuration);

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

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

var busControl = app.Services.GetService<IBusControl>();
busControl.Start();

var dbInitializer = app.Services.GetService<IDatabaseInitializer>();
dbInitializer.InitializeAsync();


app.Run();
