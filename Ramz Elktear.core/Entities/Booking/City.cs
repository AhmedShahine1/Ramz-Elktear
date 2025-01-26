namespace Ramz_Elktear.core.Entities.Booking
{
    public class City
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
    }
}
