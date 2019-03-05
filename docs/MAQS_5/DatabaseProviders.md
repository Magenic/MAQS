# <img src="resources/maqslogo.ico" height="32" width="32"> Database Providers

## Supported DataBase Providers
***Dapper*** does not provide a specific database provider implementation, instead using what is provided to it via extension methods. By default, MAQS provides implementation that supports the following providers:

* ***SQL Server***
* ***PostgreSql***
* ***SQLite***


## The IProvider Interface
An IProvider interface defines a ***connection*** type and database connection setup method. DatabaseConfig provides a convenient GetOpenConnection that takes an IProvider.

```
    /// <summary>
    /// The test provider class for testing
    /// </summary>
    public class TestProvider : IProvider<SqlConnection>
    {
        /// <summary>
        /// Method used to setup a SQL connection client
        /// </summary>
        /// <param name="connectionString"> The connection string. </param>
        /// <returns> The <see cref="SqlConnection"/> connection client. </returns>
        public SqlConnection SetupDataBaseConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = connectionString
            };

            return connection;
        }
    } 

```

### Define Your Own IProvider
After implementing the IProvider interface, use the connection as you normally would with Dapper:

```
    var connection = DatabaseConfig.GetOpenConnection(new TestProvider());
    var table = connection.Query("SELECT * FROM SOMETABLE");
```