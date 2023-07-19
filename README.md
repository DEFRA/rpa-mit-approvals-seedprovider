# EST.MIT.Approvals.SeedProvider
A minimal api for supplying invoice template reference data (.NET 6)

## Running Application
### Requirements
* Git
* .NET 6 SDK
* PostgreSQL
* Access to the DEFRA-EST ADO Artifact Feed
* **Optional:** Docker - Only needed if running PostgreSQL within container

### Environment Variables
The following environment variables are required by the application.

| Name              	| Description                         	| Default                         	|
|-------------------	|-------------------------------------	|---------------------------------	|
| POSTGRES_HOST     	| Hostname of the Postgres server     	| 127.0.0.1 	                  |
| POSTGRES_DB       	| Name of the reference data database 	| est-mit-approvals           	|
| POSTGRES_USER     	| Postgres username                   	| postgres                        	|
| POSTGRES_PASSWORD 	| Postgres password                   	| pass@word1                       	|
| POSTGRES_PORT     	| Postgres server port                	| 5432                            	|
| SCHEMA_DEFAULT    	| Default schema name                 	| public                          	|

When running using Docker / Docker Compose these values are populated from environment variables.

If running locally using `dotnet run` the values are populated from dotnet user-secrets (or system level environment variables). Please see [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows)

### Add Private Package Feed
This project uses a private NuGet package to store seed data.

Follow this guide to add the private feed to Visual Studio:
[Install NuGet packages with Visual Studio](https://learn.microsoft.com/en-us/azure/devops/artifacts/nuget/consume?view=azure-devops&tabs=windows)

### Seeding Reference Data
**Important**: The seed ref data provider will reset the connected database to reference data defaults.

#### Postgres within Docker container
Get the latest postgres docker image

```ps
docker pull postgres
```

Then spin up a container
```ps
docker run --name approvals-postgres -p 5432:5432 -e POSTGRES_PASSWORD=pass@word1 -d postgres
```
Use the values in the above steps and set the relevant env vars.

#### Postgres running locally
Use the values specified when Postgres was locally installed and set the relevant env vars.

The seed provider uses dotnet user secrets to store Postgres connection parameters.
```cs
cd EST.MIT.Approvals.SeedProvider
dotnet run
```

This application dynamically seeds reference data into the database based on Json files from the `EST.MIT.Approvals.SeedData` package.