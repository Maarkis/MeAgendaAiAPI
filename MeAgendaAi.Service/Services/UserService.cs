using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MeAgendaAi.Cryptography.Cryptography;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Entities.Email;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Interfaces.Services.Email;
using MeAgendaAi.Domain.Security;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.Validators.Authentication;
using MeAgendaAi.Domain.Validators.User;
using MeAgendaAi.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MeAgendaAi.Service.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _email;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILocationService _locationService;
        private readonly IPhoneNumberService _phoneNumberService;

        private readonly SigningConfiguration _signingConfiguration;
        private readonly TokenConfiguration _tokenConfiguration;
        private readonly IUserRepository _userRepository;

        public UserService(
            IEmailService email,
            IUserRepository userRepository,
            IClientRepository clientRepository,
            SigningConfiguration signingConfiguration,
            TokenConfiguration tokenConfiguration,
            ILocationService locationService,
            IConfiguration configuration,
            IPhoneNumberService phoneNumberService,
            IEmployeeRepository employeeRepository,
            ICompanyRepository companyRepository) : base(userRepository)
        {
            _email = email;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;
            _configuration = configuration;
            _locationService = locationService;
            _phoneNumberService = phoneNumberService;
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
        }

        public ResponseModel CreateUserFromModel(AddUserModel model, List<Roles> roles)
        {
            var resp = new ResponseModel();

            try
            {
                var userExist = _userRepository.GetByEmail(model.Email);
                if (userExist == null)
                {
                    // validate user
                    var validateUser = new AddUserModelValidator().Validate(model);
                    if (validateUser.IsValid)
                    {
                        if (model.Locations != null && model.Locations.Count > 0)
                        {
                            var validLocations = _locationService.ValidateAddLocations(model.Locations);
                            if (!validLocations.Success) return validLocations;
                        }

                        if (model.PhoneNumbers != null && model.PhoneNumbers.Count > 0)
                        {
                            var validPhoneNumbers = _phoneNumberService.ValidateAddPhoneNumbers(model.PhoneNumbers);
                            if (!validPhoneNumbers.Success) return validPhoneNumbers;
                        }

                        var userId = Guid.NewGuid();

                        string img = null;
                        if (model.Imagem != null)
                        {
                            var resultImg = DownloadImage(model.Imagem, userId.ToString()).Result;
                            if (resultImg.Success) img = resultImg.Result.ToString();
                        }

                        var newUser = new User
                        {
                            UserId = userId,
                            Email = model.Email,
                            Password = Encrypt.EncryptString(model.Password, userId.ToString()),
                            Name = model.Name,
                            Image = img,
                            Locations = _locationService.CreateLocationsFromModel(model.Locations, userId),
                            PhoneNumbers = _phoneNumberService.CreatePhoneNumbersFromModel(model.PhoneNumbers, userId),
                            CreatedAt = DateTimeUtil.UtcToBrasilia(),
                            LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                            Roles = GenerateUserRoles(roles, userId)
                        };
                        //_userRepository.Add(newUser);

                        resp.Success = true;
                        resp.Message = "Usuário adicionado com sucesso!";
                        resp.Result = newUser;
                    }
                    else
                    {
                        resp.Message = validateUser.Errors.FirstOrDefault().ErrorMessage;
                    }
                }
                else
                {
                    resp.Message = "Já existe um usuário utilizando este E-mail, por favor escolha outro E-mail.";
                }
            }
            catch (Exception e)
            {
                resp.Message = "Não foi possível adicionar o usuário";
            }

            return resp;
        }

        public ResponseModel EditUserFromModel(EditUserModel model)
        {
            var resp = new ResponseModel();

            try
            {
                // validate user
                var validateUser = new EditUserModelValidator().Validate(model);
                if (validateUser.IsValid)
                {
                    var user = _userRepository.GetById(Guid.Parse(model.UsuarioId));
                    if (user != null)
                    {
                        if (model.Locations != null && model.Locations.Count > 0)
                        {
                            var validLocations = _locationService.ValidateAddLocations(model.Locations);
                            if (!validLocations.Success) return validLocations;
                        }

                        if (model.PhoneNumbers != null && model.PhoneNumbers.Count > 0)
                        {
                            var validPhoneNumbers = _phoneNumberService.ValidateAddPhoneNumbers(model.PhoneNumbers);
                            if (!validPhoneNumbers.Success) return validPhoneNumbers;
                        }

                        if (model.Imagem != null)
                        {
                            var resultImg = DownloadImage(model.Imagem, user.UserId.ToString()).Result;
                            if (resultImg.Success) user.Image = resultImg.Result.ToString();
                        }

                        user.Name = model.Name;
                        user.Locations = _locationService.CreateLocationsFromModel(model.Locations, user.UserId);
                        user.PhoneNumbers =
                            _phoneNumberService.CreatePhoneNumbersFromModel(model.PhoneNumbers, user.UserId);
                        user.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();

                        resp.Success = true;
                        resp.Message = "Usuário adicionado com sucesso!";
                        resp.Result = user;
                    }
                    else
                    {
                        resp.Message = "Usuário não encontrado";
                    }
                }
                else
                {
                    resp.Message = validateUser.Errors.FirstOrDefault().ErrorMessage;
                }

                return resp;
            }
            catch (Exception e)
            {
                resp.Message = "Não foi possível editar o usuário";
            }

            return resp;
        }

        public ResponseModel Login(LoginModel model)
        {
            var resp = new ResponseModel();

            var validateLogin = new AuthenticationModelValidator().Validate(model);
            if (validateLogin.IsValid)
                try
                {
                    var user = _userRepository.GetByEmail(model.Email);
                    if (user != null)
                    {
                        if (!ValidatePassword(model.Senha, user))
                        {
                            resp.Success = false;
                            resp.Message = "Senha inválida";

                            return resp;
                        }


                        var secondaryId = GetSecondaryIdByUser(user);
                        var responseJWT = JWTService.GenerateToken(user, _signingConfiguration, _tokenConfiguration);
                        responseJWT.SecondaryId = secondaryId != null ? secondaryId.ToString() : string.Empty;
                        resp.Success = true;
                        resp.Message = "Login efetuado";
                        resp.Result = responseJWT;
                    }
                    else
                    {
                        resp.Message = "Usuário não cadastrado";
                    }
                }
                catch (Exception)
                {
                    resp.Message = "Não foi possível encontrar o usuário";
                }
            else
                resp.Message = validateLogin.Errors.FirstOrDefault().ErrorMessage;

            return resp;
        }


        public async Task<ResponseModel> RetrievePassword(RecoveryPassword model)
        {
            var response = new ResponseModel();
            var validateRecoveryEmail = new RecoveryEmailValidator().Validate(model);
            if (validateRecoveryEmail.IsValid)
                try
                {
                    var user = _userRepository.GetByEmail(model.Email);
                    if (user == null)
                    {
                        response.Message = "Usuário não encontrado.";

                        return response;
                    }

                    var emailRetrieve = new EmailRetrievePassword
                    {
                        FromEmail = "jeanmarkis85@hotmail.com",
                        FromName = "Me Agenda Aí",
                        Subject = "Link para alteração da sua senha do Me Agenda Aí",
                        Token = JWTService.GenerateTokenRecoverPassword(user, _signingConfiguration,
                            _tokenConfiguration),
                        Url = _configuration.GetValue<string>("URLPortal"),
                        Expiration = (Convert.ToInt32(_tokenConfiguration.Seconds) / (60 * 60)).ToString()
                    };


                    var resp = await _email.SendRecoveryPassword(user, emailRetrieve);
                    if (resp)
                    {
                        response.Success = resp;
                        response.Message = "E-mail enviado com sucesso.";
                    }
                    else
                    {
                        response.Message = "Não foi possível enviar o e-mail.";
                    }
                }
                catch (Exception)
                {
                    response.Message = "Erro no sistema, e-mail não enviado.";
                }
            else
                response.Message = validateRecoveryEmail.Errors.FirstOrDefault().ErrorMessage;

            return response;
        }


        public ResponseModel ResetPassword(ResetPassword model)
        {
            var response = new ResponseModel();
            var validateResetPassword = new ResetPasswordValidator().Validate(model);
            if (validateResetPassword.IsValid)
                try
                {
                    var validateToken =
                        JWTService.ValidateToken(model.Token, _signingConfiguration, _tokenConfiguration);
                    if (validateToken)
                    {
                        var user = _userRepository.GetById(model.Id);
                        if (user == null)
                        {
                            response.Message = "Usuário não encontrado";
                            return response;
                        }

                        user.Password = Encrypt.EncryptString(model.Password, user.UserId.ToString());
                        user.UpdatedBy = model.Id;
                        user.LastUpdatedAt = DateTime.Now;

                        _userRepository.Edit(user);


                        response.Success = true;
                        response.Message = "Senha alterada com sucesso";
                    }
                    else
                    {
                        response.Message = "Token expirado";
                    }
                }
                catch (Exception)
                {
                    response.Message = "Erro ao alterar senha, entre em contato com suporte";
                }
            else
                response.Message = validateResetPassword.Errors.FirstOrDefault().ErrorMessage;

            return response;
        }


        public ResponseModel UserVerified(Guid id)
        {
            var response = new ResponseModel();
            try
            {
                if (id == null)
                {
                    response.Message = "Id não informado";
                    return response;
                }

                var user = _userRepository.GetById(id);
                if (user == null)
                {
                    response.Message = "Usuário não encontrado";
                    return response;
                }

                response.Result = new
                {
                    UserVerified = user.Verified
                };
                response.Success = true;
                response.Message = "Usuário verificado!";
            }
            catch (Exception e)
            {
                throw e;
            }

            return response;
        }

        public async Task<ResponseModel> SendEmail(RequestResendEmail model)
        {
            var response = new ResponseModel();
            try
            {
                var resultValidator = new RequestResendEmailValidator().Validate(model);
                if (resultValidator.IsValid)
                    response = await SendEmailConfirmation(model.Email);
                else
                    response.Message = resultValidator.Errors.FirstOrDefault().ErrorMessage;
            }
            catch (Exception e)
            {
                throw e;
            }

            return response;
        }

        public ResponseModel ConfirmationEmail(Guid id)
        {
            var response = new ResponseModel();
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    response.Message = "Token não encontrado";
                    return response;
                }

                var user = _userRepository.GetById(id);
                if (user == null)
                {
                    response.Message = "Usuário não encontrado";
                    return response;
                }

                user.Verified = true;
                _userRepository.Edit(user);

                response.Message = "E-mail confirmado com sucesso";
                response.Success = true;
            }
            catch (Exception)
            {
                response.Message = "Erro ao confirmar e-mail, entre em contato com suporte";
            }

            return response;
        }


        public async Task<ResponseModel> SendEmailConfirmation(string email)
        {
            var response = new ResponseModel();
            try
            {
                var user = _userRepository.GetByEmail(email);
                if (user == null)
                {
                    response.Message = "Usuário não encotrado";
                    return response;
                }

                var emailConfirmation = new EmailConfirmation
                {
                    FromEmail = "jeanmarkis85@hotmail.com",
                    FromName = "Me Agenda Aí",
                    Subject = "Confirmar meu e-mail no Me Agenda Aí",
                    Url = _configuration.GetValue<string>("URLPortal")
                };
                var emailSent = await _email.SendEmailConfirmartion(user, emailConfirmation);
                if (emailSent)
                {
                    response.Success = true;
                    response.Message = "E-mail enviado com sucesso";
                }

                return response;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ResponseModel Account(string id)
        {
            var response = new ResponseModel();
            try
            {
                if (GuidUtil.IsGuidValid(id))
                {
                    var user = _userRepository.GetAccountById(Guid.Parse(id));
                    if (user == null)
                    {
                        response.Message = "Usuário não encotrado";
                        return response;
                    }

                    var responseAccount = new ResponseAccount
                    {
                        UserId = user.UserId,
                        Email = user.Email,
                        Image = user.Image,
                        Name = user.Name,
                        Roles = user.Roles,
                        PhoneNumbers = user.PhoneNumbers,
                        Locations = user.Locations,
                        CreateAt = user.CreatedAt
                    };
                    response.Success = true;
                    response.Message = "Informação básica da conta do usuário";
                    response.Result = responseAccount;
                    return response;
                }

                response.Message = "Guid inválido";
            }
            catch (Exception e)
            {
                throw e;
            }

            return response;
        }

        public ResponseModel EditName(RequestEditName model)
        {
            var response = new ResponseModel();
            var resultValidator = new RequestEditNameValidator().Validate(model);
            if (resultValidator.IsValid)
            {
                var user = _userRepository.GetById(model.Id);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                    user.UpdatedBy = user.UserId;
                    _userRepository.Edit(user);

                    response.Success = true;
                    response.Result = user.Name;
                    response.Message = "Imagem atualizada com sucesso!";
                }
                else
                {
                    response.Message = "Usuário não encontrado";
                }
            }
            else
            {
                response.Message = resultValidator.Errors.FirstOrDefault().ErrorMessage;
            }

            return response;
        }

        public ResponseModel AddUserImage(AddUserImageModel model)
        {
            var responseModel = new ResponseModel();

            try
            {
                if (GuidUtil.IsGuidValid(model.UserId))
                {
                    var user = _userRepository.GetById(Guid.Parse(model.UserId));
                    if (user != null)
                    {
                        string img = null;
                        if (model.Image != null)
                        {
                            var resultImg = DownloadImage(model.Image, model.UserId).Result;
                            if (resultImg.Success) img = resultImg.Result.ToString();
                        }

                        user.Image = img;
                        user.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                        user.UpdatedBy = user.UserId;
                        _userRepository.Edit(user);
                        responseModel.Success = true;
                        responseModel.Result = img;
                        responseModel.Message = "Imagem atualizada com sucesso!";
                    }
                    else
                    {
                        responseModel.Message = "Usuário não encontrado";
                    }
                }
                else
                {
                    responseModel.Message = "Guid inválido";
                }
            }
            catch (Exception e)
            {
                responseModel.Message =
                    $"Não foi possível adicionar a imagem ao usuário. {e.Message} / {e.InnerException.Message}";
            }

            return responseModel;
        }

        public Guid? GetSecondaryIdByUser(User user)
        {
            Guid? secondaryId = null;
            if (user.Roles.Any(x => x.Role == Roles.UsuarioEmpresa))
                secondaryId = _companyRepository.GetCompanyByUserId(user.UserId)?.CompanyId;
            else if (user.Roles.Any(x => x.Role == Roles.Funcionario))
                secondaryId = _employeeRepository.GetEmployeeByUserId(user.UserId)?.EmployeeId;
            else if (user.Roles.Any(x => x.Role == Roles.Cliente))
                secondaryId = _clientRepository.GetClientByUserId(user.UserId)?.ClientId;

            return secondaryId;
        }

        private List<UserRole> GenerateUserRoles(List<Roles> roles, Guid userId)
        {
            var userRoles = new List<UserRole>();
            roles.ForEach(role =>
            {
                var userRole = new UserRole
                {
                    UserRoleId = Guid.NewGuid(),
                    UserId = userId,
                    Role = role,
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                    UpdatedBy = userId
                };

                userRoles.Add(userRole);
            });

            return userRoles;
        }

        private bool ValidatePassword(string password, User user)
        {
            return Encrypt.CompareComputeHash(password, user.UserId.ToString(), user.Password);
        }

        public async Task<ResponseModel> DownloadImage(IFormFile file, string userId)
        {
            var response = new ResponseModel();

            try
            {
                //string Dir = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var dir = Directory.GetCurrentDirectory();
                dir = dir.Replace("MeAgendaAi.Application", "MeAgendaAi.Domain");
                var insideDir = "/Assets/UserImages/";
                var path = dir + insideDir;


                var subs = file.FileName.Split('.');
                var fileName = $"{userId}.{subs[1]}";

                var filePath = Path.Combine(path, fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                //using (var ms = new MemoryStream())
                //{
                //    model.Imagem.CopyTo(ms);
                //    var fileBytes = ms.ToArray();
                //    sucesso = _s3AppService.UploadMidia(path, fileBytes, true);
                //}

                response.Success = true;
                response.Result = "MeAgendaAiAPI/MeAgendaAi.Domain" + insideDir + fileName;
            }
            catch (Exception e)
            {
                response.Message = "Erro ao adicionar a imagem";
                response.Result = $"{e.Message}";
            }

            return response;
        }
    }
}