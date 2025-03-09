using Roomly.Shared.Data.Entities;
using Roomly.Shared.Data.Enums;

namespace Roomly.Shared.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        var todayTime = DateTime.Today;
        var todayDate = new DateOnly(todayTime.Year, todayTime.Month, todayTime.Day);
        if (!context.Rooms.Any())
        {
            context.Rooms.AddRange(
            new Room
            {
                Id = Guid.NewGuid(),
                Name = "Conference Room A",
                Location = "2nd Floor, North Wing",
                Capacity = 12,
                Type = RoomType.MeetingRoom,
                Description = "Простора кімната для нарад із сучасними засобами зв'язку, ідеально підходить для відеоконференцій.",
                CreatedAt = todayDate
            },
            new Room
            {
                Id = Guid.NewGuid(),
                Name = "Quiet Zone",
                Location = "3rd Floor, South Wing",
                Capacity = 5,
                Type = RoomType.Workplace,
                Description = "Уютна зона для тихої роботи, де можна зосередитися і працювати без відволікань.",
                CreatedAt = todayDate
            },
            new Room
            {
                Id = Guid.NewGuid(),
                Name = "Brainstorming Room",
                Location = "1st Floor, Central Hub",
                Capacity = 8,
                Type = RoomType.MeetingRoom,
                Description = "Простір для креативних зустрічей і мозкових штурмів, обладнаний дошками та проектором.",
                CreatedAt = todayDate
            },
            new Room
            {
                Id = Guid.NewGuid(),
                Name = "Hot Desk Area",
                Location = "4th Floor, Open Space",
                Capacity = 20,
                Type = RoomType.Workplace,
                Description = "Відкрита зона для роботи в вільному форматі, доступна для всіх співробітників.",
                CreatedAt = todayDate
            },
            new Room
            {
                Id = Guid.NewGuid(),
                Name = "Executive Suite",
                Location = "5th Floor, Penthouse",
                Capacity = 6,
                Type = RoomType.MeetingRoom,
                Description = "Стильна кімната для зустрічей з важливими клієнтами або партнерами, обладнана для проведення презентацій.",
                CreatedAt = todayDate
            },
            new Room
            {
                Id = Guid.NewGuid(),
                Name = "Collaborative Zone",
                Location = "2nd Floor, East Wing",
                Capacity = 10,
                Type = RoomType.Workplace,
                Description = "Зона для командної роботи, що підтримує спілкування та співпрацю, з доступом до спільних інструментів.",
                CreatedAt = todayDate
            });
            
            context.SaveChanges();
        }

        if (!context.AvailableSlots.Any())
        {
            var rooms = context.Rooms.ToList();
            var workDayStart = new DateTime(todayDate.Year, todayDate.Month, todayDate.Day, 9, 0, 0);
            var workDayEnd = new DateTime(todayDate.Year, todayDate.Month, todayDate.Day, 18, 0, 0);

            foreach (var room in rooms)
            {
                for (var time = workDayStart; time < workDayEnd; time = time.AddHours(1))
                {
                    context.AvailableSlots.Add(new AvailableSlot
                    {
                        Id = Guid.NewGuid(),
                        RoomId = room.Id,
                        StartTime = time,
                        EndTime = time.AddHours(1),
                        IsAvailable = true
                    });
                }
            }

            context.SaveChanges();
        }
    }
}