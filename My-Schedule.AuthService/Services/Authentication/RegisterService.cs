using My_Schedule.AuthService.Core;
using My_Schedule.AuthService.DTO.Authentication;
using My_Schedule.AuthService.DTO.Confirmations;
using My_Schedule.AuthService.Models.Confirmations;
using My_Schedule.AuthService.Services.Confirmations;
using My_Schedule.AuthService.Services.Users;
using My_Schedule.Shared.Helpers.Validators;
using My_Schedule.Shared.Services.Users.Users;

namespace My_Schedule.AuthService.Services.Auth.Authentication
{
    public class RegisterService
    {
        private readonly HashService _hashService;
        private readonly UserService _userService;
        private readonly EmailConfirmationService _emailConfirmationService;
        private readonly AuthServiceContext _dbContext;

        public RegisterService(AuthServiceContext dbContext, EmailConfirmationService emailConfirmationService, HashService hashService, UserService userservice)
        {
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _userService = userservice ?? throw new ArgumentNullException(nameof(userservice));
            _emailConfirmationService = emailConfirmationService ?? throw new ArgumentNullException(nameof(emailConfirmationService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ConfirmationCodeResponse> register(RegisterDTO registerDTO)
        {
            if (InputValidator.IsValidInput(registerDTO.Username) && EmailValidator.IsValidEmail(registerDTO.Email) && PasswordValidator.IsValidPassword(registerDTO.Password))
            {
                if (!await UserCheckService.CheckIfEmailExists(registerDTO.Email, _dbContext))
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