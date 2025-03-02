using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.CityModels;
using Ramz_Elktear.core.DTO.CompareModels;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IMapper _mapper;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<CarDetails>> GetAllCarsAsync()
        {
            var cars = await _unitOfWork.CarRepository.GetAllAsync(
                include: q => q.Include(c => c.Brands)
                               .Include(c => c.Category)
                               .Include(c => c.Model)
                               .Include(c => c.AvailableColors)
            );

            var carsDto = _mapper.Map<IEnumerable<CarDetails>>(cars);
            foreach (var carDTO in carsDto)
            {
                var imagecar = await _unitOfWork.ImageCarRepository.FindAllAsync(q => q.CarId == carDTO.Id);
                carDTO.ImageUrls = await GetImageUrls(imagecar);
            }
            return carsDto;
        }

        public async Task<CarDetails> GetCarByIdAsync(string carId)
        {
            var car = await _unitOfWork.CarRepository.FindAsync(
                q => q.Id == carId,
                include: q => q.Include(c => c.Brands)
                               .Include(c => c.Category)
                               .Include(c => c.Model)
                               .Include(c => c.AvailableColors)
            );

            if (car == null) throw new ArgumentException("Car not found");

            var carDTO = _mapper.Map<CarDetails>(car);
            var imagecar = await _unitOfWork.ImageCarRepository.FindAllAsync(q => q.CarId == car.Id);
            carDTO.ImageUrls = await GetImageUrls(imagecar);
            return carDTO;
        }

        public async Task<CarDetails> AddCarAsync(AddCar carDto)
        {
            // Validate the input
            if (string.IsNullOrEmpty(carDto.Type))
            {
                throw new ArgumentException("Car type is required.");
            }

            var car = _mapper.Map<Car>(carDto);

            // Ensure all necessary entities are present
            car.Brands = await _unitOfWork.BrandRepository.GetByIdAsync(carDto.BrandId);
            car.Category = await _unitOfWork.CarCategoryRepository.GetByIdAsync(carDto.Category);
            car.Model = await _unitOfWork.CarModelRepository.GetByIdAsync(carDto.ModelId);

            // Add available colors
            car.AvailableColors = new List<CarColor>();
            foreach (var colorId in carDto.AvailableColors)
            {
                var color = await _unitOfWork.CarColorRepository.GetByIdAsync(colorId);
                if (color != null)
                {
                    car.AvailableColors.Add(color);
                }
                else
                {
                    throw new ArgumentException($"Color with Id {colorId} does not exist.");
                }
            }

            // Add the car entity to the repository
            await _unitOfWork.CarRepository.AddAsync(car);

            // Handle images if provided
            if (carDto.Images.Count > 0)
            {
                foreach (var file in carDto.Images)
                {
                    if (file != null)
                    {
                        var path = await GetPathByName("ImagesCar");
                        var imageCar = new ImageCar
                        {
                            CarId = car.Id,
                            ImageId = await _fileHandling.UploadFile(file, path)
                        };
                        await _unitOfWork.ImageCarRepository.AddAsync(imageCar);
                    }
                }
            }

            // Save the changes and return the car details
            await _unitOfWork.SaveChangesAsync();
            return await GetCarByIdAsync(car.Id);
        }

        private async Task<List<string>> GetImageUrls(IEnumerable<ImageCar> imageCars)
        {
            var imageUrls = new List<string>();
            foreach (var image in imageCars)
            {
                var url = await GetcarImage(image.ImageId);
                if (!string.IsNullOrEmpty(url))
                {
                    imageUrls.Add(url);
                }
            }
            return imageUrls;
        }

        public async Task<bool> AddCarColorAsync(CarColor color)
        {
            await _unitOfWork.CarColorRepository.AddAsync(color);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the color: " + ex.Message, ex);
            }
        }

        public async Task<bool> AddCarCategoryAsync(CarCategory category)
        {
            await _unitOfWork.CarCategoryRepository.AddAsync(category);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the category: " + ex.Message, ex);
            }
        }

        public async Task<bool> AddCarModelAsync(CarModel model)
        {
            await _unitOfWork.CarModelRepository.AddAsync(model);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the model: " + ex.Message, ex);
            }
        }

        public async Task<IEnumerable<CarModel>> GetAllCarModelsAsync()
        {
            return await _unitOfWork.CarModelRepository.GetAllAsync();
        }

        public async Task<IEnumerable<CarCategory>> GetAllCarCategoriesAsync()
        {
            return await _unitOfWork.CarCategoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<CarColor>> GetAllCarColorsAsync()
        {
            return await _unitOfWork.CarColorRepository.GetAllAsync();
        }

        public async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }

        public async Task<string> GetcarImage(string ImageId)
        {
            if (string.IsNullOrEmpty(ImageId))
            {
                return null;
            }

            var carImage = await _fileHandling.GetFile(ImageId);
            return carImage;
        }

        public async Task<IEnumerable<CarDetails>> GetCarsByBrandAsync(string brandId)
        {
            var cars = await _unitOfWork.CarRepository.FindAllAsync(
                a => a.Brands.Id == brandId,
                include: q => q.Include(c => c.Brands)
                               .Include(c => c.Category)
                               .Include(c => c.Model)
                               .Include(c => c.AvailableColors)
            );

            var carsDto = _mapper.Map<IEnumerable<CarDetails>>(cars);
            foreach (var carDTO in carsDto)
            {
                var imagecar = await _unitOfWork.ImageCarRepository.FindAllAsync(q => q.CarId == carDTO.Id);
                carDTO.ImageUrls = await GetImageUrls(imagecar);
            }
            return carsDto;
        }

        public async Task<CarComparisonResult> CompareCarsAsync(CompareCarsRequest request)
        {
            var car1 = await _unitOfWork.CarRepository.FindAsync(
                q => q.Model.Id == request.CarModel1 && q.Category.Id == request.CarCategory1 && q.Type == request.CarType1,
                include: q => q.Include(c => c.Brands)
                               .Include(c => c.Category)
                               .Include(c => c.Model)
                               .Include(c => c.AvailableColors));

            var car2 = await _unitOfWork.CarRepository.FindAsync(
                q => q.Model.Id == request.CarModel2 && q.Category.Id == request.CarCategory2 && q.Type == request.CarType2,
                include: q => q.Include(c => c.Brands)
                               .Include(c => c.Category)
                               .Include(c => c.Model)
                               .Include(c => c.AvailableColors));

            if (car1 == null || car2 == null)
                throw new ArgumentException("One or both cars not found.");

            return new CarComparisonResult
            {
                Car1 = _mapper.Map<CarDetails>(car1),
                Car2 = _mapper.Map<CarDetails>(car2),
                ComparisonSummary = $"Comparison between {car1.Name} and {car2.Name}"
            };
        }

        public async Task<CarComparisonData> GetCarComparisonDataAsync()
        {
            var cars = await _unitOfWork.CarRepository.GetAllAsync();
            var models = await _unitOfWork.CarModelRepository.GetAllAsync();
            var categories = await _unitOfWork.CarCategoryRepository.GetAllAsync();

            return new CarComparisonData
            {
                Cars = _mapper.Map<IEnumerable<CarDetails>>(cars),
                Models = _mapper.Map<IEnumerable<CarModel>>(models),
                Categories = _mapper.Map<IEnumerable<CarCategory>>(categories),
                Types = cars.Select(c => c.Type).Distinct()
            };
        }

        public async Task<IEnumerable<BrandWithDetails>> GetCarComparisonDataWithBrandAsync()
        {
            var brands = await _unitOfWork.BrandRepository.GetAllAsync();
            var compareDate = new List<BrandWithDetails>();
            foreach(var brand in brands)
            {
                var cars = await _unitOfWork.CarRepository.FindAllAsync(
                    a => a.Brands.Id == brand.Id,
                    include: q => q.Include(c => c.Brands)
                                   .Include(c => c.Category)
                                   .Include(c => c.Model)
                                   .Include(c => c.AvailableColors)
                );

                var categories = cars.Select(c => c.Category).Distinct().ToList();
                var models = cars.Select(c => c.Model).Distinct().ToList();
                var modelDtos = _mapper.Map<IEnumerable<CarModel>>(models);
                var categoryDtos = _mapper.Map<IEnumerable<CarCategory>>(categories);

                compareDate.Add(new BrandWithDetails
                {
                    BrandId = brand.Id,
                    BrandName = brand.Name,
                    Cars = _mapper.Map<IEnumerable<CarDetails>>(cars),
                    Models = modelDtos,
                    Categories = categoryDtos,
                });
            }
            return compareDate;
        }

        public async Task<InstallmentData> GetInstallmentAsync()
        {
            // Fetch all data from repositories
            var banks = await _unitOfWork.BankRepository.GetAllAsync();
            var jobs = await _unitOfWork.JobRepository.GetAllAsync();
            var brands = await _unitOfWork.BrandRepository.GetAllAsync();
            var cities = await _unitOfWork.CityRepository.GetAllAsync();

            // Prepare the comparison data
            var compareData = new List<CompareBrand>();

            foreach (var brand in brands)
            {
                // Get all cars for the brand
                var cars = await _unitOfWork.CarRepository.FindAllAsync(
                    c => c.Brands.Id == brand.Id,
                    include: q => q.Include(c => c.Brands)
                                   .Include(c => c.Category)
                                   .Include(c => c.Model)
                                   .Include(c => c.AvailableColors)
                );

                // Group cars by category
                var categories = cars.GroupBy(c => c.Category).Select(catGroup =>
                {
                    var models = catGroup.GroupBy(car => car.Model).Select(modelGroup =>
                    {
                        // Map car details for each model
                        var carDetails = modelGroup.Select(car => _mapper.Map<CarDetails>(car)).ToList();
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
                        CategoryName = catGroup.Key.Name,
                        compareModel = models
                    };
                }).ToList();

                // Add brand with categories
                compareData.Add(new CompareBrand
                {
                    BrandId = brand.Id,
                    BrandName = brand.Name,
                    compareCategory = categories
                });
            }

            // Return the complete installment data
            return new InstallmentData
            {
                Banks = _mapper.Map<IEnumerable<BankDetails>>(banks),
                Jobs = _mapper.Map<IEnumerable<JobDetails>>(jobs),
                BrandDetails = compareData,
                Cities = _mapper.Map<IEnumerable<CityDto>>(cities)
            };
        }
    }
}
