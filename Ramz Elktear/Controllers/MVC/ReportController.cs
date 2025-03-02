using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.Reports;

namespace Ramz_Elktear.Controllers.MVC
{
    public class ReportController : Controller
    {
        private readonly ICarService _carService;
        private readonly IOfferService _offerService;
        private readonly IColorService _colorService;
        private readonly IBrandService _brandService;
        private readonly ITransmissionTypeService _transmissionTypeService;
        private readonly IFuelTypeService _fuelTypeService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly IOptionService _optionService;
        private readonly IEngineSizeService _engineSizeService;
        private readonly IOriginService _originService;
        private readonly IModelYearService _modelYearService;
        private readonly IEnginePositionService _enginePositionService;
        private readonly ISpecificationService _specificationService;
        private readonly IBookingService _bookingService;
        private readonly IBranchService _branchService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(ICarService carService, IOfferService offerService, IColorService colorService, IBrandService brandService,
            ITransmissionTypeService transmissionTypeService, IFuelTypeService fuelTypeService, ISubCategoryService subCategoryService, IOptionService optionService,
            IEngineSizeService engineSizeService, IOriginService originService, IModelYearService modelYearService, IEnginePositionService enginePositionService,
            ISpecificationService specificationService, IBookingService bookingService, IBranchService branchService, IWebHostEnvironment webHostEnvironment)
        {
            _carService = carService;
            _offerService = offerService;
            _colorService = colorService;
            _brandService = brandService;
            _transmissionTypeService = transmissionTypeService;
            _fuelTypeService = fuelTypeService;
            _subCategoryService = subCategoryService;
            _optionService = optionService;
            _engineSizeService = engineSizeService;
            _originService = originService;
            _modelYearService = modelYearService;
            _enginePositionService = enginePositionService;
            _specificationService = specificationService;
            _bookingService = bookingService;
            _branchService = branchService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("ViewAllBooking")]
        public async Task<IActionResult> ViewAllBooking()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();

            var report = new Booking();
            report.DataSource = bookings;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                return new FileContentResult(content, "application/pdf");
            }
        }

        [HttpGet("ViewAllBookingSend")]
        public async Task<IActionResult> ViewAllBookingSend()
        {
            var bookings = await _bookingService.GetBookingBystatusAsync(core.Helper.BookingStatus.send);

            var report = new Booking();
            report.DataSource = bookings;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                return new FileContentResult(content, "application/pdf");
            }
        }

        [HttpGet("ViewAllBookingRecive")]
        public async Task<IActionResult> ViewAllBookingRecive()
        {
            var bookings = await _bookingService.GetBookingBystatusAsync(core.Helper.BookingStatus.recieve);

            var report = new Booking();
            report.DataSource = bookings;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                return new FileContentResult(content, "application/pdf");
            }
        }

        [HttpGet("ViewAllCars")]
        public async Task<IActionResult> ViewAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();

            var report = new Cars();
            report.DataSource = cars;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                return new FileContentResult(content, "application/pdf");
            }
        }

        [HttpGet("ViewAllBranchs")]
        public async Task<IActionResult> ViewAllBranchs()
        {
            var branches = await _branchService.GetAllBranchesAsync();

            var report = new Branchs();
            report.DataSource = branches;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                return new FileContentResult(content, "application/pdf");
            }
        }

        [HttpGet("ViewAllOffers")]
        public async Task<IActionResult> ViewAllOffers()
        {
            var offers = await _offerService.GetAllOffersAsync();

            var report = new Offers();
            report.DataSource = offers;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                return new FileContentResult(content, "application/pdf");
            }
        }

        // GET: /booking/report
        [HttpGet("AllBooking")]
        public async Task<IActionResult> AllBooking()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();

            var report = new Booking();
            report.DataSource = bookings;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                var fileName = "BookingReport.pdf";
                string extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    throw new Exception($"Missing file extension on {fileName}");

                if (content.Length <= 0) return null;

                return File(content, "application/pdf", $"{DateTime.Now} -{fileName}");
            }
        }        
        
        // GET: /booking/report
        [HttpGet("AllBookingSend")]
        public async Task<IActionResult> AllBookingSend()
        {
            var bookings = await _bookingService.GetBookingBystatusAsync(core.Helper.BookingStatus.send);

            var report = new Booking();
            report.DataSource = bookings;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                var fileName = "BookingReport.pdf";
                string extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    throw new Exception($"Missing file extension on {fileName}");

                if (content.Length <= 0) return null;

                return File(content, "application/pdf", $"{DateTime.Now} -{fileName}");
            }
        }        
        
        // GET: /booking/report
        [HttpGet("AllBookingRecive")]
        public async Task<IActionResult> AllBookingRecive()
        {
            var bookings = await _bookingService.GetBookingBystatusAsync(core.Helper.BookingStatus.recieve);

            var report = new Booking();
            report.DataSource = bookings;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                var fileName = "BookingReport.pdf";
                string extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    throw new Exception($"Missing file extension on {fileName}");

                if (content.Length <= 0) return null;

                return File(content, "application/pdf", $"{DateTime.Now} -{fileName}");
            }
        }

        [HttpGet("AllCars")]
        public async Task<IActionResult> AllCars()
        {
            var cars = await _carService.GetAllCarsAsync();

            var report = new Cars();
            report.DataSource = cars;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                var fileName = "CarsReport.pdf";
                string extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    throw new Exception($"Missing file extension on {fileName}");

                if (content.Length <= 0) return null;

                return File(content, "application/pdf", $"{DateTime.Now} -{fileName}");
            }
        }        
        
        [HttpGet("AllBranchs")]
        public async Task<IActionResult> AllBranchs()
        {
            var branches = await _branchService.GetAllBranchesAsync();

            var report = new Branchs();
            report.DataSource = branches;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                var fileName = "branchesReport.pdf";
                string extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    throw new Exception($"Missing file extension on {fileName}");

                if (content.Length <= 0) return null;

                return File(content, "application/pdf", $"{DateTime.Now} -{fileName}");
            }
        }        

        [HttpGet("AllOffers")]
        public async Task<IActionResult> AllOffers()
        {
            var offers = await _offerService.GetAllOffersAsync();

            var report = new Offers();
            report.DataSource = offers;

            using (var stream = new MemoryStream())
            {
                await report.ExportToPdfAsync(stream);
                var content = stream.ToArray();
                var fileName = "AllOffersReport.pdf";
                string extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    throw new Exception($"Missing file extension on {fileName}");

                if (content.Length <= 0) return null;

                return File(content, "application/pdf", $"{DateTime.Now} -{fileName}");
            }
        }

    }
}
