using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.Company;
using MeAgendaAi.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Services
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {
        private ICompanyRepository _companyRepository;
        private IUserRepository _userRepository;
        private IEmployeeRepository _employeeRepository;
        public CompanyService(ICompanyRepository companyRepository, IUserRepository userRepository, IEmployeeRepository employeeRepository) : base(companyRepository)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
        }

        public ResponseModel AddCompany(AddCompanyModel model)
        {
            var resp = new ResponseModel();

            try
            {
                var userManager = _userRepository.GetById(Guid.Parse(model.ManagerUserId));
                if(userManager == null)
                {
                    resp.Result = "Não foi possível encontrar o usuário";
                    return resp;
                }

                Company newCompany = new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = model.Name,
                    CPF = model.CPF,
                    CNPJ = model.CNPJ,
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
                };
                _companyRepository.Add(newCompany);
                Employee employee = new Employee
                {
                    UserId = userManager.UserId,
                    CompanyId = newCompany.CompanyId,
                    IsManager = true,
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
                };
                _employeeRepository.Add(employee);

                resp.Success = true;
                resp.Result = "Empresa adicionada com sucesso";
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível adicionar a empresa";
            }

            return resp;
        }
    }
}
