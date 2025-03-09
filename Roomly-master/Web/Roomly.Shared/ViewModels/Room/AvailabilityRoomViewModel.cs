namespace Roomly.Rooms.ViewModels;

public class AvailabilityRoomViewModel
{
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}