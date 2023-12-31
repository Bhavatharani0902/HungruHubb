﻿using AutoMapper;
using HungryHUB.DTO;
using HungryHUB.Entity;
using HungryHUB.Service;
using HungryHUB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HungryHUB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryPartnerController : ControllerBase
    {
        private readonly IDeliveryPartnerService _deliveryPartnerService;
        private readonly IMapper _mapper;

        public DeliveryPartnerController(IDeliveryPartnerService deliveryPartnerService, IMapper mapper)
        {
            _deliveryPartnerService = deliveryPartnerService;
            _mapper = mapper;
        }

        [HttpGet, Route("GetAllDeliveryPartners")]
        [Authorize(Roles = "Admin")] 
        public IActionResult GetAllDeliveryPartners()
        {
            try
            {
                List<DeliveryPartner> deliveryPartners = _deliveryPartnerService.GetAllDeliveryPartners();
                List<DeliveryPartnerDTO> deliveryPartnersDto = _mapper.Map<List<DeliveryPartnerDTO>>(deliveryPartners);
                return StatusCode(200, deliveryPartnersDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost, Route("CreateDeliveryPartner")]
        [Authorize(Roles = "DeliveryPartner")] 
        public IActionResult CreateDeliveryPartner(DeliveryPartnerDTO deliveryPartnerDto)
        {
            try
            {
                DeliveryPartner deliveryPartner = _mapper.Map<DeliveryPartner>(deliveryPartnerDto);
                _deliveryPartnerService.CreateDeliveryPartner(deliveryPartner);
                return StatusCode(200, deliveryPartner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut, Route("UpdateDeliveryPartner/{deliveryPartnerId}")]
        [Authorize(Roles = "Admin")] 
        public IActionResult UpdateDeliveryPartner(string deliveryPartnerId, DeliveryPartnerDTO deliveryPartnerDto)
        {
            try
            {
                var existingDeliveryPartner = _deliveryPartnerService.GetDeliveryPartnerById(deliveryPartnerId);

                if (existingDeliveryPartner == null)
                {
                    return NotFound(); 
                }

                var updatedDeliveryPartner = _mapper.Map<DeliveryPartner>(deliveryPartnerDto);
                _deliveryPartnerService.UpdateDeliveryPartner(deliveryPartnerId, updatedDeliveryPartner);

                return StatusCode(200, updatedDeliveryPartner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete, Route("DeleteDeliveryPartner/{deliveryPartnerId}")]
        [Authorize(Roles = "Admin")] 
        public IActionResult DeleteDeliveryPartner(string deliveryPartnerId)
        {
            var existingDeliveryPartner = _deliveryPartnerService.GetDeliveryPartnerById(deliveryPartnerId);

            if (existingDeliveryPartner == null)
            {
                return NotFound(); 
            }

            _deliveryPartnerService.DeleteDeliveryPartner(deliveryPartnerId);

            return StatusCode(200); 
        }
    }
}
