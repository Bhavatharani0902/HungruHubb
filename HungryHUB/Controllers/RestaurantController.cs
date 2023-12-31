﻿using AutoMapper;
using HungryHUB.DTO;
using HungryHUB.Entity;
using HungryHUB.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HungryHUB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IMapper _mapper;

        public RestaurantController(IRestaurantService restaurantService, IMapper mapper)
        {
            _restaurantService = restaurantService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantDTO>> GetAllRestaurants()
        {
            var restaurants = _restaurantService.GetAllRestaurants();
            var restaurantDTOs = _mapper.Map<List<RestaurantDTO>>(restaurants);
            return StatusCode(200, restaurantDTOs);
        }

        [HttpGet("{restaurantId}")]
        public ActionResult<RestaurantDTO> GetRestaurantById(int restaurantId)
        {
            var restaurant = _restaurantService.GetRestaurantById(restaurantId);

            if (restaurant == null)
            {
                return StatusCode(404);
            }

            var restaurantDTO = _mapper.Map<RestaurantDTO>(restaurant);
            return StatusCode(200, restaurantDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Restaurant")]
        public ActionResult CreateRestaurant([FromBody] RestaurantDTO restaurantDTO)
        {
            try
            {
                var restaurant = _mapper.Map<Restaurant>(restaurantDTO);
                _restaurantService.CreateRestaurant(restaurant);
                return StatusCode(200); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut,Route("{restaurantId}")]
        [Authorize(Roles = "Restaurant")]

        public ActionResult UpdateRestaurant(int restaurantId, [FromBody] RestaurantDTO restaurantDTO)
        {
            try
            {
                var existingRestaurant = _restaurantService.GetRestaurantById(restaurantId);

                if (existingRestaurant == null)
                {
                    return StatusCode(404);
                }

                var updatedRestaurant = _mapper.Map<Restaurant>(restaurantDTO);
                _restaurantService.UpdateRestaurant(restaurantId, updatedRestaurant);

                return StatusCode(200); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{restaurantId}")]
        [Authorize(Roles = "Restaurant")]
        public ActionResult DeleteRestaurant(int restaurantId)
        {
            var existingRestaurant = _restaurantService.GetRestaurantById(restaurantId);

            if (existingRestaurant == null)
            {
                return StatusCode(404); 
            }

            _restaurantService.DeleteRestaurant(restaurantId);

            return StatusCode(200); 
        }
    }
}
