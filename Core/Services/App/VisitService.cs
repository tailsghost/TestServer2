using Kurskcartuning.Server_v2.Core.DbContext;
using Kurskcartuning.Server_v2.Core.Dtos.App;
using Kurskcartuning.Server_v2.Core.Dtos.App.Visit;
using Kurskcartuning.Server_v2.Core.Entities.AppDB;
using Kurskcartuning.Server_v2.Core.Entities.AppDB.VisitInside;
using Kurskcartuning.Server_v2.Core.Interfaces.App;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Services.App;

public class VisitService : IVisitService
{
    private readonly ApplicationDbContext _context;

    public VisitService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GeneralAppServiceResponceDto> PostVisit(ClaimsPrincipal User, VisitPostDto dto, string id)
    {

        var oneScreenMegaByte = 1 * 1024 * 1024;
        var fiveFileMegaByte = 5 * 1024 * 1024;
        var fileMimeType = "application/octet-stream";
        var screenMimeType = "image/jpeg";

        foreach (var screen in dto.ScreenShots)
        {
            if (screen != null && screen.Length > oneScreenMegaByte || screen?.ContentType != screenMimeType)
                return new GeneralAppServiceResponceDto
                {
                    IsSucced = false,
                    Message = "Неверный формат скриншотов или их размер",
                    StatusCode = 500,
                };
        }

        if (dto.FileNew != null && dto.FileNew.Length > fiveFileMegaByte && dto.FileOld != null && dto.FileOld.Length > fiveFileMegaByte || dto.FileNew?.ContentType != fileMimeType && dto.FileOld?.ContentType != fileMimeType)
            return new GeneralAppServiceResponceDto
            {
                IsSucced = false,
                Message = "Неверный формат файлов или их размер",
                StatusCode = 500,
            };

        var vehicle = await _context.Vehicles.Include(client => client.Client).FirstOrDefaultAsync(vehicle => vehicle.Id.Equals(dto.VehicleId) && vehicle.Client.UserId.Equals(id) && vehicle.ClientId.Equals(dto.CLientId));
        if (vehicle is null)
            return new GeneralAppServiceResponceDto
            {
                IsSucced = false,
                StatusCode = 401,
                Message = "Авто или клиент не найден!"
            };

        var screenshot = new List<ScreenShot>();
        var malfunctions = new List<Malfunction>();
        var suggestions = new List<Suggestion>();
        var listOfWorks = new List<ListOfWorks>();
        string? fileNewName = null;
        string? fileOldName = null;

        if (dto.ScreenShots.Count > 0)
        {
            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/{"files"}/{vehicle.Client.Phone}/{vehicle.RegistrationNumber}/{"screens"}");
            foreach (var screen in dto.ScreenShots)
            {
                var screenName = screen.FileName;
                var screenPath = Path.Combine(Directory.GetCurrentDirectory(), "files", $"{vehicle.Client.Phone}", $"{vehicle.RegistrationNumber}", "screens", screenName);
                using (var stream = new FileStream(screenPath, FileMode.Create))
                    await screen.CopyToAsync(stream);
                screenshot.Add(new ScreenShot
                {
                    CreatedAt = DateTime.Now,
                    Screen = screenName,
                    UpdateAt = DateTime.Now,
                });
            }
        }

        if (dto.FileNew != null && dto.FileOld != null)
        {
            Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/{"files"}/{vehicle.Client.Phone}/{vehicle.RegistrationNumber}/{"files"}");
                fileNewName = dto.FileNew.FileName;
                var fileNewPath = Path.Combine(Directory.GetCurrentDirectory(), "files", $"{vehicle.Client.Phone}", $"{vehicle.RegistrationNumber}", "files", fileNewName);
                fileOldName = dto.FileOld.FileName;
                var fileOldPath = Path.Combine(Directory.GetCurrentDirectory(), "files", $"{vehicle.Client.Phone}", $"{vehicle.RegistrationNumber}", "files", fileOldName);
                using (var stream = new FileStream(fileNewPath, FileMode.Create))
                    await dto.FileNew.CopyToAsync(stream);
                using (var stream = new FileStream(fileOldPath, FileMode.Create))
                    await dto.FileOld.CopyToAsync(stream);
            
        }

        if (dto.Malfunctions.Count > 0)
            foreach (var malfunction in dto.Malfunctions)
                malfunctions.Add(new Malfunction
                {
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    Value = malfunction,
                });

        if (dto.Suggestions.Count > 0)
            foreach (var suggestion in dto.Suggestions)
                suggestions.Add(new Suggestion
                {
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    Value = suggestion,

                });

        if (dto.ListWorks.Count > 0)
            foreach (var listWorks in dto.ListWorks)
            {
                listOfWorks.Add(new ListOfWorks
                {
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    Value = listWorks.Key,
                    Price = listWorks.Value,
                    
                });
            }
        
            
        

        vehicle.Visits.Add(new Visit
        {
            Mileage = dto.Mileage,
            Condition = dto.Condition,
            ScreenShots = screenshot,
            CreatedAt = DateTime.Now,
            UpdateAt = DateTime.Now,
            Malfunctions = malfunctions,
            ListWorks = listOfWorks,
            Suggestions = suggestions,
            ChiptuningFileNew = fileNewName,
            ChiptuningFileOld = fileOldName,
            Vehicle = vehicle,
        });

        _context.SaveChanges();

        return new GeneralAppServiceResponceDto
        {
            IsSucced = true,
            Message = "Успешно",
            StatusCode = 200,
        };

    }
}

