//using AutoMapper;
//using Ramz_Elktear.core.DTO.CityModels;
//using Ramz_Elktear.core.Entities.Booking;
//using Ramz_Elktear.RepositoryLayer.Interfaces;

//namespace Ramz_Elktear.BusinessLayer.Services
//{
//    public class CityService : ICityService
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;

//        public CityService(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }

//        public async Task<IEnumerable<CityDto>> GetAllCitiesAsync()
//        {
//            var cities = await _unitOfWork.CityRepository.GetAllAsync();
//            return cities.Select(city => _mapper.Map<CityDto>(city)).ToList();
//        }

//        public async Task<CityDto> GetCityByIdAsync(string cityId)
//        {
//            var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
//            if (city == null) throw new ArgumentException("City not found");

//            return _mapper.Map<CityDto>(city);
//        }

//        public async Task<CityDto> AddCityAsync(AddCityDto addCityDto)
//        {
//            var city = _mapper.Map<City>(addCityDto);
//            await _unitOfWork.CityRepository.AddAsync(city);
//            await _unitOfWork.SaveChangesAsync();

//            return _mapper.Map<CityDto>(city);
//        }

//        public async Task<bool> UpdateCityAsync(UpdateCityDto updateCityDto)
//        {
//            var city = await _unitOfWork.CityRepository.GetByIdAsync(updateCityDto.Id);
//            if (city == null) throw new ArgumentException("City not found");

//            _mapper.Map(updateCityDto, city);
//            _unitOfWork.CityRepository.Update(city);

//            try
//            {
//                await _unitOfWork.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("An error occurred while updating the city: " + ex.Message, ex);
//            }
//        }

//        public async Task<bool> DeleteCityAsync(string cityId)
//        {
//            var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId);
//            if (city == null) throw new ArgumentException("City not found");

//            _unitOfWork.CityRepository.Delete(city);

//            try
//            {
//                await _unitOfWork.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("An error occurred while deleting the city: " + ex.Message, ex);
//            }
//        }
//    }
//}
