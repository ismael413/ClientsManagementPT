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

namespace CM.ApiREST.Adapter.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CountriesApiController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly CountryRepository countryRepository;

        public CountriesApiController(
            ApplicationDbContext _context,
            IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
            countryRepository = new(context);
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            var result = countryRepository.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetCountry(int id)
        {
            var result = countryRepository.GetOne(id);
            return Ok(result);
        }


        [HttpPost]
        public IActionResult Create([FromBody] CountryDTO countryDTO)
        {
            Country country = mapper.Map<Country>(countryDTO);

            var result = countryRepository.Add(country);

            return result != null ?
                  RedirectToAction(nameof(GetCountry), country.Id)
                : NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] CountryDTO countryDTO)
        {
            var country = mapper.Map<Country>(countryDTO);

            var result = countryRepository.Update(id, country);

            return result ?
                  RedirectToAction(nameof(GetCountry), id)
                : NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var country = countryRepository.GetOne(id);
            if (country == null) return NotFound();

            var result = countryRepository.Delete(country.Id);

            return result ?
                RedirectToAction(nameof(GetCountries))
                : NoContent();
        }
    }
}
