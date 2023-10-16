using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_Schedule.Shared.Models
{
    public interface IEntityWithGuidKey
    {
        Guid Id { get; set; }
    }
}
