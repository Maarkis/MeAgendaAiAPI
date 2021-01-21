using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Validators.Location;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Services
{
    public class LocationService : BaseService<Location>, ILocationService
    {
        private ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository) : base(locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public List<Location> CreateLocationsFromModel(List<AddLocationModel> models, Guid userId, Guid? companyId)
        {
            List<Location> locations = new List<Location>();
            if(models != null)
            {
                models.ForEach(model => {
                    var location = new Location
                    {
                        LocationId = Guid.NewGuid(),
                        UserId = userId,
                        CompanyId = companyId,
                        Name = model.Name,
                        Country = model.Country,
                        State = model.State,
                        City = model.City,
                        Neighbourhood = model.Neighbourhood,
                        Street = model.Street,
                        Number = model.Number,
                        Complement = model.Complement,
                        CEP = model.CEP,
                        CreatedAt = Domain.Utils.DateTimeUtil.UtcToBrasilia(),
                        LastUpdatedAt = Domain.Utils.DateTimeUtil.UtcToBrasilia(),
                        UpdatedBy = userId
                    };
                    locations.Add(location);
                });
            }
            
            return locations;
        }

        public ResponseModel ValidateAddLocations(List<AddLocationModel> locations)
        {
            ResponseModel response = new ResponseModel { Success = true};

            locations.ForEach(location => {
                var validate = new AddLocationModelValidator().Validate(location);
                if (!validate.IsValid)
                {
                    response.Result = validate.Errors.FirstOrDefault();
                }
            });

            return response;
        }
    }
}
