using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.PhoneNumber;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Validators.PhoneNumber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeAgendaAi.Service.Services
{
    public class PhoneNumberService : BaseService<PhoneNumber>, IPhoneNumberService
    {
        private IPhoneNumberRepository _phoneNumberRepository;

        public PhoneNumberService(IPhoneNumberRepository phoneNumberRepository) : base(phoneNumberRepository)
        {
            _phoneNumberRepository = phoneNumberRepository;
        }

        public List<PhoneNumber> CreatePhoneNumbersFromModel(List<AddPhoneNumberModel> models, Guid userId)
        {
            List<PhoneNumber> phoneNumbers = new List<PhoneNumber>();

            models.ForEach(phone => {
                PhoneNumber phoneNumber = new PhoneNumber { 
                    PhoneNumberId = Guid.NewGuid(),
                    UserId = userId,
                    NameContact = phone.NameContact,
                    CountryCode = phone.CountryCode,
                    DDD = phone.DDD,
                    Number = phone.Number,
                    CreatedAt = Domain.Utils.DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = Domain.Utils.DateTimeUtil.UtcToBrasilia(),
                    UpdatedBy = userId
                };
                phoneNumbers.Add(phoneNumber);
            });

            return phoneNumbers;
        }

        public ResponseModel ValidateAddPhoneNumbers(List<AddPhoneNumberModel> phones)
        {
            ResponseModel response = new ResponseModel { Success = true };

            phones.ForEach(phone => {
                var validate = new AddPhoneNumberModelValidator().Validate(phone);
                if (!validate.IsValid)
                {
                    response.Result = validate.Errors.FirstOrDefault();
                }
            });

            return response;
        }
    }
}
