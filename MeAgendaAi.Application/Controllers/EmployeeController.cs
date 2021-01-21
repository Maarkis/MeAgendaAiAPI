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

        [HttpGet]
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

        [HttpPost]
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
        [Authorize(Roles = "Funcionario")]
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
    }
}
