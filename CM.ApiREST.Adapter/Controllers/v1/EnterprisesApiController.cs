using AutoMapper;
using CM.ApiREST.Adapter.DTOs;
using CM.Dominio.Repositories;
using CM.DominioApi.Port.Models;
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
    public class EnterprisesApiController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly EnterpriseRepository enterpriseRepository;

        public EnterprisesApiController(
            ApplicationDbContext _context,
            IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
            enterpriseRepository = new(context);
        }

        [HttpGet]
        public IActionResult GetEnterprises()
        {
            var result = enterpriseRepository.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetEnterprise(int id)
        {
            var result = enterpriseRepository.GetOne(id);
            return Ok(result);
        }


        [HttpPost]
        public IActionResult Create([FromBody] EnterpriseDTO enterpriseDTO)
        {
            Enterprise enterprise = mapper.Map<Enterprise>(enterpriseDTO);

            var result = enterpriseRepository.Add(enterprise);

            return result != null ? 
                  CreatedAtAction(nameof(GetEnterprise), enterprise.Id) 
                : NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] EnterpriseDTO enterpriseDTO)
        {
            var enterprise = mapper.Map<Enterprise>(enterpriseDTO);

            var result = enterpriseRepository.Update(id, enterprise);

            return result ?
                  RedirectToAction(nameof(GetEnterprise), id)
                : NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var enterprise = enterpriseRepository.GetOne(id);
            if (enterprise == null) return NotFound();
         
            var result = enterpriseRepository.Delete(enterprise.Id);

            return result ?
                RedirectToAction(nameof(GetEnterprises))
                : NoContent();
        }
    }
}
