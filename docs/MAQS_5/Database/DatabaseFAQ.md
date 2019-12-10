# <img src="resources/maqslogo.ico" height="32" width="32"> Database FAQ

## What about DataTables
***MAQS and Dapper*** shifts away with DataTables and instead leveages a strategy around strongly typed and dynamic object lists. Migrating from MAQS 4 to 5 requires either a rewrite of queries to support the mapping strategy, or a conversion of the returned objects into a DataTable.

## How do use a provider besides SQL, SQLite and PostgreSql
There are multiple ways to use a custom provider. 

* [Override the GetDataBaseConnection method](MAQS_5/DatabaseBaseTest.md)
* [Create your own DatabaseDriver](MAQS_5/DatabaseDriver.md)
* [Implement the IProvider Class](MAQS_5/DatabaseProviders.md)