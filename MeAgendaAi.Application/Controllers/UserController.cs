using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.EpModels.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.Utils;

namespace MeAgendaAi.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [AuthorizeRoles(Roles.Cliente, Roles.UsuarioEmpresa, Roles.Funcionario, Roles.Admin)]
        [Route("ClientVerified")]
        public ActionResult ClientVerified(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(_userService.UserVerified(id));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [HttpPost]        
        [AuthorizeRoles(Roles.Cliente, Roles.UsuarioEmpresa, Roles.Funcionario, Roles.Admin)]
        [Route("SendEmailConfirmation")]
        public async Task<ActionResult> SendEmailConfirmation([FromBody] RequestResendEmail model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await _userService.SendEmail(model));
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
                return Ok(_userService.ConfirmationEmail(id));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [HttpGet]
        [AuthorizeRoles(Roles.Admin)]
        [Route("GetAll")]
        public ActionResult GetAll()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(_userService.GetAll());
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [AuthorizeRoles(Roles.Cliente, Roles.UsuarioEmpresa, Roles.Funcionario, Roles.Admin)]
        [Route("GetById/{id}")]
        public ActionResult GetById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(_userService.GetById(id));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
