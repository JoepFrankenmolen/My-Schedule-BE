using SecureLogin.Data.Models.Tokens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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