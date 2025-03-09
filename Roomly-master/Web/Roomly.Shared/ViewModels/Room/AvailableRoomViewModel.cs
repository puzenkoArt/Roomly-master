namespace Roomly.Rooms.ViewModels;

public class AvailableRoomViewModel
{
    public Guid RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; }
}