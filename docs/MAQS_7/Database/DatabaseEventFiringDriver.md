# <img src="resources/maqslogo.ico" height="32" width="32">Database Firing Manager

## Overview
Wrap basic firing database interactions

[Execute](#Execute)  
[Query](#Query)  
[Insert](#Insert)  
[Delete](#Delete)  
[Update](#Update)  
[Dispose](#Dispose)  
[OnEvent](#OnEvent)  
[OnErrorEvent](#OnErrorEvent)  
[RaiseEvent](#RaiseEvent)  
[RaiseErrorMessage](#RaiseErrorMessage)  

## Execute
Execute parameterized SQL.
```csharp
 public override int Execute(
    string sql,
    object param = null,
    IDbTransaction transaction = null,
    int? commandTimeout = null,
    CommandType? commandType = null)
{
    try
    {
        this.RaiseEvent("execute", sql);
        return base.Execute(sql, param, transaction, commandTimeout, commandType);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## Query
Submits a query to the database
```csharp
public override IEnumerable<dynamic> Query(
    string sql,
    object param = null,
    IDbTransaction transaction = null,
    bool buffered = true,
    int? commandTimeout = null,
    CommandType? commandType = null)
{
    try
    {
        this.RaiseEvent("query", sql);
        return base.Query(sql, param, transaction, buffered, commandTimeout, commandType);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## Insert
Inserts an entity into table "T" and returns identity id or number of inserted rows if inserting a list.
```csharp
public override long Insert<T>(
    T entityToInsert,
    IDbTransaction transaction = null,
    int? commandTimeout = null,
    params string[] items)
{
    try
    {
        this.RaiseEvent("insert", items);
        return base.Insert<T>(entityToInsert, transaction, commandTimeout);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## Delete
Delete entity in table "T".
```csharp
 T entityToDelete,
    IDbTransaction transaction = null,
    int? commandTimeout = null,
    params string[] items)
{
    try
    {
        this.RaiseEvent("delete", items);
        return base.Delete<T>(entityToDelete, transaction, commandTimeout);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
```

## Update
Updates entity in table "T", checks if the entity is modified if the entity is tracked by the Get() extension.
```csharp
public override bool Update<T>(
    T entityToUpdate,
    IDbTransaction transaction = null,
    int? commandTimeout = null,
    params string[] items)
{
    try
    {
        this.RaiseEvent("update", items);
        return base.Update<T>(entityToUpdate, transaction, commandTimeout);
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## Dispose
Dispose of the database connection
```csharp
protected override void Dispose(bool disposing)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Releasing connection"));
        base.Dispose(disposing);
        this.OnEvent(StringProcessor.SafeFormatter("Released connection"));
    }
    catch (Exception ex)
    {
        this.RaiseErrorMessage(ex);
        throw ex;
    }
}
```

## OnEvent
Database event logging
```csharp
protected virtual void OnEvent(string message)
{
    this.DatabaseEvent?.Invoke(this, message);
}
```

## OnErrorEvent
Database error event logging
```csharp
protected virtual void OnErrorEvent(string message)
{
    this.DatabaseErrorEvent?.Invoke(this, message);
}
```

## RaiseEvent
Raise an event message
```csharp
private void RaiseEvent(string actionType, string query)
{
    try
    {
        this.OnEvent(StringProcessor.SafeFormatter("Performing {0} with:\r\n{1}", actionType, query));
    }
    catch (Exception e)
    {
        this.OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
    }
}

private void RaiseEvent(string actionType, params string[] items)
{
    try
    {
        StringBuilder builder = new StringBuilder();

        foreach (var item in items)
        {
            builder.AppendLine(item);
        }
                
        this.OnEvent(StringProcessor.SafeFormatter("Performing {0} with:\r\n{1}", actionType, builder.ToString()));
    }
    catch (Exception e)
    {
        this.OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
    }
}
```

## RaiseErrorMessage
Raise an exception message
```csharp
private void RaiseErrorMessage(Exception e)
{
    this.OnErrorEvent(StringProcessor.SafeFormatter("Failed because: {0}{1}{2}", e.Message, Environment.NewLine, e.ToString()));
}
```