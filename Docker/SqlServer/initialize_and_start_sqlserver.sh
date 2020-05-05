#!/bin/bash
{ 
    # Wait for SQL Server to start up
    # Check if the server is ready
    not_ready=1
    while [ $not_ready != 0 ]
    do
        # Wait for the return code of the following statement to be zero
        /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P magenicMAQS2 -d master -Q "SELECT TOP 1 message_id FROM sys.messages"
        not_ready=$?

        if [ $not_ready != 0 ]
        then
            echo "Could not contact sql server, will try again in 5 seconds."
            sleep 5s
        fi
    done

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