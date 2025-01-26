namespace Ramz_Elktear.core.DTO
{
    public class BaseResponse
    {
        public bool status { get; set; } = true;
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public dynamic Data { get; set; }
    }
}
