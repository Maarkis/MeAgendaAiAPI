using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.PhoneNumber;

namespace MeAgendaAi.Domain.Interfaces.Services
{
    public interface IPhoneNumberService : IBaseService<PhoneNumber>
    {
        List<PhoneNumber> CreatePhoneNumbersFromModel(List<AddPhoneNumberModel> models, Guid userId);
        ResponseModel ValidateAddPhoneNumbers(List<AddPhoneNumberModel> phones);
        string GetCompletePhoneNumber(PhoneNumber phoneNumber);
        List<PhoneNumberPerfilModel> UserPhoneNumbersToPhoneNumberModel(Guid userId);
    }
}