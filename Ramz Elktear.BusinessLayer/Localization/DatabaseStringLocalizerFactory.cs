using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Ramz_Elktear.BusinessLayer.Localization
{
    public class DatabaseStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly ILoggerFactory _loggerFactory;

        public DatabaseStringLocalizerFactory(
            IServiceProvider serviceProvider,
            IMemoryCache memoryCache,
            ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
            _loggerFactory = loggerFactory;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new DatabaseStringLocalizer(
                _serviceProvider,
                _memoryCache,
                _loggerFactory.CreateLogger<DatabaseStringLocalizer>(),
                resourceSource.FullName);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new DatabaseStringLocalizer(
                _serviceProvider,
                _memoryCache,
                _loggerFactory.CreateLogger<DatabaseStringLocalizer>(),
                baseName);
        }
    }
}