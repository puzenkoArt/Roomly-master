using MassTransit;
using Microsoft.EntityFrameworkCore;
using Roomly.Rooms.ViewModels;
using Roomly.Shared.Data;

namespace Roomly.Rooms.Infrastructure.Rabbit;

public class RoomAvailabilityConsumer: IConsumer<AvailabilityRoomViewModel>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<RoomAvailabilityConsumer> _logger;

    public RoomAvailabilityConsumer(
        ApplicationDbContext dbContext,
        ILogger<RoomAvailabilityConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AvailabilityRoomViewModel> context)
    {
        var request = context.Message;

        bool isAvailable = await _dbContext.AvailableSlots
            .AnyAsync(slot => slot.RoomId == request.RoomId &&
                              slot.StartTime == request.StartTime &&
                              slot.EndTime == request.EndTime &&
                              slot.IsAvailable == true);

        _logger.LogInformation($"Availability check for Room {request.RoomId}: {isAvailable}");

        await context.RespondAsync(new AvailabilityResponse { IsAvailable = isAvailable });
    }
}