using System;
using System.Net;
using System.Threading.Tasks;
using MeAgendaAi.Domain.EpModels.Client;
using MeAgendaAi.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeAgendaAi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AddClient")]
        public async Task<ActionResult> AddClient([FromBody] AddClientModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _clientService.AddClient(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        [Route("EditClient")]
        public ActionResult EditClient([FromForm] EditClientModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _clientService.EditClient(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Cliente")]
        [Route("GetClientInfoPerfil/{userId}")]
        public ActionResult GetClientInfoPerfil(string userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _clientService.GetClientPerfilInfo(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Cliente")]
        [Route("GetClientFavoriteEmployees/{userId}")]
        public ActionResult GetClientFavoriteEmployees(string userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _clientService.GetClientFavoriteEmployees(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}