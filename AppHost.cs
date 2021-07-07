using System;
using Microsoft.Extensions.Logging;

namespace PlaylisterCli
{
    public class AppHost
    {
        private readonly ILogger<AppHost> _logger;
        private readonly IDbService _dbService;

        public AppHost(ILogger<AppHost> logger, IDbService dbService)
        {
            _logger = logger;
            _dbService = dbService;
        }

        public void Run()
        {
            _logger.LogInformation("Running AppHost");
            _dbService.GetDbInfo();
        }
    }
}
