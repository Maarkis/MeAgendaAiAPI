using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.EpModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using MeAgendaAi.Domain.EpModels.Client;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

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
        public async Task<ActionResult> AddClient([FromForm] AddClientModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ResponseModel result = await _clientService.AddClient(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("EditClient")]
        public ActionResult EditClient([FromForm] EditClientModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ResponseModel result = _clientService.EditClient(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        
        [HttpGet]
        [Authorize(Roles = "Cliente")]
        [Route("ClientVerified")]
        public ActionResult ClientVerified(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(_clientService.UserVerified(id));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        [Route("SendEmailConfirmation")]
        public async Task<ActionResult> SendEmailConfirmation([FromBody] RequestResendEmail model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _clientService.SendEmail(model));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [HttpPut]        
        [AllowAnonymous]
        [Route("ConfirmationEmail")]
        public ActionResult ConfirmationEmail(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(_clientService.ConfirmationEmail(id));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }
    }
}
