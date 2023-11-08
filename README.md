# EST.MIT.Approvals.SeedProvider
A minimal api for supplying information about approvals (.NET 6)

## Running Application
### Requirements
* Git
* .NET 6 SDK
* PostgreSQL
* A PAT token with access to the DEFRA-EST ADO Artifact Feed. 
* **Optional:** Docker - Only needed if running PostgreSQL within container

### Environment Variables
The following environment variables need to be set in the AppSettings.json whilst debugging.

| Name              	| Description                         	| Default                         	|
|-------------------	|-------------------------------------	|---------------------------------	|
| POSTGRES_HOST     	| Hostname of the Postgres server     	| <postgres_server> 	            |
| POSTGRES_DB       	| Name of the reference data database 	| rpa_mit_aprovals               	|
| POSTGRES_USER     	| Postgres username                   	| postgres                        	|
| POSTGRES_PASSWORD 	| Postgres password                   	| password                       	|
| POSTGRES_PORT     	| Postgres server port                	| 5432                            	|

When running using Docker / Docker Compose these values are populated from environment variables defined in the .env file. You will need to set PACKAGE_FEED_USERNAME and PACKAGE_FEED_PAT.

If running locally using `dotnet run` the values are populated from dotnet user-secrets (or system level environment variables). Please see [Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows)

### Add Private Package Feed
This project uses a private NuGet package to store seed data.

Follow this guide to add the private feed to Visual Studio:
[Install NuGet packages with Visual Studio](https://learn.microsoft.com/en-us/azure/devops/artifacts/nuget/consume?view=azure-devops&tabs=windows)

### Seeding Reference Data
**Important**: The seed ref data provider will reset the connected database to reference data defaults.

#### Running within a Docker container
Set the PACKAGE_FEED_USERNAME and PACKAGE_FEED_PAT in the .env file. NOTE: ensure these credentials aren't in your commits.

Then run:
```
docker compose up --build
```

In a web browser go to: http://localhost:5050/browser/

Register Server
- Host name: host.docker.internal.
- Username: postgres
- Password: password

You should see the rpa_mit_approvals database and tables.

#### Postgres running locally
Use the values specified when Postgres was locally installed and set the relevant env vars.

The seed provider uses dotnet user secrets to store Postgres connection parameters.
```cs
cd EST.MIT.Approvals.SeedProvider
dotnet run
```

This application dynamically seeds reference data into the database based on Json files from the `EST.MIT.Approvals.SeedData` package.