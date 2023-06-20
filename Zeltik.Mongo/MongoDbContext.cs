using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Zeltik.Mongo
{
    /// <summary>
    /// Una clase abstracta que simplifica la interacción con el cliente de MongoDB y la base de datos.
    /// </summary>
    public abstract class MongoDbContext
    {
        private IMongoDatabase? _database;
        private MongoClient? _client;
        private readonly object _lock = new();

        /// <summary>
        /// Cliente de MongoDB.
        /// </summary>
        protected MongoClient MongoClient
        {
            get
            {
                EnsureInitialized();
                return _client!;
            }
        }
        /// <summary>
        /// Base de datos utilizada actualmente de MongoDB.
        /// </summary>
        protected IMongoDatabase Database
        {
            get
            {
                EnsureInitialized();
                return _database!;
            }
        }
        private MongoOptions? Options { get; }

        /// <summary>
        /// Constructor: Utilizado normalmente para la inyección de opciones.
        /// </summary>
        /// <param name="options">Las opciones por inyección.</param>
        protected MongoDbContext(IOptions<MongoOptions> options) : this(options.Value) { }

        /// <summary>
        /// Constructor: Utilizado para escribir directamente las opciones o pasar las opciones
        /// desde el constructor de la clase que lo hereda.
        /// </summary>
        /// <param name="options">Las opciones.</param>
        /// <param name="enableAutoInit">Si se mantiene en <see langword="true"/>, se inicializa automáticamente, de lo contrario
        /// utilice <see cref="Initialize"/>.</param>
        protected MongoDbContext(MongoOptions options, bool enableAutoInit = true)
        {
            if (enableAutoInit)
            {
                _client = new MongoClient(options.ConnectionString);
                _database = MongoClient.GetDatabase(options.DatabaseName);
            }
            else
            {
                Options = options;
            }
            OnConfiguring();
        }

        /// <summary>
        /// Inicializa en un contexto externo el cliente y la obtención de la base de datos.
        /// </summary>
        public void Initialize()
        {
            if (_client is not null) return;
            lock (_lock)
            {
                if (_client is not null) return;
                _client = new MongoClient(Options!.ConnectionString);
                _database = MongoClient.GetDatabase(Options.DatabaseName);
            }
        }

        /// <summary>
        /// Comprueba la inicialización del cliente de MongoDB.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        protected void EnsureInitialized()
        {
            if (_client is not null) return;
            lock (_lock)
            {
                if (_client is not null) return;

                throw new InvalidOperationException("El cliente de mongo no fue inicializado.");
            }
        }

        /// <summary>
        /// Obtienes una colección. Se recomienda usarlo como propiedad.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns>La colección específica obtenida de la base de datos.</returns>
        protected IMongoCollection<TEntity> GetCollection<TEntity>(string collectionName) 
            where TEntity : class
        {
            EnsureInitialized();
            return _database!.GetCollection<TEntity>(collectionName);
        }

        /// <summary>
        /// Invoca una sesión del cliente de MongoDB. Esto permite poder hacer múltiples consultas simultáneas,
        /// a la vez que también inicia las transacciones con la base de datos.
        /// Debido a que <see cref="IClientSessionHandle"/> implementa <see cref="IDisposable"/>,
        /// se recomienda el uso de <see langword="using"/> para poder liberar los recursos no administrados.
        /// Esta es la versión asíncrona de <see cref="StartSession(Action{ClientSessionOptions}?)"/>.
        /// </summary>
        /// <param name="options">Opciones para el cliente.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Una instancia que implementa la interfaz <see cref="IClientSessionHandle"/></returns>
        public Task<IClientSessionHandle> StartSessionAsync(Action<ClientSessionOptions>? options = null, CancellationToken cancellationToken = default)
        {
            EnsureInitialized();

            var sessionOptions = new ClientSessionOptions();

            options?.Invoke(sessionOptions);

            return _client!.StartSessionAsync(sessionOptions, cancellationToken);
        }

        /// <summary>
        /// Invoca una sesión del cliente de MongoDB. Esto permite poder hacer múltiples consultas simultáneas,
        /// a la vez que también inicia las transacciones con la base de datos.
        /// Debido a que <see cref="IClientSessionHandle"/> implementa <see cref="IDisposable"/>,
        /// se recomienda el uso de <see langword="using"/> para poder liberar los recursos no administrados. 
        /// Esta es la versión síncrona de <see cref="StartSessionAsync(Action{ClientSessionOptions}?, CancellationToken)"/>.
        /// </summary>
        /// <param name="options">Opciones para el cliente.</param>
        /// <returns>Una instancia que implementa la interfaz <see cref="IClientSessionHandle"/></returns>
        public IClientSessionHandle StartSession(Action<ClientSessionOptions>? options = null)
        {
            EnsureInitialized();

            var sessionOptions = new ClientSessionOptions();

            options?.Invoke(sessionOptions);

            return _client!.StartSession(sessionOptions);
        }

        /// <summary>
        /// Este método puede ser sobreescrito para poder utilizar los mapeos de entidades
        /// de MongoDB, utilizando el método <see cref="BsonClassMap.RegisterClassMap{TClass}()"/>
        /// perteneciente a la clase <see cref="BsonClassMap"/>.
        /// </summary>
        protected virtual void OnConfiguring()
        {
        }
    }
}
