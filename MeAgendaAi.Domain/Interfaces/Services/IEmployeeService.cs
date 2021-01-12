﻿using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        ResponseModel AddEmployee(AddEmployeeModel model);
        ResponseModel GetEmployeeServices(string employeeId);
        ResponseModel AddServiceToEmployee(AddServiceToEmployeeModel model);
    }
}