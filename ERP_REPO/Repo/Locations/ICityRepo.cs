//using ERP_BL.Data;
//using ERP_BL.Entities.Cities;
//using ERP_BL.Entities.States;
//using ERP_REPO.Repo.Locations;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading.Tasks;

//namespace ERP_REPO.Repo.Locations
//{
//    public interface ICityRepo
//    {
//        Task<PagedResult<CityDto>> GetAllCitiesAsync(PaginationParams paginationParams); // ✅ FIXED
//        Task<CityDto?> GetCityByIdAsync(int id);
//        Task<int> CreateCityAsync(CreateCityDto dto);
//        Task<bool> UpdateCityAsync(UpdateCityDto dto);
//        Task<bool> DeleteCityAsync(int id);
//    }
//}


//public class CityService : ICityRepo
//{
//    private readonly ApplicationDbContext _context;

//    public CityService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<PagedResult<CityDto>> GetAllCitiesAsync(PaginationParams paginationParams)
//    {
//        var query = _context.Cities
//            .Include(c => c.State)
//            .AsNoTracking(); // very important for performance

//        var totalCount = await query.CountAsync();

//        var cities = await query
//            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
//            .Take(paginationParams.PageSize)
//            .Select(c => new CityDto
//            {
//                Id = c.Id,
//                Name = c.Name,
//                StateId = c.StateId,
//                StateName = c.State.Name
//            })
//            .ToListAsync();

//        return new PagedResult<CityDto>
//        {
//            CurrentPage = paginationParams.PageNumber,
//            PageSize = paginationParams.PageSize,
//            TotalCount = totalCount,
//            Items = cities
//        };
//    }


//    public async Task<CityDto?> GetCityByIdAsync(int id)
//    {
//        return await _context.Cities
//            .Include(c => c.State)
//            .Where(c => c.Id == id)
//            .Select(c => new CityDto
//            {
//                Id = c.Id,
//                Name = c.Name,
//                StateId = c.StateId,
//                StateName = c.State.Name
//            }).FirstOrDefaultAsync();
//    }

//    public async Task<int> CreateCityAsync(CreateCityDto dto)
//    {
//        var stateExists = await _context.States.AnyAsync(s => s.Id == dto.StateId);
//        if (!stateExists)
//            throw new ArgumentException("Invalid StateId");

//        var city = new City
//        {
//            Name = dto.Name,
//            StateId = dto.StateId
//        };

//        _context.Cities.Add(city);
//        await _context.SaveChangesAsync();
//        return city.Id;
//    }


//    public async Task<bool> UpdateCityAsync(UpdateCityDto dto)
//    {
//        var city = await _context.Cities.FindAsync(dto.Id);
//        if (city == null) return false;

//        var stateExists = await _context.States.AnyAsync(s => s.Id == dto.StateId);
//        if (!stateExists)
//            throw new ArgumentException("Invalid StateId");

//        city.Name = dto.Name;
//        city.StateId = dto.StateId;

//        await _context.SaveChangesAsync();
//        return true;
//    }


//    public async Task<bool> DeleteCityAsync(int id)
//    {
//        var city = await _context.Cities.FindAsync(id);
//        if (city == null) return false;

//        _context.Cities.Remove(city);
//        await _context.SaveChangesAsync();
//        return true;
//    }
//}

