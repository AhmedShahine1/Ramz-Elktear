using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO;

namespace Ramz_Elktear.Controllers.API
{
    public class CarController : BaseController
    {
        private readonly ICarService _carService;
        private readonly IOfferService _offerService;

        public CarController(ICarService carService, IOfferService offerService)
        {
            _carService = carService;
            _offerService = offerService;
        }

        [HttpGet("cars")]
        public async Task<IActionResult> GetAllCars()
        {
            try
            {
                var cars = await _carService.GetAllCarsAsync();
                var response = new BaseResponse
                {
                    status = true,
                    Data = cars,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while retrieving cars: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("find")]
        public async Task<IActionResult> find([FromQuery] int size)
        {
            try
            {
                var cars = await _carService.GetAllCarsAsync(size);
                var response = new BaseResponse
                {
                    status = true,
                    Data = cars,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while retrieving cars: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("CarsByBrand")]
        public async Task<IActionResult> GetCarsByBrand([FromQuery] string BrandId)
        {
            try
            {
                var cars = await _carService.GetCarsByBrandIdAsync(BrandId);
                var response = new BaseResponse
                {
                    status = true,
                    Data = cars,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while retrieving cars by brand: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("car")]
        public async Task<IActionResult> GetCarById([FromQuery] string id)
        {
            try
            {
                var car = await _carService.GetCarByIdAsync(id);
                if (car == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 404,
                        ErrorMessage = "Car not found."
                    };
                    return NotFound(response);
                }

                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = car,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while retrieving the car: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("offers")]
        public async Task<IActionResult> GetAllOffers()
        {
            try
            {
                var offers = await _offerService.GetAllOffersAsync();
                var response = new BaseResponse
                {
                    status = true,
                    Data = offers,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while retrieving offers: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("offer")]
        public async Task<IActionResult> GetOfferById([FromQuery] string id)
        {
            try
            {
                var offer = await _offerService.GetOfferByIdAsync(id);
                if (offer == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 404,
                        ErrorMessage = "Offer not found."
                    };
                    return NotFound(response);
                }

                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = offer,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while retrieving the offer: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("CompareCars")]
        public async Task<IActionResult> CompareCars([FromBody] CompareCarsRequest request)
        {
            try
            {
                var result = await _carService.CompareCarsAsync(request);
                var response = new BaseResponse
                {
                    status = true,
                    Data = result,
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while comparing cars: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("InstallmentData")]
        public async Task<IActionResult> GetInstallmentData()
        {
            try
            {
                var result = await _carService.GetInstallmentAsync();
                var response = new BaseResponse
                {
                    status = true,
                    Data = result,
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while retrieving installment data: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCars([FromQuery] string brandId, [FromQuery] string categoryId, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int size = 20)
        {
            try
            {
                var cars = await _carService.SearchCarsAsync(brandId, categoryId, minPrice, maxPrice, size);
                var response = new BaseResponse
                {
                    status = true,
                    Data = cars,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while searching cars: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("Find/{brandId}/{catId}/{modelId}/{page}/{size}")]
        public async Task<IActionResult> FindCars(string brandId, string catId, string modelId, int page, int size)
        {
            try
            {
                var cars = await _carService.SearchCarsbyPageAsync(brandId, catId, modelId, page, size);
                var response = new BaseResponse
                {
                    status = true,
                    Data = cars,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while finding cars: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

    }
}
