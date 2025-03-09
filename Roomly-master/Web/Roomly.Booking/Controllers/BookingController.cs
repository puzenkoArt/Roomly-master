using Microsoft.AspNetCore.Mvc;
using Roomly.Booking.Services;
using Roomly.Booking.ViewModels;
using Roomly.Users.Infrastructure.Exceptions;

namespace Roomly.Booking.Controllers;

[ApiController]
[Route("/api/bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly UserService _userService;

    public BookingController(
        IBookingService bookingService,
        UserService userService)
    {
        _bookingService = bookingService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(BookingCreateViewModel bookingViewModel)
    {
        try
        {
            var userId = _userService.GetUserId();
            
            await _bookingService.CreateBookingAsync(bookingViewModel, userId);
            
            return Ok();
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetBookings()
    {
        try 
        {
            var userId = _userService.GetUserId();
            
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
                
            return Ok(bookings);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{bookingId:guid}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid bookingId)
    {
        try
        {
           await _bookingService.CancelBookingAsync(bookingId);
            
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}