using Kurskcartuning.Server_v2.Core.DbContext;
using Kurskcartuning.Server_v2.Core.Dtos.App;
using Kurskcartuning.Server_v2.Core.Dtos.App.Client;
using Kurskcartuning.Server_v2.Core.Dtos.App.Vehicle;
using Kurskcartuning.Server_v2.Core.Entities.AppDB;
using Kurskcartuning.Server_v2.Core.Interfaces;
using Kurskcartuning.Server_v2.Core.Interfaces.App;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kurskcartuning.Server_v2.Core.Services.App;

public class VehicleService : IVehicleService
{

    private readonly ApplicationDbContext _context;
    private readonly ILogService _logService;

    public VehicleService(ApplicationDbContext context, ILogService logService)
    {
        _context = context;
        _logService = logService;
    }

    private record DataVehiclePost
    {
        public Model newModel = new();
        public Vehicle newVehicle = new();
        public Manufacturer newManufacturer = new();
        public List<Model> newListModels = new();
        public List<Vehicle> newListVehicles = new();
    }

    public async Task<GeneralAppServiceResponceDto> PostVehicleAsync(ClaimsPrincipal User, VehiclePostDto dto, string id)
    {
        try
        {
            var Data = new DataVehiclePost();

            var client = await _context.Clients.Where(client => client.Id.Equals(dto.CLientId)).FirstOrDefaultAsync(x => x.UserId.Equals(id));
            if (client == null)
                return new GeneralAppServiceResponceDto
                {
                    IsSucced = false,
                    Message = "Ошибка получения клиента. Свяжитесь с администратором",
                    StatusCode = 404,
                };

            var isManufacturerValid = await _context.Manufacturers.FirstOrDefaultAsync(x => x.Value.Equals(dto.Manufacturer));
            var isModelValid = await _context.Models.FirstOrDefaultAsync(x => x.Value.Equals(dto.Model));

            Data.newVehicle = new Vehicle { RegistrationNumber = dto.RegistrationNumber, Year = dto.Year, Configuration = dto.Configuration, EnginePower = dto.EnginePower };


            if (isManufacturerValid is Manufacturer && isModelValid is not Model)
            {
                Data.newModel.Value = dto.Model;
                client.Vehicles.Add(Data.newVehicle);
                isManufacturerValid.Models.Add(Data.newModel);
                isManufacturerValid.Vehicles.Add(Data.newVehicle);
            }

            if (isManufacturerValid is not Manufacturer && isModelValid is not Model)
            {
                Data.newModel = new Model { Value = dto.Model };
                Data.newListModels.Add(Data.newModel);
                client.Vehicles.Add(Data.newVehicle);
                Data.newListVehicles.Add(Data.newVehicle);
                Data.newManufacturer = new Manufacturer { Value = dto.Manufacturer, Models = Data.newListModels, Vehicles = Data.newListVehicles };
                await _context.Manufacturers.AddAsync(Data.newManufacturer);
            }

            if (isManufacturerValid is not Manufacturer && isModelValid is Model)
            {
                Data.newModel = new Model { Value = dto.Model };
                Data.newListModels.Add(Data.newModel);
                client.Vehicles.Add(Data.newVehicle);
                Data.newListVehicles.Add(Data.newVehicle);

                Data.newManufacturer = new Manufacturer { Value = dto.Manufacturer, Models = Data.newListModels, Vehicles = Data.newListVehicles };
                await _context.Manufacturers.AddAsync(Data.newManufacturer);
            }

            if (isManufacturerValid is Manufacturer && isModelValid is Model)
            {
                Data.newListVehicles.Add(Data.newVehicle);
                client.Vehicles.Add(Data.newVehicle);
                isManufacturerValid.Vehicles = Data.newListVehicles;
            }

            await _context.SaveChangesAsync();

            var responce = await _context.Clients
                .Where(x => x.UserId.Equals(id) && x.Id.Equals(dto.CLientId))
                .Select(x => new VehicleResponceLineDto
                {
                    ClientId = x.Id,
                    RegistrationNumber = dto.RegistrationNumber,
                    ManufacturerValue = dto.Manufacturer,
                    ModelValue = dto.Model

                }).FirstOrDefaultAsync();

            await _logService.SaveNewLog(User.Identity.Name, $"Автомобиль {dto.Model} {dto.Manufacturer} {dto.RegistrationNumber} успешно добавлен!");

            return new GeneralAppServiceResponceDto
            {
                IsSucced = true,
                Message = "Автомобиль успешно добавлен!",
                StatusCode = 201,
                Responce = responce
            };
        }
        catch (Exception ex)
        {
            return new GeneralAppServiceResponceDto
            {
                IsSucced = false,
                Message = ex.Message,
                StatusCode = 500
            };
        }
    }

