using AutoMapper;
using CM.ApiREST.Adapter.DTOs;
using CM.Dominio.Repositories;
using CM.DominioApi.Port.Models;
using CM.Persistence.Adapter.Context;
using Microsoft.AspNetCore.Mvc;

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
            Enterprise result = enterpriseRepository.GetOne(id);
            return Ok(result);
        }


        [HttpPost]
        public IActionResult Create([FromBody] EnterpriseDTO enterpriseDTO)
        {
            Enterprise enterprise = mapper.Map<Enterprise>(enterpriseDTO);

            var result = enterpriseRepository.Add(enterprise);

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] EnterpriseDTO enterpriseDTO)
        {
            var enterprise = mapper.Map<Enterprise>(enterpriseDTO);

            bool result = enterpriseRepository.Update(id, enterprise);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = enterpriseRepository.Delete(id);

            return Ok(result);
        }
    }
}
