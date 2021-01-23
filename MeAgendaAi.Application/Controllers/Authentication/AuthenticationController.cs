using Microsoft.AspNetCore.Authorization;
using MeAgendaAi.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.EpModels;
using System.Threading.Tasks;

namespace MeAgendaAi.Application.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        //public async Task<object> Login()
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ResponseModel resp = _userService.Login(model);
                if (!resp.Success)
                {
                    return BadRequest(resp);
                }
                return Ok(resp);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<ActionResult> RecoverPassword([FromBody] RecoveryPassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ResponseModel resp = await _userService.RetrievePassword(model);
                if(!resp.Success)
                {
                    return BadRequest(resp);
                }                
                return Ok(resp);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [AllowAnonymous]
        [HttpPut]
        [Route("ConfirmationEmail")]
        public ActionResult ConfirmationEmail(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ResponseModel resp = _userService.ConfirmationEmail(id);
                if (!resp.Success)
                {
                    return BadRequest(resp);
                }
                return Ok(resp);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [AllowAnonymous]
        [HttpPut]
        [Route("ResetPassword")]
        public ActionResult ResetPassword([FromBody] ResetPassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ResponseModel resp = _userService.ResetPassword(model);
                if (!resp.Success)
                {
                    return BadRequest(resp);
                }
                return Ok(resp);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
