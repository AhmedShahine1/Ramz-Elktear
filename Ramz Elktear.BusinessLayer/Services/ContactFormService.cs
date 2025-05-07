using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.ContactForms;
using Ramz_Elktear.core.Entities;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class ContactFormService : IContactFormService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactFormService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ContactFormDetails> AddContactFormAsync(AddContactForm formDto)
        {
            var contact = new ContactForm
            {
                Name = formDto.Name,
                Email = formDto.Email,
                Phone = formDto.Phone,
                Message = formDto.Message,
                CreatedOn = DateTime.UtcNow
            };

            await _unitOfWork.ContactFormsRepository.AddAsync(contact);
            await _unitOfWork.SaveChangesAsync();
            return new ContactFormDetails
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                Message = contact.Message,
                CreatedOn = contact.CreatedOn
            };
        }

        public async Task<IEnumerable<ContactFormDetails>> GetAllContactFormsAsync()
        {
            var forms = await _unitOfWork.ContactFormsRepository.GetAllAsync();
            return forms.Select(contact => new ContactFormDetails
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                Message = contact.Message,
                CreatedOn = contact.CreatedOn
            }).ToList();
        }
    }
}
