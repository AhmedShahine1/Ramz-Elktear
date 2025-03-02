namespace Ramz_Elktear.core.Entities.Cars
{
    public class CarCategory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } // e.g., "Luxury", "Economy"
    }
}
