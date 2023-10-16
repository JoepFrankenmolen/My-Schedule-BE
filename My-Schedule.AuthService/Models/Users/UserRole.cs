﻿using My_Schedule.Shared.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace My_Schedule.AuthService.Models.Users
{
    public class UserRole : IUserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public UserRoleType Role { get; set; }
    }
}
