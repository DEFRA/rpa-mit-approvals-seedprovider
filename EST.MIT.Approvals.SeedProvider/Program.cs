using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EST.MIT.Approvals.Data;
using EST.MIT.Approvals.SeedProvider.Provider;
using EST.MIT.Approvals.Api.Authentication;
using Microsoft.Extensions.Options;

var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

var logger = loggerFactory.CreateLogger<Program>();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var isProd = (Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") != "Development");
var interceptor = new AadAuthenticationInterceptor(new TokenGenerator(), config, isProd);
var connStringTask = interceptor.GetConnectionStringAsync();
var connString = connStringTask.GetAwaiter().GetResult();

logger.LogInformation("Connection string: {connString}", connString);

var optionsBuilder = new DbContextOptionsBuilder<ApprovalsContext>();

optionsBuilder.UseLoggerFactory(loggerFactory);

optionsBuilder.UseNpgsql(
        connString,
        x => x.MigrationsAssembly("EST.MIT.Approvals.Data")
    )
    .UseSnakeCaseNamingConvention();

var context = new ApprovalsContext(optionsBuilder.Options);

SeedProvider.SeedReferenceData(context, logger, isProd);