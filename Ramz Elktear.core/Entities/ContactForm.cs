namespace Ramz_Elktear.core.Entities
{
    public class ContactForm
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } // اسم المستخدم
        public string Email { get; set; } // البريد الإلكتروني
        public string Phone { get; set; } // رقم الهاتف
        public string Message { get; set; } // الرسالة
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
