using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.Helper
{
    public enum BookingStatus
    {
        [Display(Name = "تم الاستلام")]
        recieve,

        [Display(Name = "تم الإرسال")]
        send
    }
}
