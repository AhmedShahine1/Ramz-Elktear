namespace Ramz_Elktear.core.DTO.UpdateModels
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public ChangePasswordModel ChangePasswordModel { get; set; } = new ChangePasswordModel();
    }
}
