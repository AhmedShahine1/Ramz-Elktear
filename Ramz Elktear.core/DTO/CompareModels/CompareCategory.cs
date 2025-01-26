namespace Ramz_Elktear.core.DTO.CompareModels
{
    public class CompareCategory
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<CompareModel> compareModel { get; set; }
    }
}
