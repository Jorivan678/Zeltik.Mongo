using Zeltik.Mongo;

namespace Microsoft.Extensions.Options
{
    /// <summary>
    /// Una clase básica para establecer la opciones de conexión para
    /// las clases que hereden de <see cref="MongoDbContext"/>.
    /// </summary>
    public class MongoOptions
    {
        /// <summary>
        /// La cadena de conexión.
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// El nombre de la base de datos.
        /// </summary>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// Establece la cadena de conexión.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>La misma instancia de <see cref="MongoOptions"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MongoOptions SetConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            ConnectionString = connectionString;

            return this;
        }

        /// <summary>
        /// Establece el nombre de la base de datos a utilizar desde MongoDB.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns>La misma instancia de <see cref="MongoOptions"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public MongoOptions SetDatabaseName(string databaseName)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentNullException(nameof(databaseName));

            DatabaseName = databaseName;

            return this;
        }
    }
}
