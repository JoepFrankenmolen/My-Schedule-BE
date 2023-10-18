namespace My_Schedule.Shared.Models.ClientDetails
{
    public interface IClientDetails
    {
        Guid Id { get; set; }
        string IPAddress { get; set; }
        string UserAgent { get; set; }
    }
}