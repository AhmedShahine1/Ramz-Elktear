using Ramz_Elktear.core.DTO.BookingModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto> GetBookingByIdAsync(string id);
        Task<BookingDto> AddBookingAsync(CreateBookingDto createBookingDto);
    }
}
