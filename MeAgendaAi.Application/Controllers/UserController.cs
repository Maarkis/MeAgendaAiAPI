using System;
using System.Net;
using System.Threading.Tasks;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                return Ok(_userService.UserVerified(id));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [AuthorizeRoles(Roles.Cliente, Roles.UsuarioEmpresa, Roles.Funcionario, Roles.Admin)]
        [Route("EditName")]
        public ActionResult EditName([FromBody] RequestEditName model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                return Ok(_userService.EditName(model));
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
            if (!ModelState.IsValid) return BadRequest(ModelState);
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
            if (!ModelState.IsValid) return BadRequest(ModelState);
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

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
        [Route("Account/{id}")]
        public ActionResult Account(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(_userService.Account(id));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [AuthorizeRoles(Roles.Admin)]
        [Route("GetById/{id}")]
        public ActionResult GetById(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(_userService.GetById(id));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [AuthorizeRoles(Roles.Cliente, Roles.UsuarioEmpresa, Roles.Funcionario, Roles.Admin)]
        [Route("AddUserImage")]
        public ActionResult AddUserImage([FromForm] AddUserImageModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                return Ok(_userService.AddUserImage(model));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}