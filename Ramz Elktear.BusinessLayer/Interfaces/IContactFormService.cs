using Ramz_Elktear.core.DTO.ContactForms;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IContactFormService
    {
        Task<ContactFormDetails> AddContactFormAsync(AddContactForm formDto);
        Task<IEnumerable<ContactFormDetails>> GetAllContactFormsAsync();
    }
}
