using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Location
{
    public class AddLocationModel
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Neighbourhood { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string CEP { get; set; }
    }
}
