using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using FiapCloudGames.Games.Domain.Repositories;
using FiapCloudGames.Games.Infrastructure.Persistence;
using FiapCloudGames.Games.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Games.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext()
            .AddRepositories()
            .AddUnitOfWork()
            .AddElasticSearch(configuration);

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        string connectionString = Environment.GetEnvironmentVariable("FiapCloudGamesGamesConnectionString")!;

        services.AddDbContext<FiapCloudGamesGamesDbContext>(options => options.UseSqlServer(connectionString));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IGameGenreRepository, GameGenreRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        string apiKeyString = configuration.GetSection("ElasticSearch")!.GetValue<string>("ApiKey")!;
        string uriString = configuration.GetSection("ElasticSearch")!.GetValue<string>("Uri")!;

        ElasticsearchClientSettings elasticsearchClientSettings = new ElasticsearchClientSettings(new Uri(uriString)).Authentication(new ApiKey(apiKeyString));
        ElasticsearchClient elasticsearchClient = new(elasticsearchClientSettings);

        services.AddSingleton(elasticsearchClient);

        return services;
    }
}
