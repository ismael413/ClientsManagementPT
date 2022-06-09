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
    public class CitiesApiController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly CityRepository cityRepository;

        public CitiesApiController(
            ApplicationDbContext _context,
            IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
            cityRepository = new(context);
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var result = cityRepository.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var result = cityRepository.GetOne(id);
            return Ok(result);
        }


        [HttpPost]
        public IActionResult Create([FromBody] CityDTO cityDTO)
        {
            City city = mapper.Map<City>(cityDTO);

            var result = cityRepository.Add(city);

            return result != null ?
                  CreatedAtAction(nameof(GetCity), city.Id)
                : NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] CityDTO cityDTO)
        {
            var city = mapper.Map<City>(cityDTO);

            var result = cityRepository.Update(id, city);

            return result ?
                  RedirectToAction(nameof(GetCity), id)
                : NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var city = cityRepository.GetOne(id);
            if (city == null) return NotFound();

            var result = cityRepository.Delete(city.Id);

            return result ?
                RedirectToAction(nameof(GetCities))
                : NoContent();
        }
    }
}
