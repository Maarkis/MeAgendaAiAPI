using System;
using System.Net;
using MeAgendaAi.Domain.EpModels.Company;
using MeAgendaAi.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeAgendaAi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AddCompany")]
        public ActionResult AddCompany([FromBody] AddCompanyModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _companyService.AddCompany(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("EditCompany")]
        public ActionResult EditCompany([FromBody] EditCompanyModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _companyService.EditCompany(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "UsuarioEmpresa")]
        [Route("AddServicesInCompany")]
        public ActionResult AddServicesInCompany([FromBody] AddMultipleServicesModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _companyService.CreateServiceForCompany(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "UsuarioEmpresa,Funcionario,Cliente")]
        [Route("GetServicesFromCompany/{companyId}")]
        public ActionResult GetServicesFromCompany(string companyId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _companyService.GetCompanyServices(companyId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        //[Authorize(Roles = "UsuarioEmpresa,Funcionario,Cliente")]
        [Route("GetCompanyComplete")]
        public ActionResult GetCompanyComplete(string companyId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _companyService.GetCompanyInfo(companyId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        [HttpGet]
        [Authorize(Roles = "UsuarioEmpresa")]
        [Route("GetCompanyInfoPerfil")]
        public ActionResult GetCompanyInfoPerfil(string userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _companyService.GetCompanyInfoPerfil(userId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "UsuarioEmpresa")]
        [Route("UpdateCompanyPolicy")]
        public ActionResult UpdateCompanyPolicy(UpdatePolicyModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = _companyService.UpdatePolicy(model);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}