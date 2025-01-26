namespace Ramz_Elktear.core.Entities.Cars
{

    public class CarColor
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } // e.g., "Red", "Blue"
        public List<Car> Cars { get; set; }
    }
}
