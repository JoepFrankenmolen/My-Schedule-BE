using My_Schedule.AuthService.DTO.Confirmations;
using My_Schedule.AuthService.Models.Confirmations;

namespace My_Schedule.AuthService.DTO.Authentication
{
    // Used when a code has been generated. This is sent to the user using the api and the code is send using email.
    public class ConfirmationCodeResponse
    {
        public ConfirmationType Type { get; set; }
        public ConfirmationCodeType CharachterType { get; set; }
        public string ConfirmationId { get; set; }
    }
}