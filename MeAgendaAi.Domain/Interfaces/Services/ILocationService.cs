using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces.Services
{
    public interface ILocationService : IBaseService<Location>
    {
        List<Location> CreateLocationsFromModel(List<AddLocationModel> models, Guid userId);
        ResponseModel ValidateAddLocations(List<AddLocationModel> locations);
    }
}
