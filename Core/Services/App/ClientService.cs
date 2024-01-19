using Kurskcartuning.Server_v2.Core.DbContext;
using Kurskcartuning.Server_v2.Core.Dtos.App;
using Kurskcartuning.Server_v2.Core.Dtos.App.Client;
using Kurskcartuning.Server_v2.Core.Entities.AppDB;
using Kurskcartuning.Server_v2.Core.Interfaces;
using Kurskcartuning.Server_v2.Core.Interfaces.App;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Services.App;

public class ClientService : IClientService
{

    private readonly ApplicationDbContext _context;
    private readonly ILogService _logService;

    public ClientService(ApplicationDbContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }

    public async Task<GeneralAppServiceResponceDto> DeleteClientAsync(ClaimsPrincipal User, ClientDeleteDto dto)
    {
        Client? client = await _context.Clients.FirstOrDefaultAsync(x => x.Id.Equals(dto.Id));

        if (client is null)
            return new GeneralAppServiceResponceDto()
            {
                IsSucced = false,
                StatusCode = 404,
                Message = "Ошибка удаления клиента"
            };

        _context.Clients.Remove(client);

        await _context.SaveChangesAsync();

        await _logService.SaveNewLog(User.Identity.Name, $"Клиент {dto.Name} ({dto.Phone}) успешно удален!");

        return new GeneralAppServiceResponceDto()
        {
            IsSucced = true,
            StatusCode = 200,
            Message = "Клиент успешно удален!"
        };
    }

    public async Task<GeneralAppServiceResponceDto> GetClientIdAsync(long id)
    {
        var client = await _context.Clients.Where(x => x.Id.Equals(id)).Select(x => new ClientResponceDto() { FirstName = x.FirstName, Id = x.Id, Phone = x.Phone }).OrderBy(q => q.Id).FirstOrDefaultAsync();
        if (client is null)
            return new GeneralAppServiceResponceDto()
            {
                IsSucced = false,
                StatusCode = 404,
                Message = "Ошибка получения клиента"
            };

        return new GeneralAppServiceResponceDto()
        {
            IsSucced = true,
            Message = "Клиент получен",
            StatusCode = 200,
            Responce = client
        };
    }

    public async Task<GeneralAppServiceResponceDto> GetClientsAsync(string id)
    {
        var clients = await _context.Clients.Where(x => x.UserId.Equals(id)).Select(x => new ClientResponceDto() { FirstName = x.FirstName, Id = x.Id, Phone = x.Phone }).OrderBy(q => q.FirstName).ToListAsync();

        if (clients.Count == 0)
            return new GeneralAppServiceResponceDto()
            {
                IsSucced = false,
                Message = "У Вас еще нет клиентов",
                StatusCode = 404,
            };

        return new GeneralAppServiceResponceDto()
        {
            IsSucced = true,
            Message = "Клиенты получены",
            StatusCode = 200,
            Responce = clients
        };
    }

    public async Task<GeneralAppServiceResponceDto> PostClientAsync(ClaimsPrincipal User, ClientPostDto dto, string id)
    {

        var isUserPhoneValid = await _context.Clients.Where(x => x.Phone.Equals(dto.Phone))
            .Select(x => new ClientResponceDto() { FirstName = x.FirstName, Id = x.Id, Phone = x.Phone })
            .FirstOrDefaultAsync();

        if (isUserPhoneValid is not null)
            return new GeneralAppServiceResponceDto()
            {
                IsSucced = false,
                Message = "Такой клиент уже существует!",
                StatusCode = 409,
            };

        var user = await _context.Clients.AddAsync(new Client { FirstName = dto.Name, Phone = dto.Phone, UserId = id });

        await _context.SaveChangesAsync();

        await _logService.SaveNewLog(User.Identity.Name, $"Клиент {dto.Name} ({dto.Phone}) успешно добавлен!");

        var client = await _context.Clients
            .Where(x => x.Phone.Equals(dto.Phone))
            .Select(x => new ClientResponceDto() { FirstName = x.FirstName, Id = x.Id, Phone = x.Phone })
            .FirstOrDefaultAsync();

        if (client is null)
            return new GeneralAppServiceResponceDto()
            {
                IsSucced = false,
                Message = "Ошибка получения клиента, обновите приложение",
                StatusCode = 404,
            };

        return new GeneralAppServiceResponceDto()
        {
            IsSucced = true,
            Message = "Клиент успешно добавлен!",
            StatusCode = 201,
            Responce = client
        };
    }

    public async Task<GeneralAppServiceResponceDto> PutClientAsync(ClaimsPrincipal User, ClientPutDto dto)
    {
        var client = await _context.Clients
            .Where(x => x.Id.Equals(dto.Id))
            .Select(x => new ClientResponceDto { FirstName = x.FirstName, Id = x.Id, Phone = x.Phone })
            .FirstOrDefaultAsync();


        if (client == null)
            return new GeneralAppServiceResponceDto()
            {
                IsSucced = false,
                Message = "Ошибка получения клиента",
                StatusCode = 404,
            };


        client.Phone = dto.Phone;
        client.FirstName = dto.Name;

        await _context.SaveChangesAsync();

        await _logService.SaveNewLog(User.Identity.Name, $"Клиент {dto.Name} ({dto.Phone}) успешно обновлен!");


        return new GeneralAppServiceResponceDto()
        {
            IsSucced = true,
            Message = "Клиент успешно обнавлен!",
            StatusCode = 202,
            Responce = client
        };
    }
}

