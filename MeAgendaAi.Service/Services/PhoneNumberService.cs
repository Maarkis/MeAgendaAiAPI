using System;
using System.Collections.Generic;
using System.Linq;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.PhoneNumber;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.Validators.PhoneNumber;

namespace MeAgendaAi.Service.Services
{
    public class PhoneNumberService : BaseService<PhoneNumber>, IPhoneNumberService
    {
        private readonly IPhoneNumberRepository _phoneNumberRepository;

        public PhoneNumberService(IPhoneNumberRepository phoneNumberRepository) : base(phoneNumberRepository)
        {
            _phoneNumberRepository = phoneNumberRepository;
        }

        public List<PhoneNumber> CreatePhoneNumbersFromModel(List<AddPhoneNumberModel> models, Guid userId)
        {
            var phoneNumbers = new List<PhoneNumber>();
            if (models != null && models.Count > 0)
                models.ForEach(phone =>
                {
                    var phoneNumber = new PhoneNumber
                    {
                        PhoneNumberId = Guid.NewGuid(),
                        UserId = userId,
                        NameContact = phone.NameContact,
                        CountryCode = phone.CountryCode,
                        DDD = phone.DDD,
                        Number = phone.Number,
                        CreatedAt = DateTimeUtil.UtcToBrasilia(),
                        LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                        UpdatedBy = userId
                    };
                    phoneNumbers.Add(phoneNumber);
                });

            return phoneNumbers;
        }

        public ResponseModel ValidateAddPhoneNumbers(List<AddPhoneNumberModel> phones)
        {
            var response = new ResponseModel { Success = true };

            phones.ForEach(phone =>
            {
                var validate = new AddPhoneNumberModelValidator().Validate(phone);
                if (!validate.IsValid) response.Result = validate.Errors.FirstOrDefault();
            });

            return response;
        }

        public string GetCompletePhoneNumber(PhoneNumber phoneNumber)
        {
            var completePhoneNumber = phoneNumber.CountryCode != 0 ? $"+{phoneNumber.CountryCode} " : string.Empty;
            completePhoneNumber += phoneNumber.DDD != 0 ? $" ({phoneNumber.DDD})" : string.Empty;
            completePhoneNumber += phoneNumber.Number != null && phoneNumber.Number != string.Empty
                ? $" {phoneNumber.Number}"
                : string.Empty;

            return completePhoneNumber;
        }

        public List<PhoneNumberPerfilModel> UserPhoneNumbersToPhoneNumberModel(Guid userId)
        {
            var phoneNumberPerfilModels = new List<PhoneNumberPerfilModel>();

            var phoneNumbers = _phoneNumberRepository.GetByUserID(userId);
            if (phoneNumbers != null)
                phoneNumbers.ForEach(phoneNumber =>
                {
                    var phoneNumberPerfilModel = new PhoneNumberPerfilModel
                    {
                        PhoneNumberId = phoneNumber.PhoneNumberId.ToString(),
                        NameContact = phoneNumber.NameContact,
                        CompletePhoneNumber = GetCompletePhoneNumber(phoneNumber),
                        CountryCode = phoneNumber.CountryCode,
                        DDD = phoneNumber.DDD,
                        Number = phoneNumber.Number
                    };

                    phoneNumberPerfilModels.Add(phoneNumberPerfilModel);
                });

            return phoneNumberPerfilModels;
        }
    }
}