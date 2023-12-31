using Azure.Core;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

#pragma warning disable 1591

namespace EST.MIT.Approvals.Api.Authentication;

public class AadAuthenticationInterceptor : DbConnectionInterceptor
{
    private readonly IConfigurationRoot _configuration;
    private readonly bool _isProd;
    private readonly ITokenGenerator _tokenService;

    public AadAuthenticationInterceptor(ITokenGenerator tokenService, IConfigurationRoot configuration, bool isProd)
    {
        _configuration = configuration;
        _isProd = isProd;
        _tokenService = tokenService;
    }

    public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        // handles connectionString init for Non-async db calls, such as for InvoiceTypeParameterFilter used in AddSwaggerServices
        if (!_isProd) {
            connection.ConnectionString = GetConnectionString();
        }
        return base.ConnectionOpening(connection, eventData, result);
    }

    public string GetConnectionString()
    {
        var host = _configuration["POSTGRES_HOST"];
        var port = _configuration["POSTGRES_PORT"];
        var db = _configuration["POSTGRES_DB"];
        var user = _configuration["POSTGRES_USER"];
        var pass = _configuration["POSTGRES_PASSWORD"];
        return string.Format(_configuration["DbConnectionTemplate"]!, host, port, db, user, pass);
    }

    public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(
        DbConnection connection,
        ConnectionEventData eventData,
        InterceptionResult result,
        CancellationToken cancellationToken = default)
    {
        connection.ConnectionString = await GetConnectionStringAsync(cancellationToken);

        return result;
    }

    public async Task<string> GetConnectionStringAsync(CancellationToken cancellationToken = default)
    {
        var host = _configuration["POSTGRES_HOST"];
        var port = _configuration["POSTGRES_PORT"];
        var db = _configuration["POSTGRES_DB"];
        var user = _configuration["POSTGRES_USER"];
        var pass = _configuration["POSTGRES_PASSWORD"];
        var postgresSqlAAD = _configuration["AzureADPostgreSQLResourceID"];

        if (_isProd)
        {
            if ((!TokenCache.AccessToken.HasValue) || (DateTime.Now >= TokenCache.AccessToken.Value.ExpiresOn.AddMinutes(-1)))
            {
                TokenCache.AccessToken = await _tokenService.GetTokenAsync(postgresSqlAAD!, cancellationToken);
            }

            pass = TokenCache.AccessToken.Value.Token;
        }

        return string.Format(_configuration["DbConnectionTemplate"]!, host, port, db, user, pass);
   }

    public bool IsLocalDatabase()
    {
        var host = _configuration["POSTGRES_HOST"];
        return string.IsNullOrEmpty(host) || host.ToLower() == "localhost" || host == "127.0.0.1";
    }
}