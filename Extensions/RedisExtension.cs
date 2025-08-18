using StackExchange.Redis;

namespace emailAPI.Extensions
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration config)
        {
            var redisHost = config["REDIS_HOST"]
                ?? throw new InvalidOperationException("REDIS_HOST is not set in environment variables.");
            var redisPort = config["REDIS_PORT"]
                ?? throw new InvalidOperationException("REDIS_PORT is not set in environment variables.");
            var redisPassword = config["REDIS_PASSWORD"]
                ?? throw new InvalidOperationException("REDIS_PASSWORD is not set in environment variables.");

            var options = new ConfigurationOptions
            {
                EndPoints = { $"{redisHost}:{redisPort}" },
                Password = redisPassword,
                AbortOnConnectFail = false,
            };

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options));

            return services;
        }
    }
}