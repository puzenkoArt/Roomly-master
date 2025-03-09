using Roomly.Shared.Data.Enums;

namespace Roomly.Shared.Data.Entities;

public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int Capacity { get; set; }
    public RoomType Type { get; set; }
    public string Description { get; set; }
    public DateOnly CreatedAt { get; set; }
}