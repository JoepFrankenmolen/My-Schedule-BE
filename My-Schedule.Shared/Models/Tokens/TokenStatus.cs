using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SecureLogin.Data.Models.Tokens;

namespace My_Schedule.Shared.Models.Tokens
{
    public class TokenStatus : ITokenStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        public long? BlockedTimestamp { get; set; }
    }
}
