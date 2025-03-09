using AutoMapper;
using Roomly.Rooms.ViewModels;
using Roomly.Shared.Data.Entities;

namespace Roomly.Users.Infrastructure.Mappings;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<RoomViewModel, Room>();
    }
}