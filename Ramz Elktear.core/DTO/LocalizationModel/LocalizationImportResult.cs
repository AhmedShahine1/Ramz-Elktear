namespace Ramz_Elktear.core.DTO.LocalizationModel
{
    public class LocalizationImportResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ResourcesAdded { get; set; }
        public int ResourcesUpdated { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
