using Ramz_Elktear.core.DTO.BookingModels;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<List<BookingDto>> GetBookingBystatusAsync(BookingStatus bookingstatus);
        Task<BookingDto> GetBookingByIdAsync(string id);
        Task<BookingDto> AddBookingAsync(CreateBookingDto createBookingDto);
        Task<bool> UpdateBookingStatusAsync(string bookingId);
        Task<bool> AssignSalesAsync(string bookingId, string salesId);
        Task<List<BookingDto>> GetBookingsByBuyerIdAsync(string buyerId);
        Task<List<BookingDto>> GetBookingsBySellerIdAsync(string sellerId);
        Task<bool> DeleteBookingAsync(string bookingId);
        Task<List<BookingDto>> GetBookingByUserAsync(string userId);
        Task<List<BookingDto>> GetBookingsByManagerIdAsync(string managerId);
    }
}
