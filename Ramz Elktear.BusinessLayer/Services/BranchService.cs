using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BranchModels;
using Ramz_Elktear.core.Entities.Branchs;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileHandling _fileHandling;

        public BranchService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<BranchDetails>> GetAllBranchesAsync()
        {
            var branches = await _unitOfWork.BranchRepository.GetAllAsync();
            var branchDTOs = new List<BranchDetails>();

            foreach (var branch in branches)
            {
                branchDTOs.Add(new BranchDetails()
                {
                    Id = branch.Id,
                    Name = branch.Name,
                    PhoneNumber = branch.PhoneNumber,
                    Email = branch.Email,
                    StartWork = branch.StartWork,
                    EndWork = branch.EndWork,
                    ImageUrl = await GetBranchImage(branch.ImageId),
                });
            }

            return branchDTOs;
        }

        public async Task<BranchDetails> GetBranchByIdAsync(string branchId)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(branchId);
            if (branch == null) throw new ArgumentException("Branch not found");

            var branchDTO = _mapper.Map<BranchDetails>(branch);
            branchDTO.ImageUrl = await GetBranchImage(branch.ImageId);

            return branchDTO;
        }

        public async Task<BranchDetails> AddBranchAsync(AddBranch branchDto)
        {
            var branch = _mapper.Map<Branch>(branchDto);
            await SetBranchImage(branch, branchDto.Image);

            await _unitOfWork.BranchRepository.AddAsync(branch);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BranchDetails>(branch);
        }

        public async Task<bool> UpdateBranchAsync(BranchDetails branchDto, IFormFile newImage = null)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(branchDto.Id);
            if (branch == null) throw new ArgumentException("Branch not found");

            _mapper.Map(branchDto, branch);

            if (newImage != null)
            {
                await UpdateBranchImage(branch, newImage);
            }

            _unitOfWork.BranchRepository.Update(branch);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the branch: " + ex.Message, ex);
            }
        }

        public async Task<bool> DeleteBranchAsync(string branchId)
        {
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(branchId);
            if (branch == null) throw new ArgumentException("Branch not found");

            _unitOfWork.BranchRepository.Delete(branch);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the branch: " + ex.Message, ex);
            }
        }

        private async Task SetBranchImage(Branch branch, IFormFile image)
        {
            var path = await GetPathByName("ImageBranch");
            branch.ImageId = await _fileHandling.UploadFile(image, path);
        }

        private async Task UpdateBranchImage(Branch branch, IFormFile newImage)
        {
            var path = await GetPathByName("ImageBranch");
            branch.ImageId = await _fileHandling.UpdateFile(newImage, path, branch.ImageId);
        }

        private async Task<string> GetBranchImage(string imageId)
        {
            if (string.IsNullOrEmpty(imageId))
            {
                return null;
            }

            return await _fileHandling.GetFile(imageId);
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }
    }
}
