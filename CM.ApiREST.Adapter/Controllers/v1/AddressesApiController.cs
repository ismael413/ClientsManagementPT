using AutoMapper;
using CM.ApiREST.Adapter.DTOs;
using CM.Dominio.Repositories;
using CM.DominioApi.Port.Models.Addreses;
using CM.Persistence.Adapter.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ApiREST.Adapter.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AddressesApiController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly AddressRepository addressRepository;

        public AddressesApiController(
            ApplicationDbContext _context,
            IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
            addressRepository = new(context);
        }

        [HttpGet]
        public IActionResult GetAddresses()
        {
            var result = addressRepository.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetAddress(int id)
        {
            var result = addressRepository.GetOne(id);
            return Ok(result);
        }


        [HttpPost]
        public IActionResult Create([FromBody] AddressDTO addressDTO)
        {
            Address address = mapper.Map<Address>(addressDTO);

            var result = addressRepository.Add(address);

            return result != null ?
                  CreatedAtAction(nameof(GetAddress), address.Id)
                : NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] AddressDTO addressDTO)
        {
            var address = mapper.Map<Address>(addressDTO);

            var result = addressRepository.Update(id, address);

            return result ?
                  RedirectToAction(nameof(GetAddress), id)
                : NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var address = addressRepository.GetOne(id);
            if (address == null) return NotFound();

            var result = addressRepository.Delete(address.Id);

            return result ?
                RedirectToAction(nameof(GetAddresses))
                : NoContent();
        }
    }
}
