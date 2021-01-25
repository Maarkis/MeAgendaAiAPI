using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.EpModels.Employee;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using MeAgendaAi.Domain.EpModels.EmployeeWorkHours;
using Microsoft.AspNetCore.Authorization;
using MeAgendaAi.Domain.Enums;

namespace MeAgendaAi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AddEmployee")]
        public ActionResult AddEmployee([FromBody] AddEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _employeeService.AddEmployee(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Funcionario,UsuarioEmpresa")]
        [Route("EditEmployee")]
        public ActionResult EditEmployee([FromBody] EditEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _employeeService.EditEmployee(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Funcionario,UsuarioEmpresa,Cliente")]
        [Route("GetEmployeeServices/{employeeId}")]
        public ActionResult GetEmployeeServices(string employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _employeeService.GetEmployeeServices(employeeId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Funcionario")]
        [Route("GetEmployeePerfilInfo/{userId}")]
        public ActionResult GetEmployeePerfilInfo(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _employeeService.GetEmployeePerfilInfo(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles = "Funcionario,UsuarioEmpresa,Cliente")]
        [Route("GetEmployeeInfoComplete/{employeeId}")]
        public ActionResult GetEmployeeInfoComplete(string employeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _employeeService.GetEmployeeInfo(employeeId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Funcionario,UsuarioEmpresa")]
        [Route("AddServiceToEmployee")]
        public ActionResult AddServiceToEmployee(AddServiceToEmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _employeeService.AddServiceToEmployee(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Funcionario,UsuarioEmpresa")]
        [Route("AddWorkHoursToEmployee")]
        public ActionResult AddWorkHoursToEmployee([FromBody] AddEmployeeWorkHoursModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _employeeService.AddWorkHoursToEmployee(model, User.Identity.Name);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Funcionario,UsuarioEmpresa,Cliente")]
        [Route("GetEmployeeAvailableHours")]
        public ActionResult GetEmployeeAvailableHours(string employeeId, string serviceId, string date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _employeeService.GetEmployeeAvailableHours(employeeId, serviceId, date);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Funcionario,UsuarioEmpresa")]
        [Route("GetEmployeeMonthSchedule/{userId}/{ano}/{mes}")]
        public ActionResult GetEmployeeMonthSchedule(string userId, int ano, int mes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = _employeeService.GetEmployeeMonthSchedule(userId, ano, mes);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
