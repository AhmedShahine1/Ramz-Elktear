using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.BookingModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.CityModels;
using Ramz_Elktear.core.Entities.Booking;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(q=>q
            .Include(w => w.User)
            .Include(w => w.Car)
            .Include(w => w.City)
            );
            return bookings.Select(booking =>  new BookingDto()
            {
                Id = booking.Id,
                User = _mapper.Map<AuthDTO>(booking.User),
                Car = _mapper.Map<CarDetails>(booking.Car),
                City = _mapper.Map<CityDto>(booking.City),
            }).ToList();
        }

        public async Task<BookingDto> GetBookingByIdAsync(string bookingId)
        {
            var booking = await _unitOfWork.BookingRepository.FindAsync(a=>a.Id==bookingId,q=>q
            .Include(w=>w.User)
            .Include(w=>w.Car)
            .Include(w=>w.City)
            );
            if (booking == null) throw new ArgumentException("Booking not found");
            var BookingDTO = new BookingDto()
            {
                Id = bookingId,
                User = _mapper.Map<AuthDTO>(booking.User),
                Car = _mapper.Map<CarDetails>(booking.Car),
                City = _mapper.Map<CityDto>(booking.City),
            };
            return BookingDTO;
        }

        public async Task<BookingDto> AddBookingAsync(CreateBookingDto createBookingDto)
        {
            var booking = new Booking()
            {
                City = await _unitOfWork.CityRepository.GetByIdAsync(createBookingDto.CityId),
                User = await _unitOfWork.UserRepository.GetByIdAsync(createBookingDto.UserId),
                Car = await _unitOfWork.CarRepository.GetByIdAsync(createBookingDto.CarId)
            }; 
            await _unitOfWork.BookingRepository.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return await GetBookingByIdAsync(booking.Id);
        }

        public async Task<bool> UpdateBookingAsync(string bookingId, BookingDto bookingDto)
        {
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(bookingId);
            if (booking == null) throw new ArgumentException("Booking not found");

            _mapper.Map(bookingDto, booking);
            _unitOfWork.BookingRepository.Update(booking);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the booking: " + ex.Message, ex);
            }
        }

        public async Task<bool> DeleteBookingAsync(string bookingId)
        {
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(bookingId);
            if (booking == null) throw new ArgumentException("Booking not found");

            _unitOfWork.BookingRepository.Delete(booking);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the booking: " + ex.Message, ex);
            }
        }
    }
}
