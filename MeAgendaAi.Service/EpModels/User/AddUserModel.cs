﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.EpModels.User
{
    public class AddUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
    }
}