using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Ramz_Elktear.core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Ramz_Elktear.BusinessLayer.Localization
{
    public class DatabaseStringLocalizer : IStringLocalizer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;
        private readonly string _resourceName;
        private const string CACHE_KEY = "LocalizationResources";
        // Static field to track when resources were last reloaded
        private static DateTime _lastReloaded = DateTime.MinValue;
        // Cache expiration time (5 minutes)
        private static readonly TimeSpan _cacheExpirationTime = TimeSpan.FromMinutes(5);

        public DatabaseStringLocalizer(
            IServiceProvider serviceProvider,
            IMemoryCache memoryCache,
            ILogger logger,
            string resourceName)
        {
            _serviceProvider = serviceProvider;
            _memoryCache = memoryCache;
            _logger = logger;
            _resourceName = resourceName;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var resources = GetAllResources(includeParentCultures);

            return resources.Select(r =>
                new LocalizedString(r.ResourceKey, r.Value, false));
        }

        private string GetString(string name)
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name;
            _logger.LogDebug($"Getting localized string '{name}' for culture '{currentCulture}'");

            var resources = GetAllResources(true);

            var resource = resources.FirstOrDefault(r =>
                r.ResourceKey == name && r.CultureCode == currentCulture);

            if (resource == null)
            {
                _logger.LogWarning($"Localized resource not found: {name}, Culture: {currentCulture}");
                // Try to find it in the database again (bypass cache)
                if ((DateTime.UtcNow - _lastReloaded) > _cacheExpirationTime)
                {
                    _logger.LogInformation("Cache expired, reloading resources from database");
                    ReloadResources();
                    resources = GetAllResources(true);
                    resource = resources.FirstOrDefault(r =>
                        r.ResourceKey == name && r.CultureCode == currentCulture);
                }
            }

            return resource?.Value;
        }

        private IEnumerable<LocalizedResourceDto> GetAllResources(bool includeParentCultures)
        {
            if (!_memoryCache.TryGetValue(CACHE_KEY, out List<LocalizedResourceDto> resources))
            {
                _logger.LogInformation("Resources not found in cache, loading from database");
                resources = LoadResourcesFromDatabase();
                _memoryCache.Set(CACHE_KEY, resources, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheExpirationTime
                });
                _lastReloaded = DateTime.UtcNow;
            }

            return resources ?? new List<LocalizedResourceDto>();
        }

        private List<LocalizedResourceDto> LoadResourcesFromDatabase()
        {
            try
            {
                _logger.LogInformation("Loading localization resources from database");

                // Get a scoped service provider
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Get the DbContext from the scoped provider
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var resources = dbContext.LocalizationResources
                        .Include(r => r.Values)
                        .AsNoTracking() // For better performance
                        .ToList();

                    _logger.LogInformation($"Loaded {resources.Count} resource entries from database");

                    return resources.SelectMany(r => r.Values.Select(v => new LocalizedResourceDto
                    {
                        ResourceKey = r.ResourceKey,
                        CultureCode = v.CultureCode,
                        Value = v.Value
                    })).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading localization resources from database");
                return new List<LocalizedResourceDto>();
            }
        }

        public void ReloadResources()
        {
            _logger.LogInformation("Manually reloading localization resources");
            _memoryCache.Remove(CACHE_KEY);
            _lastReloaded = DateTime.UtcNow;
        }
    }

    public class LocalizedResourceDto
    {
        public string ResourceKey { get; set; }
        public string CultureCode { get; set; }
        public string Value { get; set; }
    }
}