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

var interceptor = new AadAuthenticationInterceptor(new TokenGenerator(), config, true);
var connStringTask = interceptor.GetConnectionStringAsync();
var connString = connStringTask.GetAwaiter().GetResult();

logger.LogInformation("Connection string: {connString}", connString);

//options
//    .UseNpgsql(
//        connString,
//        x => x.MigrationsAssembly("EST.MIT.Approvals.Data")
//    )
//    .UseSnakeCaseNamingConvention();


//builder.Services.AddDbContext<ApprovalsContext>(options =>
//{
//    var connStringTask = interceptor.GetConnectionStringAsync();
//    var connString = connStringTask.GetAwaiter().GetResult();

//    options.AddInterceptors(interceptor);

//    options
//        .UseNpgsql(
//            connString,
//            x => x.MigrationsAssembly("EST.MIT.Approvals.Data")
//        )
//        .UseSnakeCaseNamingConvention();
//});


//var host = config["POSTGRES_HOST"];
//var db = config["POSTGRES_DB"];
//var port = config["POSTGRES_PORT"];
//var user = config["POSTGRES_USER"];
//var pass = config["POSTGRES_PASSWORD"];

//var postgres = string.Format(config["DbConnectionTemplate"]!, host, port, db, user, pass);

//logger.LogInformation("DbConnectionTemplate: {postgres}", postgres);

var optionsBuilder = new DbContextOptionsBuilder<ApprovalsContext>();

optionsBuilder.UseLoggerFactory(loggerFactory);

optionsBuilder.UseNpgsql(
        connString,
        x => x.MigrationsAssembly("EST.MIT.Approvals.Data")
    )
    .UseSnakeCaseNamingConvention();

var context = new ApprovalsContext(optionsBuilder.Options);

SeedProvider.SeedReferenceData(context, logger);