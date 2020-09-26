using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.EpModels
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            Success = false;
            Result = string.Empty;
        }
        public bool Success { get; set; }
        public object Result { get; set; }
    }
}
