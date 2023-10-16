using System.ComponentModel.DataAnnotations;

namespace My_Schedule.Shared.Models.ClientDetails
{
    public interface IClientDetails: IEntityWithGuidKey
    {
        string IPAddress { get; set; }
        string UserAgent { get; set; }
    }
}
