using System;
using System.Collections.Generic;
using System.Linq;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.Validators.Location;

namespace MeAgendaAi.Service.Services
{
    public class LocationService : BaseService<Location>, ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository) : base(locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public List<Location> CreateLocationsFromModel(List<AddLocationModel> models, Guid userId)
        {
            var locations = new List<Location>();
            if (models != null)
                models.ForEach(model =>
                {
                    var location = new Location
                    {
                        LocationId = Guid.NewGuid(),
                        UserId = userId,
                        Name = model.Name,
                        Country = model.Country,
                        State = model.State,
                        City = model.City,
                        Neighbourhood = model.Neighbourhood,
                        Street = model.Street,
                        Number = model.Number,
                        Complement = model.Complement,
                        CEP = model.CEP,
                        CreatedAt = DateTimeUtil.UtcToBrasilia(),
                        LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                        UpdatedBy = userId
                    };
                    locations.Add(location);
                });

            return locations;
        }

        public ResponseModel ValidateAddLocations(List<AddLocationModel> locations)
        {
            var response = new ResponseModel { Success = true };

            locations.ForEach(location =>
            {
                var validate = new AddLocationModelValidator().Validate(location);
                if (!validate.IsValid) response.Result = validate.Errors.FirstOrDefault();
            });

            return response;
        }

        public string GetCompletLocation(Location location)
        {
            var completeLocation = location.State != null && location.State != string.Empty ? $"{location.State}," : "";
            completeLocation += location.City != null && location.City != string.Empty ? $"{location.City}," : "";
            completeLocation += location.Neighbourhood != null && location.Neighbourhood != string.Empty
                ? $"{location.Neighbourhood},"
                : "";
            completeLocation += location.Street != null && location.Street != string.Empty ? $"{location.Street}," : "";
            completeLocation += location.Number != 0 ? $"{location.Number}" : "";
            completeLocation += location.Complement != null && location.Complement != string.Empty
                ? $", {location.Complement}"
                : "";

            return completeLocation;
        }

        public List<LocationPerfilModel> UserLocationsToBasicLocationModel(Guid userId)
        {
            var locationPerfilModels = new List<LocationPerfilModel>();

            var locations = _locationRepository.GetLocationsByUserId(userId);
            if (locations != null)
                locations.ForEach(location =>
                {
                    var model = new LocationPerfilModel
                    {
                        LocationId = location.LocationId.ToString(),
                        Name = location.Name,
                        CompleteLocation = GetCompletLocation(location),
                        Country = location.Country,
                        State = location.State,
                        City = location.City,
                        Neighbourhood = location.Neighbourhood,
                        Street = location.Street,
                        Number = location.Number,
                        Complement = location.Complement,
                        CEP = location.CEP
                    };

                    locationPerfilModels.Add(model);
                });

            return locationPerfilModels;
        }
    }
}