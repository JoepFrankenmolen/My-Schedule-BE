﻿using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.DTO.Confirmations;
using My_Schedule.AuthService.Models.Confirmations;
using My_Schedule.AuthService.Services.Confirmations;
using My_Schedule.AuthService.Services.Users;
using My_Schedule.Shared.Helpers.Validators;

namespace My_Schedule.AuthService.Services.Auth.Authentication
{
    public class RegisterService
    {
        private readonly HashService _hashService;
        private readonly UserHelper _userHelper;
        private readonly UserService _userService;
        private readonly EmailConfirmationService _emailConfirmationService;

        public RegisterService(EmailConfirmationService emailConfirmationService, UserHelper userHelper, HashService hashService, UserService userservice)
        {
            _userHelper = userHelper ?? throw new ArgumentNullException(nameof(userHelper));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _userService = userservice ?? throw new ArgumentNullException(nameof(userservice));
            _emailConfirmationService = emailConfirmationService ?? throw new ArgumentNullException(nameof(emailConfirmationService));
        }

        public async Task<ConfirmationCodeResponse> register(RegisterDTO registerDTO)
        {
            if (InputValidator.IsValidInput(registerDTO.Username) && EmailValidator.IsValidEmail(registerDTO.Email) && PasswordValidator.IsValidPassword(registerDTO.Password))
            {
                if (!await _userHelper.CheckIfEmailExists(registerDTO.Email))
                {
                    var hashDTO = await _hashService.GenerateSaltAndHash(registerDTO.Password);

                    var user = await _userService.CreateUser(registerDTO, hashDTO);

                    var confirmationId = await _emailConfirmationService.CreateEmailConfirmation(user);

                    return new ConfirmationCodeResponse
                    {
                        ConfirmationId = confirmationId.ToString(),
                        CharachterType = ConfirmationCodeType.INT,
                        Type = ConfirmationType.EmailConfirmation
                    };
                }
            }

            throw new ArgumentException();
        }
    }
}