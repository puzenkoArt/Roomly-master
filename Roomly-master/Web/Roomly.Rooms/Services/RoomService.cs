using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Roomly.Rooms.ViewModels;
using Roomly.Shared.Data;
using Roomly.Shared.Data.Entities;
using Roomly.Users.Infrastructure.Exceptions;

namespace Roomly.Rooms.Services;

public interface IRoomService
{
    Task CreateRoomAsync(RoomViewModel roomViewModel);
    Task<List<RoomViewModel>> GetRoomsAsync();
    Task<List<AvailableRoomViewModel>> GetAvailableSlotsByRoomIdAsync(Guid roomId);
}

public class RoomService : IRoomService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<RoomService> _logger;
    private readonly IMapper _mapper;

    public RoomService(
        ApplicationDbContext dbContext,
        ILogger<RoomService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task CreateRoomAsync(RoomViewModel roomViewModel)
    {
        var room = await _dbContext.Rooms
            .FirstOrDefaultAsync(u => u.Name == roomViewModel.Name);
        if (room is not null)
            throw new EntityAlreadyExistsException();
        
        var roomEntity = _mapper.Map<Room>(roomViewModel);
        
        roomEntity.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        
        await _dbContext.Rooms.AddAsync(roomEntity);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"Room {roomViewModel.Name} has been created");
    }

    public async Task<List<RoomViewModel>> GetRoomsAsync()
    {
        var rooms = await _dbContext.Rooms
            .Select(room => new RoomViewModel()
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Location = room.Location,
                Type = room.Type,
                Description = room.Description
            })
            .ToListAsync();
        
        _logger.LogInformation($"Getted {rooms.Count} available rooms");
        
        return rooms;
    }

    public async Task<List<AvailableRoomViewModel>> GetAvailableSlotsByRoomIdAsync(Guid roomId)
    {
        var rooms = await _dbContext.AvailableSlots
            .Include(r => r.Room)
            .Where(slot => slot.RoomId == roomId && slot.IsAvailable == true)
            .OrderBy(slot => slot.StartTime)
            .Select(slot => new AvailableRoomViewModel
            {
                RoomId = slot.Room.Id,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                IsAvailable = slot.IsAvailable
            })
            .ToListAsync();
        
        _logger.LogInformation($"Getted {rooms.Count} available slot rooms");
        
        return rooms;
    }
}