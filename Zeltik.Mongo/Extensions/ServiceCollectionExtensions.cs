using Microsoft.Extensions.Options;
using Zeltik.Mongo;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Añade el contexto de MongoDB a la inyección de dependencias y opcionalmente, las opciones
        /// que este puede utilizar.
        /// </summary>
        /// <typeparam name="TContext">El contexto de MongoDB a utilizar.</typeparam>
        /// <param name="services"></param>
        /// <param name="conf">El delegado de configuración.</param>
        /// <param name="instanceName">Nombre para la instancia de <see cref="MongoOptions"/>.</param>
        /// <returns>Una referencia a esta misma instancia.</returns>
        public static IServiceCollection AddMongoContext<TContext>(this IServiceCollection services, Action<MongoOptions>? conf = null, string? instanceName = null) where TContext : MongoDbContext
        {
            if (conf is not null) services.ConfigureMongo(conf, instanceName);

            services.AddSingleton(typeof(TContext));

            return services;
        }

        private static void ConfigureMongo(this IServiceCollection services, Action<MongoOptions> configure, string? instanceName)
        {
            if (!string.IsNullOrWhiteSpace(instanceName))
                services.AddOptions<MongoOptions>(instanceName).Configure(configure);
            else
                services.AddOptions<MongoOptions>().Configure(configure);
        }
    }
}
