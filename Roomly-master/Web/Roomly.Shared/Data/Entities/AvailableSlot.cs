namespace Roomly.Shared.Data.Entities;

public class AvailableSlot
{
    public Guid Id { get; set; }
    public Room Room { get; set; }
    public Guid RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsAvailable { get; set; }
}