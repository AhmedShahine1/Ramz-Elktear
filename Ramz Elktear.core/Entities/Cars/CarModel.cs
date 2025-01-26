namespace Ramz_Elktear.core.Entities.Cars
{

    public class CarModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } // e.g., "2025", "2024"
    }
}
