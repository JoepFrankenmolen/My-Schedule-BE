using My_Schedule.AuthService.Models.Confirmations;

namespace My_Schedule.AuthService.DTO.Authentication
{
    public class ConfirmDTO
    {
        public Guid ConfirmationId { get; set; }
        public ConfirmationType ConfirmationType { get; set; }
        public string Code { get; set; }
    }
}