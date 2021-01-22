﻿using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeAgendaAi.Domain.Interfaces.Services.Email
{
    public interface IEmailService
    {
        Task<bool> SendRecoveryPassword(User user, string token);
        
    }
}