using AutoMapper;
using DevExpress.XtraRichEdit.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.DTO.CarColorModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.CarOfferModels;
using Ramz_Elktear.core.DTO.CarSpecificationModels;
using Ramz_Elktear.core.DTO.CategoryModels;
using Ramz_Elktear.core.DTO.CityModels;
using Ramz_Elktear.core.DTO.ColorModels;
using Ramz_Elktear.core.DTO.CompareModels;
using Ramz_Elktear.core.DTO.EnginePositionModels;
using Ramz_Elktear.core.DTO.EngineSizeModels;
using Ramz_Elktear.core.DTO.FuelTypeModels;
using Ramz_Elktear.core.DTO.ImageCarModels;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.DTO.ModelYearModels;
using Ramz_Elktear.core.DTO.OfferModels;
using Ramz_Elktear.core.DTO.OptionModels;
using Ramz_Elktear.core.DTO.OriginModels;
using Ramz_Elktear.core.DTO.SpecificationModels;
using Ramz_Elktear.core.DTO.TransmissionTypeModels;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileHandling _fileHandling;
        private readonly ICarColorService _carColorService;
        private readonly IImageCarService _imageCarService;
        private readonly ICarOfferService _carOfferService;
        private readonly ICarSpecificationService _carSpecificationService;
        private readonly IBrandService _brandService;
        private readonly IOptionService _optionService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ITransmissionTypeService _transmissionTypeService;
        private readonly IFuelTypeService _fuelTypeService;
        private readonly IOriginService _originService;
        private readonly IEnginePositionService _enginePositionService;
        private readonly IEngineSizeService _engineSizeService;
        private readonly IModelYearService _modelYearService;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling,
            ICarColorService carColorService, IImageCarService imageCarService,
            ICarOfferService carOfferService, ICarSpecificationService carSpecificationService, IBrandService brandService, IOptionService optionService,
            ISubCategoryService subCategoryService, ITransmissionTypeService transmissionTypeService, IFuelTypeService fuelTypeService, IModelYearService modelYearService,
            IEngineSizeService engineSizeService, IEnginePositionService enginePositionService, IOriginService originService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
            _carColorService = carColorService;
            _imageCarService = imageCarService;
            _carOfferService = carOfferService;
            _carSpecificationService = carSpecificationService;
            _brandService = brandService;
            _optionService = optionService;
            _subCategoryService = subCategoryService;
            _transmissionTypeService = transmissionTypeService;
            _fuelTypeService = fuelTypeService;
            _modelYearService = modelYearService;
            _engineSizeService = engineSizeService;
            _enginePositionService = enginePositionService;
            _originService = originService;
        }

        public async Task<CarDTO> GetCarByIdAsync(string carId)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(carId);
            if (car == null) throw new ArgumentException("Car not found");

            return await PopulateCarDtoAsync(car);
        }

        public async Task<IEnumerable<CarDTO>> GetAllCarsAsync()
        {
            var cars = await _unitOfWork.CarRepository
                .FindAllAsync(x => !x.IsDeleted && x.IsActive,
                              orderBy: q => q
                                  .OrderByDescending(c => c.IsSpecial)
                                  .ThenByDescending(c => c.CreatedDate));

            var carDtos = new List<CarDTO>();

            foreach (var car in cars)
            {
                carDtos.Add(await PopulateCarDtoAsync(car));
            }

            return carDtos;
        }

        public async Task<IEnumerable<CarDTO>> GetCarsByBrandIdAsync(string brandId)
        {
            var cars = await _unitOfWork.CarRepository
                .FindAllAsync(x => x.BrandId == brandId && !x.IsDeleted && x.IsActive,
                              orderBy: q => q
                                  .OrderByDescending(c => c.IsSpecial)
                                  .ThenByDescending(c => c.CreatedDate));

            if (!cars.Any()) return Enumerable.Empty<CarDTO>();

            var carDtos = new List<CarDTO>();
            foreach (var car in cars)
            {
                carDtos.Add(await PopulateCarDtoAsync(car));
            }

            return carDtos;
        }

        public async Task<CarComparisonResult> CompareCarsAsync(CompareCarsRequest request)
        {
            var car1 = await _unitOfWork.CarRepository.FindAsync(x => x.NameEn == request.CarModel1
                                                                      && x.SubCategory.NameEn == request.CarCategory1
                                                                      && x.Option.NameEn == request.CarType1
                                                                      && !x.IsDeleted && x.IsActive);

            var car2 = await _unitOfWork.CarRepository.FindAsync(x => x.NameEn == request.CarModel2
                                                                      && x.SubCategory.NameEn == request.CarCategory2
                                                                      && x.Option.NameEn == request.CarType2
                                                                      && !x.IsDeleted && x.IsActive);

            if (car1 == null) throw new ArgumentException("Car 1 not found");
            if (car2 == null) throw new ArgumentException("Car 2 not found");

            var car1DTO = await PopulateCarDtoAsync(car1);
            var car2DTO = await PopulateCarDtoAsync(car2);

            return new CarComparisonResult
            {
                Car1 = car1DTO,
                Car2 = car2DTO,
                ComparisonSummary = $"Result compare between {car1DTO.NameEn} and {car2DTO.NameEn}"
            };
        }

        public async Task<CarDTO> AddCarAsync(AddCar carDto)
        {
            var car = _mapper.Map<Car>(carDto);

            // Handle main car image
            if (carDto.Image != null)
            {
                await SetCarImage(car, carDto.Image);
            }

            // Add car to database first
            await _unitOfWork.CarRepository.AddAsync(car);
            await _unitOfWork.SaveChangesAsync();
            await _imageCarService.AddCarImageAsync(new AddImageCar { CarId = car.Id, Image = carDto.ImageWithoutBackground, paths = await GetPathByName("ImageWithoutBackground") });

            // Add additional images
            foreach (var color in carDto.ColorId)
            {
                await _carColorService.AddCarColorAsync(new AddCarColor { CarId = car.Id, ColorId = color, IsActive = true });
            }
            foreach (var Image in carDto.Images)
            {
                await _imageCarService.AddCarImageAsync(new AddImageCar { CarId = car.Id, Image = Image, paths = await GetPathByName("CarImages") });
            }
            foreach (var Image in carDto.InsideCarImages)
            {
                await _imageCarService.AddCarImageAsync(new AddImageCar { CarId = car.Id, Image = Image, paths = await GetPathByName("InsideCarImages") });
            }
            foreach (var Specifications in carDto.SpecificationsId)
            {
                await _carSpecificationService.AddCarSpecificationAsync(new AddCarSpecification { CarId = car.Id, SpecificationId = Specifications });
            }
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarDTO>(car);
        }

        public async Task<CarDTO> UpdateCarAsync(UpdateCarDTO carDto)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(carDto.Id);
            if (car == null) throw new ArgumentException("Car not found");

            // Map updated properties
            car.NameAr = carDto.NameAr;
            car.NameEn = carDto.NameEn;
            car.DescrptionAr = carDto.DescrptionAr;
            car.DescrptionEn = carDto.DescrptionEn;
            car.CarCode = carDto.CarCode;
            car.CarSKU = carDto.CarSKU;
            car.Kilometers = carDto.Kilometers;
            car.SellingPrice = carDto.SellingPrice;
            car.InstallmentPrice = carDto.InstallmentPrice;
            car.QuantityInStock = carDto.QuantityInStock;
            car.IsSpecial = carDto.IsSpecial;
            car.IsActive = carDto.IsActive;
            // Handle updating main car image
            if (carDto.Image != null)
            {
                await UpdateCarImage(car, carDto.Image);
            }

            // Handle updating main car Image Without Background
            if (carDto.ImageWithoutBackground != null)
            {
                await _imageCarService.AddCarImageAsync(new AddImageCar { CarId = car.Id, Image = carDto.ImageWithoutBackground, paths = await GetPathByName("ImageWithoutBackground") });
            }

            // Compare and update images
            var existingImages = (await _imageCarService.GetAllCarImageUrlsByPathAsync(car.Id, "CarImages")).ToList();
            var removedImages = existingImages.Where(img => !carDto.ImagesURL.Contains(img)).ToList();
            foreach (var image in removedImages)
            {
                await _imageCarService.DeleteCarImageAsync(image);
            }
            foreach (var newImage in carDto.Images)
            {
                await _imageCarService.AddCarImageAsync(new AddImageCar { CarId = car.Id, Image = newImage, paths = await GetPathByName("CarImages") });
            }

            // Update Inside Car Images
            var existingInsideImages = (await _imageCarService.GetAllCarImageUrlsByPathAsync(car.Id, "InsideCarImages")).ToList();
            var removedInsideImages = existingInsideImages.Where(img => !carDto.InsideCarImagesURL.Contains(img)).ToList();
            foreach (var image in removedInsideImages)
            {
                await _imageCarService.DeleteCarImageAsync(image);
            }
            foreach (var newImage in carDto.InsideCarImages)
            {
                await _imageCarService.AddCarImageAsync(new AddImageCar { CarId = car.Id, Image = newImage, paths = await GetPathByName("InsideCarImages") });
            }

            // Update Colors
            var existingColors = await _carColorService.GetCarColorByCarIdAsync(car.Id);
            var removedColors = existingColors.Where(c => !carDto.ColorId.Contains(c.Id)).ToList();
            var newColors = carDto.ColorId.Where(c => !existingColors.Any(ec => ec.Id == c)).ToList();
            foreach (var color in removedColors)
            {
                await _carColorService.DeleteCarColorAsync(carDto.Id,color.Id);
            }
            foreach (var color in newColors)
            {
                await _carColorService.AddCarColorAsync(new AddCarColor { CarId = car.Id, ColorId = color, IsActive = true });
            }

            // Update Offers
            var existingOffers = await _carOfferService.GetAllOffersForCarAsync(car.Id);
            var removedOffers = existingOffers.Where(o => !carDto.OfferId.Contains(o.Id)).ToList();
            var newOffers = carDto.OfferId.Where(o => !existingOffers.Any(eo => eo.Id == o)).ToList();
            foreach (var offer in removedOffers)
            {
                await _carOfferService.DeleteCarOfferAsync(offer.Id,carDto.Id);
            }
            foreach (var offer in newOffers)
            {
                await _carOfferService.AddCarOfferAsync(new AddCarOffer { CarId = car.Id, OfferId = offer });
            }

            // Update Specifications
            var existingSpecifications = await _carSpecificationService.GetSpecificationsByCarIdAsync(car.Id);
            var removedSpecifications = existingSpecifications.Where(s => !carDto.SpecificationsId.Contains(s.Id)).ToList();
            var newSpecifications = carDto.SpecificationsId.Where(s => !existingSpecifications.Any(es => es.Id == s)).ToList();
            foreach (var spec in removedSpecifications)
            {
                await _carSpecificationService.DeleteCarSpecificationAsync(carDto.Id,spec.Id);
            }
            foreach (var spec in newSpecifications)
            {
                await _carSpecificationService.AddCarSpecificationAsync(new AddCarSpecification { CarId = car.Id, SpecificationId = spec });
            }

            // Save changes
            _unitOfWork.CarRepository.Update(car);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarDTO>(car);
        }

        private async Task UpdateCarImage(Car car, IFormFile newImage)
        {
            var path = await GetPathByName("CarImages");
            car.ImageId = await _fileHandling.UpdateFile(newImage, path, car.ImageId);
        }

        public async Task<bool> DeleteCarAsync(string carId)
        {
            var car = await _unitOfWork.CarRepository.GetByIdAsync(carId);
            if (car == null) throw new ArgumentException("Car not found");

            // Delete related entities first
            await DeleteCarRelatedEntities(carId);

            // Delete the car
            _unitOfWork.CarRepository.Delete(car);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private async Task DeleteCarRelatedEntities(string carId)
        {
            // Delete car colors
            var carColors = await _unitOfWork.CarColorRepository.FindAllAsync(x => x.CarId == carId);
            _unitOfWork.CarColorRepository.DeleteRange(carColors);

            // Delete car offers
            var carOffers = await _unitOfWork.CarOfferRepository.FindAllAsync(x => x.CarId == carId);
            _unitOfWork.CarOfferRepository.DeleteRange(carOffers);

            // Delete car images
            var carImages = await _unitOfWork.ImageCarRepository.FindAllAsync(x => x.CarId == carId);
            _unitOfWork.ImageCarRepository.DeleteRange(carImages);

            // Delete car specifications
            var carSpecifications = await _unitOfWork.CarSpecificationRepository.FindAllAsync(x => x.carId == carId);
            _unitOfWork.CarSpecificationRepository.DeleteRange(carSpecifications);

            var carBooking = await _unitOfWork.BookingRepository.FindAllAsync(q => q.Car.Id == carId);
            _unitOfWork.BookingRepository.DeleteRange(carBooking);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task SetCarImage(Car car, IFormFile image)
        {
            var path = await GetPathByName("CarImages");
            car.ImageId = await _fileHandling.UploadFile(image, path);
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }

        public async Task<InstallmentData> GetInstallmentAsync()
        {
            // Fetch all data from repositories
            var banks = await _unitOfWork.BankRepository.GetAllAsync();
            var jobs = await _unitOfWork.JobRepository.GetAllAsync();
            var brands = await _unitOfWork.BrandRepository.GetAllAsync();

            // Prepare the comparison data
            var compareData = new List<CompareBrand>();

            foreach (var brand in brands)
            {
                // Get all cars for the brand
                var cars = await _unitOfWork.CarRepository.FindAllAsync(
                    c => c.BrandId == brand.Id,
                    include: q => q.Include(c => c.Brand)
                                   .Include(c => c.SubCategory)
                                   .Include(c => c.ModelYear)
                                   .Include(c => c.CarColors),
                    orderBy: q => q
                        .OrderByDescending(c => c.IsSpecial)
                        .ThenByDescending(c => c.CreatedDate));

                // Group cars by category
                var categories = cars.GroupBy(c => c.SubCategory).Select(catGroup =>
                {
                    var models = catGroup.GroupBy(car => car.ModelYear).Select(modelGroup =>
                    {
                        var carDetails = new List<CarDTO>();
                        foreach (var car in modelGroup)
                        {
                            carDetails.Add(PopulateCarDtoAsync(car).Result); // Process one at a time
                        }

                        return new CompareModel
                        {
                            ModelId = modelGroup.Key.Id,
                            ModelName = modelGroup.Key.Name,
                            carDetails = carDetails
                        };
                    }).ToList();

                    return new CompareCategory
                    {
                        CategoryId = catGroup.Key.Id,
                        CategoryName = catGroup.Key.NameAr,
                        compareModel = models
                    };
                }).ToList();

                // Add brand with categories
                compareData.Add(new CompareBrand
                {
                    BrandId = brand.Id,
                    BrandName = brand.NameAr,
                    compareCategory = categories
                });
            }

            // Return the complete installment data
            return new InstallmentData
            {
                Banks = _mapper.Map<IEnumerable<BankDetails>>(banks),
                Jobs = _mapper.Map<IEnumerable<JobDTO>>(jobs),
                BrandDetails = compareData,
            };
        }

        private async Task<CarDTO> PopulateCarDtoAsync(Car car)
        {
            var carDto = _mapper.Map<CarDTO>(car);
            carDto.ImageUrl = await _fileHandling.GetFile(car.ImageId) ?? string.Empty;
            carDto.Brand = await _brandService.GetBrandByIdAsync(car.BrandId) ?? new BrandDetails();
            carDto.Option = await _optionService.GetOptionByIdAsync(car.OptionId) ?? new OptionDTO();
            carDto.SubCategory = await _subCategoryService.GetSubCategoryByIdAsync(car.SubCategoryId) ?? new SubCategoryDTO();
            carDto.TransmissionType = await _transmissionTypeService.GetTransmissionTypeByIdAsync(car.TransmissionTypeId) ?? new TransmissionTypeDTO();
            carDto.FuelType = await _fuelTypeService.GetFuelTypeByIdAsync(car.FuelTypeId) ?? new FuelTypeDTO();
            carDto.Offer = (await _carOfferService.GetAllOffersForCarAsync(car.Id)).ToList() ?? new List<OfferDTO>();
            carDto.Color = (await _carColorService.GetCarColorByCarIdAsync(car.Id)).ToList() ?? new List<ColorDTO>();
            carDto.EnginePosition = await _enginePositionService.GetEnginePositionByIdAsync(car.EnginePositionId) ?? new EnginePositionDTO();
            carDto.EngineSize = await _engineSizeService.GetEngineSizeByIdAsync(car.EngineSizeId) ?? new EngineSizeDTO();
            carDto.Origin = await _originService.GetOriginByIdAsync(car.OriginId) ?? new OriginDTO();
            carDto.ModelYear = await _modelYearService.GetModelYearByIdAsync(car.ModelYearId) ?? new ModelYearDTO();
            carDto.ImagesUrl = (await _imageCarService.GetAllCarImageUrlsByPathAsync(car.Id, "CarImages")).ToList() ?? new List<string>();
            carDto.ImageWithoutBackgroundUrl = (await _imageCarService.GetAllCarImageUrlsByPathAsync(car.Id, "ImageWithoutBackground")).FirstOrDefault();
            carDto.InsideCarImagesUrl = (await _imageCarService.GetAllCarImageUrlsByPathAsync(car.Id, "InsideCarImages")).ToList() ?? new List<string>();
            carDto.Specifications = (await _carSpecificationService.GetSpecificationsByCarIdAsync(car.Id)).ToList() ?? new List<SpecificationDTO>();

            return carDto;
        }
        // Gets all cars (with pagination)
        public async Task<IEnumerable<CarDTO>> GetAllCarsAsync(int size = 20)
        {
            var cars = await _unitOfWork.CarRepository.GetAllAsync(
                include: null,
                orderBy: query => (IOrderedQueryable<Car>)query.Take(size)
            );
            return _mapper.Map<IEnumerable<CarDTO>>(cars);
        }

        // Search cars based on filters
        public async Task<(IEnumerable<CarDTO> cars, int totalCount)> SearchCarsAsync(
            string brandId,
            string categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            int pageNumber,
            int pageSize)
        {
            var query = await _unitOfWork.CarRepository.GetAllAsync(include: q => q.Include(a => a.SubCategory));

            if (!string.IsNullOrEmpty(brandId)) query = query.Where(c => c.BrandId == brandId);
            if (!string.IsNullOrEmpty(categoryId)) query = query.Where(c => c.SubCategory.CategoryId == categoryId);
            if (minPrice.HasValue) query = query.Where(c => c.SellingPrice >= minPrice.Value);
            if (maxPrice.HasValue) query = query.Where(c => c.SellingPrice <= maxPrice.Value);

            int totalCount = query.Count();

            var cars = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Populate DTOs
            var populatedCars = new List<CarDTO>();
            foreach (var car in cars)
            {
                var carDto = await DetailsCarViewAsync(car);
                populatedCars.Add(carDto);
            }

            return (populatedCars, totalCount);
        }


        private async Task<CarDTO> DetailsCarViewAsync(Car car)
        {
            var carDto = _mapper.Map<CarDTO>(car);
            carDto.ImageUrl = await _fileHandling.GetFile(car.ImageId) ?? string.Empty;
            carDto.Brand = await _brandService.GetBrandByIdAsync(car.BrandId) ?? new BrandDetails();
            carDto.Option = await _optionService.GetOptionByIdAsync(car.OptionId) ?? new OptionDTO();
            carDto.TransmissionType = await _transmissionTypeService.GetTransmissionTypeByIdAsync(car.TransmissionTypeId) ?? new TransmissionTypeDTO();
            carDto.FuelType = await _fuelTypeService.GetFuelTypeByIdAsync(car.FuelTypeId) ?? new FuelTypeDTO();
            carDto.EnginePosition = await _enginePositionService.GetEnginePositionByIdAsync(car.EnginePositionId) ?? new EnginePositionDTO();
            carDto.EngineSize = await _engineSizeService.GetEngineSizeByIdAsync(car.EngineSizeId) ?? new EngineSizeDTO();
            carDto.Origin = await _originService.GetOriginByIdAsync(car.OriginId) ?? new OriginDTO();
            carDto.ModelYear = await _modelYearService.GetModelYearByIdAsync(car.ModelYearId) ?? new ModelYearDTO();
            return carDto;
        }

        public async Task<IEnumerable<CarDTO>> SearchCarsbyPageAsync(string? brandId, string? categoryId, string? modelId, int page, int size)
        {
            // Query database efficiently
            var query = _unitOfWork.CarRepository.GetAll(
                include: q => q.Include(a => a.SubCategory).Include(a => a.ModelYear)
            );

            // Apply filters only if values exist
            if (brandId != null)
                query = query.Where(c => c.BrandId == brandId);

            if (categoryId != null)
                query = query.Where(c => c.SubCategory.CategoryId == categoryId);

            if (modelId != null)
                query = query.Where(c => c.ModelYearId == modelId);

            // Apply pagination
            var cars = query.Skip((page - 1) * size).Take(size).ToList();

            // Populate each car DTO with additional data
            var populatedCars = new List<CarDTO>();
            foreach (var car in cars)
            {
                var carDto = await PopulateCarDtoAsync(car);
                populatedCars.Add(carDto);
            }

            return populatedCars;
        }

        public async Task<IEnumerable<CarDTO>> GetCarsByOfferIdAsync(string offerId)
        {
            var carOffers = await _unitOfWork.CarOfferRepository.FindAllAsync(x => x.OfferId == offerId);
            if (!carOffers.Any()) return Enumerable.Empty<CarDTO>();

            var carIds = carOffers.Select(x => x.CarId).ToList();
            var cars = await _unitOfWork.CarRepository.FindAllAsync(x => carIds.Contains(x.Id) && !x.IsDeleted && x.IsActive);

            var carDtos = new List<CarDTO>();
            foreach (var car in cars)
            {
                carDtos.Add(await PopulateCarDtoAsync(car));
            }

            return carDtos;
        }
    }
}