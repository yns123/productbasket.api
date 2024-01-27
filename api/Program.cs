using System.Reflection;
using data.respositories;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

//* API Versioning
services.AddApiVersioning((options) =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});
services.AddRouting(options => options.LowercaseUrls = true);

// Add services to the container.

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddHttpContextAccessor();

ConfigureContainer(services, configuration);
AddConfiguration(services, configuration);

//TODO LOGGING ayarları
//TODO LOCALIZATION ayarları
//TODO AUTHORIZATION ayarları (IdentityServer/JWT/OAuth)
//TODO CONTEXT ayarları
//TODO RATELIMIT ayarları
//TODO HEALTHCHECK ayarları


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseStaticFiles()
    .UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "source/")),
        RequestPath = "/source"
    });

app.UseExceptionHandler("/error");

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapSwagger();

app.Run();


IServiceCollection ConfigureContainer(IServiceCollection services, IConfiguration configuration)
{
    services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));
    services.AddTransient<IRedisService, RedisService>();

    /*-----Services-----*/
    services.AddScoped<IBasketService, BasketService>();
    services.AddScoped<IProductService, ProductService>();

    return services;
}

void AddConfiguration(IServiceCollection serviceCollection, IConfiguration configuration)
{
    serviceCollection.Configure<MongoSettings>(configuration.GetSection("Mongo"));
    serviceCollection.Configure<RedisCacheSettings>(configuration.GetSection("Redis"));
}