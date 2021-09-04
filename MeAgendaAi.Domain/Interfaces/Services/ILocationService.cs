using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Location;

namespace MeAgendaAi.Domain.Interfaces.Services
{
    public interface ILocationService : IBaseService<Location>
    {
        List<Location> CreateLocationsFromModel(List<AddLocationModel> models, Guid userId);
        ResponseModel ValidateAddLocations(List<AddLocationModel> locations);
        string GetCompletLocation(Location location);
        List<LocationPerfilModel> UserLocationsToBasicLocationModel(Guid userId);
    }
}