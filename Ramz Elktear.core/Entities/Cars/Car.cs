using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Files;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ramz_Elktear.core.Entities.Cars
{
    public class Car
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Brand information
        public Brand Brands { get; set; }

        [ForeignKey(nameof(Brands))]
        public string BrandId { get; set; }

        // Car Type (e.g., Sedan, SUV, Coupe)
        public string Type { get; set; }

        // Number of Seats
        public int NumberOfSeats { get; set; }

        // Car Category (dropdown list)
        public CarCategory Category { get; set; }

        // Available Colors (dropdown list)
        public List<CarColor> AvailableColors { get; set; } = new List<CarColor>();

        // Car Models (dropdown list)
        public CarModel Model { get; set; }

        // Engine details
        public string Engine { get; set; } // e.g., "V6, 3.5L"

        // Transmission type
        public string Transmission { get; set; } // e.g., "Automatic", "Manual"

        // Acceleration (0-100 km/h in seconds)
        public double Acceleration { get; set; } // e.g., 6.5

        // Fuel consumption (liters per 100 km)
        public double FuelConsumption { get; set; } // e.g., 7.8

        // Dimensions: length x width x height (in mm)
        public string Dimensions { get; set; } // e.g., "4500 x 1800 x 1400"

        // Storage capacity (in liters)
        public int StorageCapacity { get; set; } // e.g., 500

        // Key features
        public List<string> KeyFeatures { get; set; } = new List<string>(); // e.g., { "Sunroof", "Leather Seats" }

        // Safety systems
        public List<string> SafetySystems { get; set; } = new List<string>(); // e.g., { "ABS", "Airbags" }

        // Car Name
        public string Name { get; set; } // e.g., "Toyota Camry"

        // Type of Fuel
        public string FuelType { get; set; } // e.g., "Petrol", "Diesel"

        // Transmission Type
        public string TransmissionType { get; set; } // e.g., "Automatic"

        // Price
        public decimal Price { get; set; } // e.g., 25000.99

        // Description
        public string Description { get; set; } // e.g., "A reliable and stylish sedan with advanced safety features."
    }
}
