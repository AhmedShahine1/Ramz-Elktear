using AutoMapper;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.Entities.Installment;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class BankService : IBankService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BankService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BankDetails>> GetAllBanksAsync()
        {
            var banks = await _unitOfWork.BankRepository.GetAllAsync();
            return banks.Select(bank => _mapper.Map<BankDetails>(bank)).ToList();
        }

        public async Task<BankDetails> GetBankByIdAsync(string bankId)
        {
            var bank = await _unitOfWork.BankRepository.GetByIdAsync(bankId);
            if (bank == null) throw new ArgumentException("Bank not found");

            return _mapper.Map<BankDetails>(bank);
        }

        public async Task<BankDetails> AddBankAsync(AddBank bankDto)
        {
            var bank = _mapper.Map<Bank>(bankDto);
            await _unitOfWork.BankRepository.AddAsync(bank);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BankDetails>(bank);
        }

        public async Task<bool> UpdateBankAsync(BankDetails bankDto)
        {
            var bank = await _unitOfWork.BankRepository.GetByIdAsync(bankDto.Id);
            if (bank == null) throw new ArgumentException("Bank not found");

            _mapper.Map(bankDto, bank);
            _unitOfWork.BankRepository.Update(bank);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the bank: " + ex.Message, ex);
            }
        }

        public async Task<bool> DeleteBankAsync(string bankId)
        {
            var bank = await _unitOfWork.BankRepository.GetByIdAsync(bankId);
            if (bank == null) throw new ArgumentException("Bank not found");

            _unitOfWork.BankRepository.Delete(bank);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the bank: " + ex.Message, ex);
            }
        }
    }
}
