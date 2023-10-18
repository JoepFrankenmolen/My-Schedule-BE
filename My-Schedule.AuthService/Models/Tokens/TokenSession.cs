using My_Schedule.AuthService.Models.ClientDetail;
using My_Schedule.Shared.Models.ClientDetails;
using SecureLogin.Data.Models.Tokens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.AuthService.Models.Tokens
{
    public class TokenSession: ITokenSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public long CreationTimestamp { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public IClientDetails ClientDetails { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        public long? BlockedTimestamp { get; set; }
    }
}