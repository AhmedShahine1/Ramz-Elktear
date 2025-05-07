namespace Ramz_Elktear.core.Entities.Installment
{
    public class Bank
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Address { get; set; }
        public string SwiftCode { get; set; }
        public string IBAN { get; set; }
        public bool IsAccapted { get; set; } = false;
    }
}
