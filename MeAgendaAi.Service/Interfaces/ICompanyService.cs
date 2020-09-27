﻿using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.Company;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Interfaces
{
    public interface ICompanyService : IBaseService<Company>
    {
        ResponseModel AddCompany(AddCompanyModel model);
    }
}