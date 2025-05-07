namespace Ramz_Elktear.core.Entities.Installment
{
    public class Job
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public bool IsConvertable { get; set; }
    }
}
