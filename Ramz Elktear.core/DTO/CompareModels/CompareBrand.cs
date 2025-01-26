namespace Ramz_Elktear.core.DTO.CompareModels
{
    public class CompareBrand
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<CompareCategory> compareCategory { get; set; }
    }
}
