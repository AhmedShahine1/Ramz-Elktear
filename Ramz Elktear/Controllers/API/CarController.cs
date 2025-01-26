using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.OfferModels;
using Ramz_Elktear.core.Entities.Cars;

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

        [HttpGet("CarsByBrand")]
        public async Task<IActionResult> GetCarsByBrand([FromQuery] string BrandId)
        {
            try
            {
                var cars = await _carService.GetCarsByBrandAsync(BrandId);
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

        [HttpPost("AddCarModel")]
        public async Task<IActionResult> AddCarModel([FromBody] CarModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Car model data is invalid.");

                var result = await _carService.AddCarModelAsync(model);
                if (result)
                    return Ok("Car model added successfully.");
                return StatusCode(500, "An error occurred while adding the car model.");
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while adding the car model: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("AddCarColor")]
        public async Task<IActionResult> AddCarColor([FromBody] CarColor color)
        {
            try
            {
                if (color == null)
                    return BadRequest("Car color data is invalid.");

                var result = await _carService.AddCarColorAsync(color);
                if (result)
                    return Ok("Car color added successfully.");
                return StatusCode(500, "An error occurred while adding the car color.");
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while adding the car color: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("AddCarCategory")]
        public async Task<IActionResult> AddCarCategory([FromBody] CarCategory category)
        {
            try
            {
                if (category == null)
                    return BadRequest("Car category data is invalid.");

                var result = await _carService.AddCarCategoryAsync(category);
                if (result)
                    return Ok("Car category added successfully.");
                return StatusCode(500, "An error occurred while adding the car category.");
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while adding the car category: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("car")]
        public async Task<IActionResult> AddCar([FromForm] AddCar carDto)
        {
            try
            {
                if (carDto == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 400,
                        ErrorMessage = "Invalid data."
                    };
                    return BadRequest(response);
                }

                var newCar = await _carService.AddCarAsync(carDto);
                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = newCar,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return CreatedAtAction(nameof(GetCarById), new { id = newCar.Id }, successResponse);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while adding the car: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("Offer")]
        public async Task<IActionResult> AddOffer([FromForm] AddOffer offerDto)
        {
            try
            {
                if (offerDto == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 400,
                        ErrorMessage = "Invalid data."
                    };
                    return BadRequest(response);
                }

                var newOffer = await _offerService.AddOfferAsync(offerDto);
                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = newOffer,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return CreatedAtAction(nameof(GetOfferById), new { id = newOffer.Id }, successResponse);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"An error occurred while adding the offer: {ex.Message}"
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

        [HttpGet("CompareCarsData")]
        public async Task<IActionResult> GetCarComparisonData()
        {
            try
            {
                var result = await _carService.GetCarComparisonDataWithBrandAsync();
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
                    ErrorMessage = $"An error occurred while retrieving comparison data: {ex.Message}"
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
    }
}
