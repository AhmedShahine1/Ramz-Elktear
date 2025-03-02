using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.BookingModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.Entities.Booking;
using Ramz_Elktear.core.Helper;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System.Collections.Generic;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICarService _carService;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, ICarService carService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _carService = carService;
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(q=>q
            .Include(w => w.Seller)
            .Include(w => w.Buyer)
            .Include(w => w.Car)
            );
            return bookings.Select(booking =>  new BookingDto()
            {
                Id = booking.Id,
                BookingStatus = booking.Status,
                Seller = _mapper.Map<AuthDTO>(booking.Seller),
                Buyer = _mapper.Map<AuthDTO>(booking.Buyer),
                Car = _mapper.Map<CarDTO>(booking.Car),
            }).ToList();
        }

        public async Task<List<BookingStatsByMonthDto>> GetBookingStatsByMonthAsync()
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync();

            var bookingStats = bookings
                .GroupBy(b => new { b.CreateAt.Year, b.CreateAt.Month })  // Group by year and month
                .Select(g => new BookingStatsByMonthDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    BookingCount = g.Count()  // Count the number of bookings for each month
                })
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .ToList();

            return bookingStats;
        }

        public async Task<BookingDto> GetBookingByIdAsync(string bookingId)
        {
            var booking = await _unitOfWork.BookingRepository.FindAsync(a=>a.Id==bookingId,q=>q
            .Include(w=>w.Seller)
            .Include(w=>w.Buyer)
            .Include(w=>w.Car)
            );
            if (booking == null) throw new ArgumentException("Booking not found");
            var BookingDTO = new BookingDto()
            {
                Id = bookingId,
                BookingStatus = booking.Status,
                Seller = _mapper.Map<AuthDTO>(booking.Seller),
                Buyer = _mapper.Map<AuthDTO>(booking.Buyer),
                Car = await _carService.GetCarByIdAsync(booking.Car.Id),
            };
            return BookingDTO;
        }
        
        public async Task<List<BookingDto>> GetBookingBystatusAsync(BookingStatus bookingstatus)
        {
            var booking = await _unitOfWork.BookingRepository.FindAllAsync(a => a.Status == bookingstatus.ToString(), include: q => q
            .Include(w => w.Buyer)
            .Include(w => w.Seller)
            .Include(w => w.Car)
            );
            if (booking == null) throw new ArgumentException("Booking not found");
            return booking.Select(booking => new BookingDto()
            {
                Id = booking.Id,
                BookingStatus = booking.Status,
                Seller = _mapper.Map<AuthDTO>(booking.Seller),
                Buyer = _mapper.Map<AuthDTO>(booking.Buyer),
                Car = _mapper.Map<CarDTO>(booking.Car),
            }).ToList();

        }

        public async Task<BookingDto> AddBookingAsync(CreateBookingDto createBookingDto)
        {
            var booking = new Booking()
            {
                Buyer = await _unitOfWork.UserRepository.GetByIdAsync(createBookingDto.UserId),
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

        public async Task<bool> AssignSalesAsync(string bookingId, string salesId)
        {
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(bookingId);
            if (booking == null) throw new ArgumentException("Booking not found");

            var salesPerson = await _unitOfWork.UserRepository.GetByIdAsync(salesId);
            if (salesPerson == null) throw new ArgumentException("Salesperson not found");

            booking.Seller = salesPerson;
            _unitOfWork.BookingRepository.Update(booking);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateBookingStatusAsync(string bookingId)
        {
            var booking = await _unitOfWork.BookingRepository.GetByIdAsync(bookingId);
            if (booking == null) throw new ArgumentException("Booking not found");

            booking.Status = BookingStatus.recieve.ToString(); // Change status as needed
            _unitOfWork.BookingRepository.Update(booking);
            await _unitOfWork.SaveChangesAsync();

            return true;
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

        public async Task<List<BookingDto>> GetBookingsBySellerIdAsync(string sellerId)
        {
            var bookings = await _unitOfWork.BookingRepository.FindAllAsync(
                b => b.Seller.Id == sellerId,
                include: q => q
                    .Include(b => b.Buyer)
                    .Include(b => b.Seller)
                    .Include(b => b.Car)
            );

            if (!bookings.Any()) throw new ArgumentException("No bookings found for the specified seller.");

            return bookings.Select(booking => new BookingDto()
            {
                Id = booking.Id,
                BookingStatus = booking.Status,
                Seller = _mapper.Map<AuthDTO>(booking.Seller),
                Buyer = _mapper.Map<AuthDTO>(booking.Buyer),
                Car = _mapper.Map<CarDTO>(booking.Car),
            }).ToList();
        }

        public async Task<List<BookingDto>> GetBookingsByBuyerIdAsync(string buyerId)
        {
            var bookings = await _unitOfWork.BookingRepository.FindAllAsync(
                b => b.Buyer.Id == buyerId,
                include: q => q
                    .Include(b => b.Buyer)
                    .Include(b => b.Seller)
                    .Include(b => b.Car)
            );

            if (!bookings.Any()) return new List<BookingDto>();

            return bookings.Select(booking => new BookingDto()
            {
                Id = booking.Id,
                BookingStatus = booking.Status,
                Seller = _mapper.Map<AuthDTO>(booking.Seller),
                Buyer = _mapper.Map<AuthDTO>(booking.Buyer),
                Car = _carService.GetCarByIdAsync(booking.Car.Id).Result,
            }).ToList();
        }

    }
}
