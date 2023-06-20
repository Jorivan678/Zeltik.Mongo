namespace Microsoft.Extensions.Options
{
    public class MongoOptions
    {
        public string ConnectionString { get; private set; }

        public string DatabaseName { get; private set; }

        /// <summary>
        /// Establece la cadena de conexión.
        /// </summary>
        /// <param name="configuration"></param>
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
        /// <param name="configuration"></param>
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
