using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.AddEmployee;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Interfaces
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        ResponseModel AddEmployee(AddEmployeeModel model);
    }
}
