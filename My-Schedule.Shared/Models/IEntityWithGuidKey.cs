namespace My_Schedule.Shared.Models
{
    public interface IEntityWithGuidKey
    {
        Guid Id { get; set; }
    }
}