using ERP_BL.Data;
using ERP_BL.Entities.Core.PowerUsers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_REPO.Repo.Core.PowerUsers
{
    public interface IPowerUserRepo
    {
        Task<PowerUser?> GetByPowerUserNameAsync(string username);
        Task AddPowerUserAsync(PowerUser user);
        Task UpdatePowerUserAsync(PowerUser user);

    }

    public class PowerUserService : IPowerUserRepo
    {
        private readonly ApplicationDbContext _context;

        public PowerUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PowerUser?> GetByPowerUserNameAsync(string username)
        {
            return await _context.PowerUsers
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task AddPowerUserAsync(PowerUser user)
        {
            // No password hashing here; just save the user as is
            _context.PowerUsers.Add(user);
            await _context.SaveChangesAsync();
        }

        public bool VerifyPassword(string storedHash, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
        public async Task UpdatePowerUserAsync(PowerUser user)
        {
            _context.PowerUsers.Update(user);
            await _context.SaveChangesAsync();
        }

    }
}