namespace Ramz_Elktear.core.Entities.Installment
{
    public class InsurancePercentage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public string Gender { get; set; }
        public decimal Percentage { get; set; }
    }
}
