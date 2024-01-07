namespace My_Schedule.NotificationService.Models.DTO
{
    public class FilteredTableNotificationDto
    {
        public IEnumerable<TableNotificationDto> Notifications { get; set; }
        public int NotificationTotal { get; set; }
    }
}