    public async Task<GeneralAppServiceResponceDto> GetVehicleIdAsync(long id, string userId)
    {
        Vehicle? vehicle = await _context.Vehicles.Include(x => x.Manufacturer).ThenInclude(x => x.Models).FirstOrDefaultAsync(x => x.Id.Equals(id) && x.Client.UserId.Equals(userId));

        if (vehicle is null)
            return new GeneralAppServiceResponceDto
            {
                IsSucced = false,
                Message = "Ошибка получения автомобиля. Свяжитесь с администратором",
                StatusCode = 404,
            };

        Model? model = vehicle.Manufacturer.Models.FirstOrDefault(x => x.Manufacturer.Id == vehicle.Manufacturer.Id && vehicle.ManufacturerId.Equals(vehicle.Manufacturer.Id));


        if (model is null && vehicle.Manufacturer is null)
            return new GeneralAppServiceResponceDto
            {
                IsSucced = false,
                Message = "Ошибка получения автомобиля. Свяжитесь с администратором",
                StatusCode = 404,
            };

        return new GeneralAppServiceResponceDto
        {
            IsSucced = true,
            Message = "Успешно",
            StatusCode = 200,
            Responce = new VehicleResponceIdDto
            {
                Configuration = vehicle.Configuration,
                EnginePower = vehicle.EnginePower,
                ManufacturerValue = vehicle.Manufacturer.Value,
                ModelValue = model!.Value,
                RegistrationNumber = vehicle.RegistrationNumber,
                Year = vehicle.Year
            }
        };
    }

    public async Task<GeneralAppServiceResponceDto> PutVehicleAsync(long id, string userId, VehiclePutDto dto)
    {
        Vehicle? vehicle = await _context.Vehicles.Include(x => x.Manufacturer).ThenInclude(x => x.Models).FirstOrDefaultAsync(x => x.Id.Equals(id) && x.Client.UserId.Equals(userId));

        if (vehicle is null)
            return new GeneralAppServiceResponceDto
            {
                IsSucced = false,
                Message = "Ошибка получения автомобиля. Свяжитесь с администратором",
                StatusCode = 404,
            };

        try
        {
            vehicle.RegistrationNumber = dto.RegistrationNumber;
            vehicle.Configuration = dto.Configuration;
            vehicle.EnginePower = dto.EnginePower;

            await _context.SaveChangesAsync();

            Model? model = vehicle.Manufacturer.Models.FirstOrDefault(x => x.Manufacturer.Id == vehicle.Manufacturer.Id && vehicle.ManufacturerId.Equals(vehicle.Manufacturer.Id));

            if (model is null && vehicle.Manufacturer is null)
                return new GeneralAppServiceResponceDto
                {
                    IsSucced = false,
                    Message = "Ошибка получения автомобиля. Свяжитесь с администратором",
                    StatusCode = 404,
                };

            return new GeneralAppServiceResponceDto
            {
                IsSucced = true,
                Message = "Успешно",
                StatusCode = 200,
                Responce = new VehicleResponceIdDto
                {
                    Configuration = dto.Configuration,
                    EnginePower = dto.EnginePower,
                    ManufacturerValue = vehicle.Manufacturer.Value,
                    Year = vehicle.Year,
                    RegistrationNumber = dto.RegistrationNumber,
                    ModelValue = model!.Value
                }
            };

        }
        catch (Exception ex)
        {
            return new GeneralAppServiceResponceDto
            {
                IsSucced = false,
                Message = ex.Message,
                StatusCode = 500,
            };
        }
    }

    public async Task<GeneralAppServiceResponceDto> GetVehicleListAsync(long clientId, string userId)
    {
        List<Vehicle>? vehicles = await _context.Vehicles.Include(x => x.Manufacturer).ThenInclude(x => x.Models).Where(x => x.Client.UserId.Equals(userId) && x.ClientId.Equals(clientId)).ToListAsync();

        if (vehicles is null)
            return new GeneralAppServiceResponceDto
            {
                IsSucced = false,
                Message = "Ошибка получения автомобилей. Свяжитесь с администратором",
                StatusCode = 404,
            };

        List<VehicleResponceLineDto> responce = new();

        foreach (var vehicle in vehicles)
        {
            Model? model = vehicle.Manufacturer.Models.FirstOrDefault(x => x.Manufacturer.Id == vehicle.Manufacturer.Id && vehicle.ManufacturerId.Equals(vehicle.Manufacturer.Id));
            if (model is null)
                return new GeneralAppServiceResponceDto
                {
                    IsSucced = false,
                    Message = "Ошибка получения автомобилей. Свяжитесь с администратором",
                    StatusCode = 404,
                };
            responce.Add(new VehicleResponceLineDto
            {
                ClientId = vehicle.ClientId,
                ManufacturerValue = vehicle.Manufacturer.Value,
                ModelValue = model.Value,
                RegistrationNumber = vehicle.RegistrationNumber,
            });
        }

        return new GeneralAppServiceResponceDto
        {
            IsSucced = true,
            Message = "Успешно",
            StatusCode = 200,
            Responce = responce
        };
    }
}

