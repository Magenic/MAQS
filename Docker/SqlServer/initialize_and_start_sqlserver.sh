#!/bin/bash
{ 
    # Wait 30 seconds for SQL server to start up.
    # Ideally, SQL server would have a hook to run scripts once the database is ready.
    sleep 30s
    echo "Started initializing database"
    # Set up the schema and stored procedures
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P magenicMAQS2 -d master -i `dirname $0`/schema.sql
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P magenicMAQS2 -d master -i `dirname $0`/stored_procedures.sql
    # Use BCP to import test data
    /opt/mssql-tools/bin/bcp MagenicAutomation.dbo.States in "`dirname $0`/SeedData/MagenicAutomation/States.csv" \
        -c -t',' -S localhost -U sa -P magenicMAQS2
    echo "Finished initializing database"
}&

# Start SQL server
exec /opt/mssql/bin/sqlservr