namespace Ramz_Elktear.core.Entities.Categories
{
    public class CarCategory
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } // e.g., "Luxury", "Economy"
    }
}
