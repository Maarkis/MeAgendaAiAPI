using System;
using System.Net;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.EpModels.Scheduling;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeAgendaAi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulingController : ControllerBase
    {
        private readonly ISchedulingService _schedulingService;

        public SchedulingController(ISchedulingService schedulingService)
        {
            _schedulingService = schedulingService;
        }

        [HttpPost]
        [Authorize(Roles = "Cliente,UsuarioEmpresa,Funcionario")]
        [Route("CreateScheduling")]
        public ActionResult CreateScheduling([FromBody] CreateSchedulingModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _schedulingService.CreateScheduling(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [AuthorizeRoles(Roles.Cliente, Roles.UsuarioEmpresa, Roles.Funcionario)]
        [Route("GetClientSchedulingsByUserId/{userId}")]
        public ActionResult GetClientSchedulingsByUserId(string userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _schedulingService.GetClientSchedulings(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [AuthorizeRoles(Roles.Cliente, Roles.UsuarioEmpresa, Roles.Funcionario)]
        [Route("GetClientHistoricoSchedulingsByUserId/{userId}")]
        public ActionResult GetClientHistoricoSchedulingsByUserId(string userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _schedulingService.GetHistoricoClientSchedulings(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Cliente,UsuarioEmpresa,Funcionario")]
        [Route("GetEmployeeSchedulingsByUserId/{userId}")]
        public ActionResult GetEmployeeSchedulingsByUserId(string userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _schedulingService.GetEmployeeSchedulings(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        [HttpGet]
        [Authorize(Roles = "Cliente,UsuarioEmpresa,Funcionario")]
        [Route("GetHistoricoEmployeeSchedulingsByUserId/{userId}")]
        public ActionResult GetHistoricoEmployeeSchedulingsByUserId(string userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _schedulingService.GetHistoricoEmployeeSchedulings(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Cliente,UsuarioEmpresa,Funcionario")]
        [Route("UpdateSchedulingStatus")]
        public ActionResult UpdateSchedulingStatus(UpdateSchedulingStatusModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _schedulingService.UpdateSchedulingStatus(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}