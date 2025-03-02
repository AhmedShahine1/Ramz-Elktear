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

        // GET: Job/Index
        public async Task<IActionResult> Index()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return View(jobs);
        }

        public IActionResult Create()
        {
            return View();
        }
        // POST: Job/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddJob jobDto)
        {
            if (!ModelState.IsValid) return View(jobDto);

            await _jobService.AddJobAsync(jobDto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Job/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound();
            return View(new UpdateJob()
            {
                Id = job.Id,
                Name = job.Name,
                Description = job.Description
            });
        }

        // POST: Job/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateJob jobDto)
        {
            if (!ModelState.IsValid) return View(jobDto);

            var result = await _jobService.UpdateJobAsync(jobDto);
            if (!result) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // POST: Job/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _jobService.DeleteJobAsync(id);
            if (!result) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}