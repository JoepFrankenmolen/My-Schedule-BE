using My_Schedule.AuthService.Models.ClientDetail;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.AuthService.Models.Tokens
{
    public class TokenSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public long CreationTimeStamp { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public ClientDetails clientDetails { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        public long? BlockedTimeStamp { get; set; }
    }
}