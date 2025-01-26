using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.Entities.ApplicationData;
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
            var brands = await _unitOfWork.BrandRepository.GetAllAsync(include: q => q.Include(b => b.Logo));
            var brandDto = new List<BrandDetails>();
            foreach (var brand in brands)
            {
                var b = new BrandDetails()
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    LogoUrl = await GetbrandImage(brand.LogoId),
                };
                brandDto.Add(b);
            }
            return brandDto;
        }

        public async Task<BrandDetails> GetBrandByIdAsync(string brandId)
        {
            var brand = await _unitOfWork.BrandRepository.FindAsync(a => a.Id == brandId, include: q => q.Include(b => b.Logo));
            if (brand == null) throw new ArgumentException("Brand not found");
            var brandDto = new BrandDetails()
            {
                Id = brandId,
                Name = brand.Name,
                LogoUrl = await GetbrandImage(brand.LogoId),
            };
            
            return brandDto;
        }

        public async Task<BrandDetails> AddBrandAsync(AddBrand brandDto)
        {
            var brand = new Brand()
            {
                Name = brandDto.Name,
            };
            await SetBrandImage(brand, brandDto.Logo);
            await _unitOfWork.BrandRepository.AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();
            var res = new BrandDetails()
            {
                Id = brand.Id,
                Name = brand.Name,
                LogoUrl = await GetbrandImage(brand.LogoId),
            };

            return res;
        }

        public async Task<bool> UpdateBrandAsync(string brandId, BrandDetails brandDto)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(brandId);
            if (brand == null) throw new ArgumentException("Brand not found");

            _mapper.Map(brandDto, brand);
            _unitOfWork.BrandRepository.Update(brand);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the brand: " + ex.Message, ex);
            }
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

        public async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }

        private async Task SetBrandImage(Brand brand, IFormFile imageProfile)
        {
            var path = await GetPathByName("LogoBrand");
            brand.LogoId = await _fileHandling.UploadFile(imageProfile, path);
        }

        private async Task UpdatebrandImage(Brand brand, IFormFile imageProfile)
        {
            var path = await GetPathByName("LogoBrand");
            brand.LogoId = await _fileHandling.UpdateFile(imageProfile, path,brand.LogoId);
        }
        public async Task<string> GetbrandImage(string brandId)
        {
            if (string.IsNullOrEmpty(brandId))
            {
                return null;
            }

            var brandImage = await _fileHandling.GetFile(brandId);
            return brandImage;
        }

    }
}
