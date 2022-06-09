using AutoMapper;
using CM.ApiREST.Adapter.DTOs;
using CM.Dominio.Repositories;
using CM.DominioApi.Port.Models;
using CM.DominioApi.Port.Ports;
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
    public class ClientsApiController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAddressRepository addressRepository;
        private readonly ClientRepository clientRepository;

        public ClientsApiController(
            ApplicationDbContext _context,
            IMapper _mapper,
            IAddressRepository _addressRepository)
        {
            context = _context;
            mapper = _mapper;
            addressRepository = _addressRepository;
            clientRepository = new(context, addressRepository);
        }

        [HttpGet]
        public IActionResult GetClients()
        {
            var result = clientRepository.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetClient(int id)
        {
            var result = clientRepository.GetOne(id);
            return Ok(result);
        }


        [HttpPost]
        public IActionResult Create([FromBody] ClientDTO clientDTO)
        {
            Client client = mapper.Map<Client>(clientDTO);

            var result = clientRepository.Add(client);

            return result != null ?
                  CreatedAtAction(nameof(GetClient), client.Id)
                : NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, [FromBody] ClientDTO clientDTO)
        {
            var client = mapper.Map<Client>(clientDTO);

            var result = clientRepository.Update(id, client);

            return result ?
                  RedirectToAction(nameof(GetClient), id)
                : NoContent();
        }

        [HttpGet("{id},{ensureDeleting}")]
        public IActionResult Delete(int id, bool ensureDeleting)
        {
            var client = clientRepository.GetOne(id);
            if (client == null) return NotFound();

            clientRepository.Delete(client);

            return ensureDeleting ? RedirectToAction(nameof(ConfirmDelete), mapper.Map<ClientDTO>(client)) : NoContent();
        }


        [HttpDelete] 
        public IActionResult ConfirmDelete([FromBody] ClientDTO clientDTO)
        {
            var client = mapper.Map<Client>(clientDTO);

            bool clientAndHisAddressesAreRemoved = clientRepository.ConfirmStatesEntriesAreDeleted(client);

            if (clientAndHisAddressesAreRemoved)
                clientRepository.SaveChanges();

            return RedirectToAction(nameof(GetClients));
        }

    }
}
