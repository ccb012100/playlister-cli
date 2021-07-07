using System;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace PlaylisterCli
{
    public class DbService : IDbService
    {
        private readonly ILogger<DbService> _logger;

        public DbService(ILogger<DbService> logger)
        {
            _logger = logger;
        }

        public string GetDbInfo()
        {
            _logger.LogInformation("Logging from DbService");

            return DateTime.Now.ToString(CultureInfo.CurrentCulture);
        }
    }
}
