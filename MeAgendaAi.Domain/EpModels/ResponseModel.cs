using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            Success = false;
            Result = string.Empty;
            Message = string.Empty;
        }
        public bool Success { get; set; }
        public object Result { get; set; }
        public string Message { get; set; }
    }
}
