using My_Schedule.Shared.Models.ClientDetails;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.Shared.Models.ClientDetails
{
    public class ClientDetails : IClientDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string IPAddress { get; set; }

        [Required]
        public string UserAgent { get; set; }
    }
}