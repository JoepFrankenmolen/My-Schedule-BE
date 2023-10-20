﻿using My_Schedule.Shared.Models.ClientDetails;
using SecureLogin.Data.Models.Tokens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.AuthService.Models.Tokens
{
    public class TokenSession : ITokenStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public long CreationTimestamp { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public Guid ClientDetailsId { get; set; }

        public virtual ClientDetails ClientDetails { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        public long? BlockedTimestamp { get; set; }
    }
}