# <img src="resources/maqslogo.ico" height="32" width="32"> MAQS Dapper

## Dapper ORM
***Dapper*** is a simple, fast, open source object-relational mapping (ORM) framework. Dapper allows tests to map the database queries results into defeind or dynamic objects. See the Dapper github page for more information and usage guides: [Dapper GitHub](https://github.com/StackExchange/Dapper).


## MAQS with Dapper
MAQS integrates seemlessly with Dapper by implementing some default [MAQS Providers](MAQS_5/DatabaseProviders.md) and [MAQS Database Configuration Keys](MAQS_5/DatabaseSettings.md). 

To execute a query and map the results into a list of Orders objects:
```
 var orders = this.DatabaseDriver.Query<Orders>("select * from orders").ToList();
```

*The above works if using a default provider and the BaseDatabaseTest class.