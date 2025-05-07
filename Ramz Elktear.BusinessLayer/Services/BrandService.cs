using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    // Brand Service Implementation
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<BrandDetails>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.BrandRepository.GetAllAsync();
            var brandDtos = new List<BrandDetails>();

            foreach (var brand in brands)
            {
                brandDtos.Add(new BrandDetails
                {
                    Id = brand.Id,
                    NameAr = brand.NameAr,
                    NameEn = brand.NameEn,
                    DescriptionAr = brand.DescriptionAr,
                    DescriptionEn = brand.DescriptionEn,
                    ImageUrl = await GetBrandImage(brand.ImageId),
                });
            }

            return brandDtos;
        }

        public async Task<BrandDetails> GetBrandByIdAsync(string brandId)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(brandId);
            if (brand == null) throw new ArgumentException("Brand not found");

            return new BrandDetails
            {
                Id = brand.Id,
                NameAr = brand.NameAr,
                NameEn = brand.NameEn,
                DescriptionAr = brand.DescriptionAr,
                DescriptionEn = brand.DescriptionEn,
                ImageUrl = await GetBrandImage(brand.ImageId),
            };
        }

        public async Task<BrandDetails> AddBrandAsync(AddBrand brandDto)
        {
            var brand = new Brand
            {
                NameAr = brandDto.NameAr,
                NameEn = brandDto.NameEn,
                DescriptionAr = brandDto.DescriptionAr,
                DescriptionEn = brandDto.DescriptionEn,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
            };

            await SetBrandImage(brand, brandDto.Image);
            await _unitOfWork.BrandRepository.AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();

            return await GetBrandByIdAsync(brand.Id);
        }

        public async Task<BrandDetails> UpdateBrandAsync(UpdateBrandDto brandDto)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(brandDto.Id);
            if (brand == null) throw new ArgumentException("Brand not found");

            brand.NameAr = brandDto.NameAr;
            brand.NameEn = brandDto.NameEn;
            brand.DescriptionAr = brandDto.DescriptionAr;
            brand.DescriptionEn = brandDto.DescriptionEn;
            brand.LastModifiedDate = DateTime.UtcNow;

            if (brandDto.Image != null)
            {
                await UpdateBrandImage(brand, brandDto.Image);
            }

            _unitOfWork.BrandRepository.Update(brand);
            await _unitOfWork.SaveChangesAsync();

            return await GetBrandByIdAsync(brand.Id);
        }

        public async Task<bool> DeleteBrandAsync(string brandId)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(brandId);
            if (brand == null) throw new ArgumentException("Brand not found");

            _unitOfWork.BrandRepository.Delete(brand);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the brand: " + ex.Message, ex);
            }
        }

        private async Task SetBrandImage(Brand brand, IFormFile image)
        {
            var path = await GetPathByName("ImageBrand");
            brand.ImageId = await _fileHandling.UploadFile(image, path);
        }

        private async Task UpdateBrandImage(Brand brand, IFormFile image)
        {
            var path = await GetPathByName("ImageBrand");
            brand.ImageId = await _fileHandling.UpdateFile(image, path, brand.ImageId);
        }

        private async Task<string> GetBrandImage(string brandId)
        {
            if (string.IsNullOrEmpty(brandId)) return null;
            return await _fileHandling.GetFile(brandId);
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }
    }
}
