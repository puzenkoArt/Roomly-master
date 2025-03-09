using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Roomly.Rooms.Services;
using Roomly.Rooms.ViewModels;
using Roomly.Users.Infrastructure.Exceptions;

namespace Roomly.Rooms.Controllers;

[ApiController]
[Route("/api/rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }
    
    [HttpPost]
    [Authorize(Roles="Administrator")]
    public async Task<IActionResult> CreateRoom(RoomViewModel roomViewModel)
    {
        try
        {
            await _roomService.CreateRoomAsync(roomViewModel);
            
            return Ok();
        }
        catch (EntityAlreadyExistsException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _roomService.GetRoomsAsync();
        
        return Ok(rooms);
    }
    
    [HttpGet("{roomId:guid}/slots")]
    public async Task<IActionResult> GetAvailableSlots(Guid roomId)
    {
        var rooms = await _roomService.GetAvailableSlotsByRoomIdAsync(roomId);
        
        return Ok(rooms);
    }
}