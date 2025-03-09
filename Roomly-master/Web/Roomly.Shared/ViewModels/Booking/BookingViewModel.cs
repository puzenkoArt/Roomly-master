using Roomly.Shared.Data.Enums;

namespace Roomly.Booking.ViewModels;

public class BookingViewModel
{
    public Guid BookingId { get; set; }
    public Guid RoomId { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public string RoomName { get; set; }
    public string RoomLocation { get; set; }
    public BookingStatus Status { get; set; }
    public int RoomCapacity { get; set; }
    public RoomType RoomType { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}