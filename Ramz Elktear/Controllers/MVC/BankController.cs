using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.core.DTO.InstallmentModels;

namespace Ramz_Elktear.Controllers.MVC
{
    [Authorize(Policy = "Admin")]
    public class BankController : Controller
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<IActionResult> Index()
        {
            var banks = await _bankService.GetAllBanksAsync();
            return View(banks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddBank bankDto)
        {
            if (!ModelState.IsValid) return View(bankDto);

            await _bankService.AddBankAsync(bankDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var bank = await _bankService.GetBankByIdAsync(id);
            if (bank == null) return NotFound();

            return View(bank);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, BankDetails bankDto)
        {
            if (!ModelState.IsValid) return View(bankDto);

            var success = await _bankService.UpdateBankAsync(bankDto);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var success = await _bankService.DeleteBankAsync(id);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
