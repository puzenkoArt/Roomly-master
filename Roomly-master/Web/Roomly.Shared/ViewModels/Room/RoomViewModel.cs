using Roomly.Shared.Data.Enums;

namespace Roomly.Rooms.ViewModels;

public class RoomViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int Capacity { get; set; }
    public RoomType Type { get; set; }
    public string Description { get; set; } 
}