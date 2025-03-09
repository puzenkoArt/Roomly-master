namespace Roomly.Booking.ViewModels;

public class BookingCreateViewModel
{
    public Guid RoomId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}