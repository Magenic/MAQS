# How to run Unit Tests locally

## Requirements

1. [Visual Studio](https://visualstudio.microsoft.com/downloads/)
2. [Docker](https://www.docker.com/)

## Docker containers

In the MAQS repository, we have 3 docker images to be able to test Email, MongoDB, and SQL server.

1. Then traverse to the docker directory and run `docker-compose up -d`
2. This will setup all the instances along with generating data
3. Note: There is a setup issue with SQL Server where if a user doesn't destroy the SQLServer instance, it will populate the data again causing duplicated
4. Once complete with work, call `docker-compose down`
