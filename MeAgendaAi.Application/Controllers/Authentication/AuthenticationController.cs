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
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {              
                return Ok(_userService.Login(model));                            
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
                return Ok(await _userService.RetrievePassword(model));
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
                ;
                return Ok(_userService.ResetPassword(model));
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
