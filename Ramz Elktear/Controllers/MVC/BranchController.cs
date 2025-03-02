using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BranchModels;

namespace Ramz_Elktear.Controllers.MVC
{
    namespace Ramz_Elktear.Controllers
    {
        [Authorize(Policy = "Admin")]
        public class BranchController : Controller
        {
            private readonly IBranchService _branchService;

            public BranchController(IBranchService branchService)
            {
                _branchService = branchService;
            }

            // GET: Branch/Index
            public async Task<IActionResult> Index()
            {
                var branches = await _branchService.GetAllBranchesAsync();
                return View(branches);
            }

            // GET: Branch/Create
            public IActionResult Create()
            {
                return View();
            }

            // POST: Branch/Create
            [HttpPost]
            public async Task<IActionResult> Create(AddBranch model)
            {
                if (ModelState.IsValid)
                {
                    await _branchService.AddBranchAsync(model);
                    return RedirectToAction("Index");
                }

                return View(model);
            }

            // GET: Branch/Edit/{id}
            public async Task<IActionResult> Edit(string id)
            {
                var branch = await _branchService.GetBranchByIdAsync(id);
                if (branch == null)
                {
                    return NotFound();
                }

                return View(branch);
            }

            // POST: Branch/Edit/{id}
            [HttpPost]
            public async Task<IActionResult> Edit(BranchDetails model, IFormFile newImage)
            {
                if (ModelState.IsValid)
                {
                    var result = await _branchService.UpdateBranchAsync(model, newImage);
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                }

                return View(model);
            }

            // GET: Branch/Delete/{id}
            public async Task<IActionResult> Delete(string id)
            {
                var result = await _branchService.DeleteBranchAsync(id);
                if (result)
                {
                    return RedirectToAction("Index");
                }

                return NotFound();
            }
        }
    }

}
