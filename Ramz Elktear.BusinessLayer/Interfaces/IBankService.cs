using Ramz_Elktear.core.DTO.InstallmentModels;

public interface IBankService
{
    Task<IEnumerable<BankDetails>> GetAllBanksAsync();
    Task<BankDetails> GetBankByIdAsync(string bankId);
    Task<BankDetails> AddBankAsync(AddBank bankDto);
    Task<bool> UpdateBankAsync(BankDetails bankDto);
    Task<bool> DeleteBankAsync(string bankId);
}