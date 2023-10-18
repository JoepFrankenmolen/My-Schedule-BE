using My_Schedule.AuthService.Models.Confirmations;

namespace My_Schedule.AuthService.DTO.Authentication
{
    public class ConfirmationDTO
    {
        public Confirmation Confirmation { get; set; }
        public string Code { get; set; }
    }
}