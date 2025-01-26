namespace Ramz_Elktear.core.DTO.CarModels
{
    public class CarDetails
    {
        public string Id { get; set; }
        public string BrandName { get; set; }
        public string BrandId { get; set; }
        public string Type { get; set; }
        public int NumberOfSeats { get; set; }
        public string Category { get; set; }
        public List<string> AvailableColors { get; set; }
        public string Model { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        public double Acceleration { get; set; }
        public double FuelConsumption { get; set; }
        public string Dimensions { get; set; }
        public int StorageCapacity { get; set; }
        public List<string> KeyFeatures { get; set; }
        public List<string> SafetySystems { get; set; }
        public string Name { get; set; }
        public string FuelType { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
