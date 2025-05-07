using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.core.DTO.InstallmentModels;

namespace Ramz_Elktear.Controllers.MVC
{
    public class JobController : Controller
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var jobs = await _jobService.GetAllAsync();
            return View(jobs);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(AddJobDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _jobService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var job = await _jobService.GetByIdAsync(id);
            if (job == null) return NotFound();

            return View(job);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(string id, JobDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var success = await _jobService.UpdateAsync(dto);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _jobService.DeleteAsync(id);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}