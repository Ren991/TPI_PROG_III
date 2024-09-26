using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Carts)
                .ThenInclude(c => c.SaleLine)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> ListAsync()
        {
            return await _context.Users
                .Include(u => u.Carts)
                .ThenInclude(c => c.SaleLine)
                .ThenInclude(cp => cp.Product)
                .ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public User? GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(p => p.Name == userName);
        }

        public IEnumerable<User> GetAll() 
        {
            return _context.Users;
        }
    }
}
