using Microsoft.AspNetCore.Http;
using Ramz_Elktear.core.Entities.Brands;
using System.Collections.Generic;

namespace Ramz_Elktear.core.DTO.CarModels
{
    public class AddCar
    {
        // Brand Information
        public string BrandId { get; set; }

        // Car Type (e.g., Sedan, SUV, Coupe)
        public string Type { get; set; }

        // Number of Seats
        public int NumberOfSeats { get; set; }

        // Car Category (dropdown list)
        public string Category { get; set; }

        // Available Colors (dropdown list)
        public List<string> AvailableColors { get; set; } = new List<string>();

        // Car Model (dropdown list)
        public string ModelId { get; set; }

        // Engine Details
        public string Engine { get; set; } // e.g., "V6, 3.5L"

        // Transmission Type
        public string Transmission { get; set; } // e.g., "Automatic", "Manual"

        // Acceleration (0-100 km/h in seconds)
        public double Acceleration { get; set; } // e.g., 6.5

        // Fuel Consumption (liters per 100 km)
        public double FuelConsumption { get; set; } // e.g., 7.8

        // Dimensions: length x width x height (in mm)
        public string Dimensions { get; set; } // e.g., "4500 x 1800 x 1400"

        // Storage Capacity (in liters)
        public int StorageCapacity { get; set; } // e.g., 500

        // Key Features
        public List<string> KeyFeatures { get; set; } = new List<string>();

        // Safety Systems
        public List<string> SafetySystems { get; set; } = new List<string>();

        // Car Name
        public string Name { get; set; }

        // Type of Fuel
        public string FuelType { get; set; }

        // Price
        public decimal Price { get; set; }

        // Description
        public string Description { get; set; }

        public List<IFormFile> Images { get; set; } = new List<IFormFile> ();
    }
}
