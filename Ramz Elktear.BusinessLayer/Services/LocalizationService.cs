using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.BusinessLayer.Localization;
using Ramz_Elktear.core.DTO.LocalizationModel;
using Ramz_Elktear.core.Entities.Localization;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System.Text;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly DatabaseStringLocalizerFactory _dbLocalizerFactory;

        public LocalizationService(
            IUnitOfWork unitOfWork,
            IMemoryCache memoryCache,
            IStringLocalizerFactory dbLocalizerFactory)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _dbLocalizerFactory = dbLocalizerFactory as DatabaseStringLocalizerFactory;
        }

        public async Task<LocalizationManagementViewModel> GetLocalizationResourcesAsync(
            string searchString, string group, int page = 1, int pageSize = 20)
        {
            // Get all resource groups for filtering
            var resources = _unitOfWork.LocalizationResourceRepository.FindAll(
                criteria: r => true,
                include: query => query.Include(r => r.Values)
            );

            var groups = resources
                .Select(r => r.ResourceGroup)
                .Distinct()
                .OrderBy(g => g)
                .ToList();

            // Apply filtering
            if (!string.IsNullOrEmpty(searchString))
            {
                resources = resources.Where(r =>
                    r.ResourceKey.Contains(searchString) ||
                    r.Description.Contains(searchString) ||
                    r.Values.Any(v => v.Value.Contains(searchString)));
            }

            if (!string.IsNullOrEmpty(group))
            {
                resources = resources.Where(r => r.ResourceGroup == group);
            }

            // Apply ordering
            resources = resources
                .OrderBy(r => r.ResourceGroup)
                .ThenBy(r => r.ResourceKey);

            // Calculate pagination
            var totalItems = resources.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            // Apply pagination
            var paginatedResources = resources
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Create view model
            var viewModel = new LocalizationManagementViewModel
            {
                Resources = paginatedResources,
                SearchString = searchString,
                CurrentGroup = group,
                CurrentPage = page,
                TotalPages = totalPages,
                Groups = groups,
                CreateModel = new LocalizationViewModel
                {
                    Resource = new LocalizationResourceDto(),
                    Values = new List<LocalizationValueViewModel>
                    {
                        new() { CultureCode = "en", Value = "" },
                        new() { CultureCode = "ar", Value = "" }
                    }
                }
            };

            return viewModel;
        }

        public async Task<LocalizationViewModel> GetLocalizationResourceByIdAsync(int id)
        {
            var resource = _unitOfWork.LocalizationResourceRepository.Find(
                r => r.Id == id,
                include: query => query.Include(r => r.Values)
            );

            if (resource == null)
            {
                return null;
            }

            var model = MapToViewModel(resource);

            // Ensure we have entries for all supported cultures
            var supportedCultures = new[] { "en", "ar" };
            foreach (var culture in supportedCultures)
            {
                if (!model.Values.Any(v => v.CultureCode == culture))
                {
                    model.Values.Add(new LocalizationValueViewModel
                    {
                        CultureCode = culture,
                        Value = ""
                    });
                }
            }

            return model;
        }

        public async Task<bool> CreateLocalizationResourceAsync(LocalizationViewModel model, string userId)
        {
            // Check if the key already exists
            var existingResource = _unitOfWork.LocalizationResourceRepository.Find(
                r => r.ResourceKey == model.Resource.ResourceKey
            );

            if (existingResource != null)
            {
                return false;
            }

            // Create the resource entity from DTO
            var resource = new LocalizationResource
            {
                ResourceKey = model.Resource.ResourceKey,
                ResourceGroup = model.Resource.ResourceGroup,
                Description = model.Resource.Description,
                CreatedAt = DateTime.UtcNow,
                Values = new List<LocalizationValue>()
            };

            // Add values for each culture
            foreach (var valueModel in model.Values)
            {
                resource.Values.Add(new LocalizationValue
                {
                    CultureCode = valueModel.CultureCode,
                    Value = valueModel.Value,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _unitOfWork.LocalizationResourceRepository.Add(resource);
            var result = await _unitOfWork.SaveChangesAsync();

            // Refresh the localization cache if successful
            if (result > 0)
            {
                RefreshLocalizationCache();
            }

            return result > 0;
        }

        public async Task<bool> UpdateLocalizationResourceAsync(LocalizationViewModel model, string userId)
        {
            try
            {
                var resource = _unitOfWork.LocalizationResourceRepository.Find(
                    r => r.Id == model.Resource.Id,
                    include: query => query.Include(r => r.Values)
                );

                if (resource == null)
                {
                    return false;
                }

                // Update resource properties
                resource.ResourceKey = model.Resource.ResourceKey;
                resource.ResourceGroup = model.Resource.ResourceGroup;
                resource.Description = model.Resource.Description;
                resource.UpdatedAt = DateTime.UtcNow;

                // Update existing values and add new ones
                foreach (var valueModel in model.Values)
                {
                    var existingValue = resource.Values
                        .FirstOrDefault(v => v.CultureCode == valueModel.CultureCode);

                    if (existingValue != null)
                    {
                        // Log change if value has changed
                        if (existingValue.Value != valueModel.Value)
                        {
                            _unitOfWork.LocalizationChangeLogRepository.Add(new LocalizationChangeLog
                            {
                                ResourceId = resource.Id,
                                CultureCode = existingValue.CultureCode,
                                OldValue = existingValue.Value,
                                NewValue = valueModel.Value,
                                UserId = userId,
                                ChangedAt = DateTime.UtcNow
                            });

                            // Update value
                            existingValue.Value = valueModel.Value;
                            existingValue.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        // Add new value
                        resource.Values.Add(new LocalizationValue
                        {
                            ResourceId = resource.Id,
                            CultureCode = valueModel.CultureCode,
                            Value = valueModel.Value,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                _unitOfWork.LocalizationResourceRepository.Update(resource);
                var result = await _unitOfWork.SaveChangesAsync();

                // Refresh the localization cache if successful
                if (result > 0)
                {
                    RefreshLocalizationCache();
                }

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteLocalizationResourceAsync(int id)
        {
            var resource = _unitOfWork.LocalizationResourceRepository.Find(
                r => r.Id == id,
                include: query => query.Include(r => r.Values)
            );

            if (resource == null)
            {
                return false;
            }

            _unitOfWork.LocalizationResourceRepository.Delete(resource);
            var result = await _unitOfWork.SaveChangesAsync();

            // Refresh the localization cache if successful
            if (result > 0)
            {
                RefreshLocalizationCache();
            }

            return result > 0;
        }

        public void RefreshLocalizationCache()
        {
            // Remove localization cache
            _memoryCache.Remove("LocalizationResources");

            // Force reload for the database localizer
            if (_dbLocalizerFactory != null)
            {
                var localizer = _dbLocalizerFactory.Create(typeof(LocalizationService));
                if (localizer is DatabaseStringLocalizer dbLocalizer)
                {
                    dbLocalizer.ReloadResources();
                }
            }
        }

        public async Task<bool> ImportJsonDataAsync(string jsonContent, string userId)
        {
            try
            {
                // Parse the JSON data
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var importedResources = System.Text.Json.JsonSerializer.Deserialize<List<LocalizationResourceDto>>(jsonContent, options);

                if (importedResources == null || !importedResources.Any())
                {
                    return false;
                }

                foreach (var resourceDto in importedResources
                    .Where(r => !string.IsNullOrEmpty(r.ResourceKey))
                    .DistinctBy(r => r.ResourceKey))
                {
                    // Skip if resource doesn't have a key
                    if (string.IsNullOrEmpty(resourceDto.ResourceKey))
                    {
                        continue;
                    }

                    // Check if resource exists
                    var existingResource = _unitOfWork.LocalizationResourceRepository.Find(
                        r => r.ResourceKey == resourceDto.ResourceKey,
                        include: query => query.Include(r => r.Values)
                    );

                    if (existingResource == null)
                    {
                        // Create new resource
                        var newResource = new LocalizationResource
                        {
                            ResourceKey = resourceDto.ResourceKey,
                            ResourceGroup = resourceDto.ResourceGroup,
                            Description = resourceDto.Description,
                            CreatedAt = DateTime.UtcNow,
                            Values = new List<LocalizationValue>()
                        };

                        // Add values
                        if (resourceDto.Values != null)
                        {
                            foreach (var valueDto in resourceDto.Values)
                            {
                                newResource.Values.Add(new LocalizationValue
                                {
                                    CultureCode = valueDto.CultureCode,
                                    Value = valueDto.Value,
                                    CreatedAt = DateTime.UtcNow
                                });
                            }
                        }

                        _unitOfWork.LocalizationResourceRepository.Add(newResource);
                    }
                    else
                    {
                        // Update existing resource
                        existingResource.ResourceGroup = resourceDto.ResourceGroup;
                        existingResource.Description = resourceDto.Description;
                        existingResource.UpdatedAt = DateTime.UtcNow;

                        // Update values
                        if (resourceDto.Values != null)
                        {
                            foreach (var valueDto in resourceDto.Values)
                            {
                                var existingValue = existingResource.Values
                                    .FirstOrDefault(v => v.CultureCode == valueDto.CultureCode);

                                if (existingValue != null)
                                {
                                    // Log change if value has changed
                                    if (existingValue.Value != valueDto.Value)
                                    {
                                        _unitOfWork.LocalizationChangeLogRepository.Add(new LocalizationChangeLog
                                        {
                                            ResourceId = existingResource.Id,
                                            CultureCode = existingValue.CultureCode,
                                            OldValue = existingValue.Value,
                                            NewValue = valueDto.Value,
                                            UserId = userId,
                                            ChangedAt = DateTime.UtcNow
                                        });

                                        // Update value
                                        existingValue.Value = valueDto.Value;
                                        existingValue.UpdatedAt = DateTime.UtcNow;
                                    }
                                }
                                else
                                {
                                    // Add new value
                                    existingResource.Values.Add(new LocalizationValue
                                    {
                                        ResourceId = existingResource.Id,
                                        CultureCode = valueDto.CultureCode,
                                        Value = valueDto.Value,
                                        CreatedAt = DateTime.UtcNow
                                    });
                                }
                            }
                        }

                        _unitOfWork.LocalizationResourceRepository.Update(existingResource);
                    }
                }

                var result = await _unitOfWork.SaveChangesAsync();

                // Refresh the localization cache if successful
                if (result > 0)
                {
                    RefreshLocalizationCache();
                }

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ImportCsvDataAsync(string csvContent, string userId)
        {
            try
            {
                // Split the CSV content into lines
                var lines = csvContent.Split('\n');
                if (lines.Length <= 1)
                {
                    return false;
                }

                // Skip the header line
                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line)) continue;

                    // Parse the CSV line (using helper method)
                    var values = ParseCsvLine(line);
                    if (values.Length < 5) continue;

                    string resourceKey = values[0];
                    string resourceGroup = values[1];
                    string description = values[2];
                    string cultureCode = values[3];
                    string value = values[4];

                    // Skip if resource key is empty
                    if (string.IsNullOrEmpty(resourceKey))
                    {
                        continue;
                    }

                    // Find or create the resource
                    var resource = _unitOfWork.LocalizationResourceRepository.Find(
                        r => r.ResourceKey == resourceKey,
                        include: query => query.Include(r => r.Values)
                    );

                    if (resource == null)
                    {
                        resource = new LocalizationResource
                        {
                            ResourceKey = resourceKey,
                            ResourceGroup = resourceGroup,
                            Description = description,
                            CreatedAt = DateTime.UtcNow,
                            Values = new List<LocalizationValue>()
                        };

                        _unitOfWork.LocalizationResourceRepository.Add(resource);
                    }

                    // Add or update the value for this culture
                    var existingValue = resource.Values
                        .FirstOrDefault(v => v.CultureCode == cultureCode);

                    if (existingValue != null)
                    {
                        // Log change if value has changed
                        if (existingValue.Value != value)
                        {
                            _unitOfWork.LocalizationChangeLogRepository.Add(new LocalizationChangeLog
                            {
                                ResourceId = resource.Id,
                                CultureCode = existingValue.CultureCode,
                                OldValue = existingValue.Value,
                                NewValue = value,
                                UserId = userId,
                                ChangedAt = DateTime.UtcNow
                            });

                            existingValue.Value = value;
                            existingValue.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        resource.Values.Add(new LocalizationValue
                        {
                            CultureCode = cultureCode,
                            Value = value,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                var result = await _unitOfWork.SaveChangesAsync();

                // Refresh the localization cache if successful
                if (result > 0)
                {
                    RefreshLocalizationCache();
                }

                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<(byte[] FileContents, string ContentType, string FileName)> ExportResourcesAsync(string format)
        {
            try
            {
                var resources = _unitOfWork.LocalizationResourceRepository.FindAll(
                    criteria: r => true,
                    include: query => query.Include(r => r.Values)
                ).ToList();

                if (format.ToLowerInvariant() == "json")
                {
                    // Convert to DTOs for JSON export
                    var resourceDtos = resources.Select(r => new LocalizationResourceDto
                    {
                        Id = r.Id,
                        ResourceKey = r.ResourceKey,
                        ResourceGroup = r.ResourceGroup,
                        Description = r.Description,
                        Values = r.Values.Select(v => new LocalizationValueDto
                        {
                            Id = v.Id,
                            CultureCode = v.CultureCode,
                            Value = v.Value
                        }).ToList()
                    }).ToList();

                    // Serialize to JSON
                    var jsonData = System.Text.Json.JsonSerializer.Serialize(resourceDtos, new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                    });

                    return (System.Text.Encoding.UTF8.GetBytes(jsonData),
                        "application/json",
                        $"localization_export_{DateTime.Now:yyyyMMdd}.json");
                }
                else if (format.ToLowerInvariant() == "csv")
                {
                    // Export as CSV
                    var csv = new StringBuilder();
                    csv.AppendLine("ResourceKey,ResourceGroup,Description,CultureCode,Value");

                    foreach (var resource in resources)
                    {
                        foreach (var value in resource.Values)
                        {
                            // Escape CSV values
                            var resourceKey = resource.ResourceKey?.Replace("\"", "\"\"");
                            var resourceGroup = resource.ResourceGroup?.Replace("\"", "\"\"");
                            var description = resource.Description?.Replace("\"", "\"\"");
                            var cultureValue = value.Value?.Replace("\"", "\"\"");

                            csv.AppendLine($"\"{resourceKey}\",\"{resourceGroup}\",\"{description}\",\"{value.CultureCode}\",\"{cultureValue}\"");
                        }
                    }

                    return (System.Text.Encoding.UTF8.GetBytes(csv.ToString()),
                        "text/csv",
                        $"localization_export_{DateTime.Now:yyyyMMdd}.csv");
                }
                else
                {
                    throw new ArgumentException("Unsupported export format");
                }
            }
            catch (Exception)
            {
                return (Array.Empty<byte>(), "text/plain", "export_failed.txt");
            }
        }

        public async Task<int> ScanMissingKeysAsync(string userId)
        {
            try
            {
                var scannedKeys = new List<string>
                {
                    "Common.Save",
                    "Common.Cancel",
                    "Common.Edit",
                    "Common.Delete",
                    "Validation.Required",
                    "Validation.MaxLength",
                    "User.Login",
                    "User.Register"
                };

                int addedCount = 0;

                // Check each key against the database
                foreach (var key in scannedKeys)
                {
                    var exists = _unitOfWork.LocalizationResourceRepository.IsExist(r => r.ResourceKey == key);

                    if (!exists)
                    {
                        // Add the missing key
                        var parts = key.Split('.');
                        var group = parts.Length > 1 ? parts[0] : "Common";

                        var resource = new LocalizationResource
                        {
                            ResourceKey = key,
                            ResourceGroup = group,
                            Description = $"Auto-added by scan on {DateTime.UtcNow}",
                            CreatedAt = DateTime.UtcNow,
                            Values = new List<LocalizationValue>
                            {
                                new LocalizationValue
                                {
                                    CultureCode = "en",
                                    Value = key, // Use the key as default value
                                    CreatedAt = DateTime.UtcNow
                                },
                                new LocalizationValue
                                {
                                    CultureCode = "ar",
                                    Value = key, // Use the key as default value
                                    CreatedAt = DateTime.UtcNow
                                }
                            }
                        };

                        _unitOfWork.LocalizationResourceRepository.Add(resource);
                        addedCount++;
                    }
                }

                if (addedCount > 0)
                {
                    await _unitOfWork.SaveChangesAsync();
                    RefreshLocalizationCache();
                }

                return addedCount;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #region Helper Methods

        private LocalizationViewModel MapToViewModel(LocalizationResource resource)
        {
            return new LocalizationViewModel
            {
                Resource = new LocalizationResourceDto
                {
                    Id = resource.Id,
                    ResourceKey = resource.ResourceKey,
                    ResourceGroup = resource.ResourceGroup,
                    Description = resource.Description
                },
                Values = resource.Values.Select(v => new LocalizationValueViewModel
                {
                    Id = v.Id,
                    CultureCode = v.CultureCode,
                    Value = v.Value
                }).ToList()
            };
        }

        private string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            var currentValue = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        // Escaped quote inside quotes
                        currentValue.Append('"');
                        i++; // Skip the next quote
                    }
                    else
                    {
                        // Toggle quote state
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    // End of value
                    result.Add(currentValue.ToString());
                    currentValue.Clear();
                }
                else
                {
                    currentValue.Append(c);
                }
            }

            // Add the last value
            result.Add(currentValue.ToString());

            return result.ToArray();
        }

        #endregion
    }
}