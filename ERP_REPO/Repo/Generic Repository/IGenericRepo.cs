using ERP_BL.Data;
using ERP_BL.Enums;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Linq.Dynamic.Core;
namespace ERP_REPO.Repo
{//
    public interface IGenericRepo<T> where T : class
    {
        Task<(IEnumerable<T> Data, int TotalCount)> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            string status = "All",
            string? sortColumn = null,
            string? sortDirection = "asc",
            string? searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10);
        Task<T?> GetByIdAsync(int id, string includeProperties = "");

        Task AddAsync(T entity, ClaimsPrincipal user);

        void Update(T entity, ClaimsPrincipal user);

        Task<bool> DeactivateAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task SaveAsync();
    }
    public class GenericService<T> : IGenericRepo<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task AddAsync(T entity, ClaimsPrincipal user)
        {
            var userId = GetUserId(user);

            SetEntityField(entity, "CreatedById", userId);
            SetEntityField(entity, "CreationDate", DateTime.UtcNow);  
            SetEntityField(entity, "IsActive", true);

            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity, ClaimsPrincipal user)
        {
            var userId = GetUserId(user);

            SetEntityField(entity, "LastModifiedById", userId);
            SetEntityField(entity, "ModifiedDate", DateTime.UtcNow); 

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        public virtual async Task<(IEnumerable<T> Data, int TotalCount)> GetAllAsync( 

            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            string status = "All",
            string? sortColumn = null,
            string? sortDirection = "asc",
            string? searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (typeof(T).GetProperty("IsActive") != null)
            {
                if (status.Equals("active", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(x => EF.Property<bool>(x, "IsActive"));
                else if (status.Equals("inactive", StringComparison.OrdinalIgnoreCase))
                    query = query.Where(x => !EF.Property<bool>(x, "IsActive"));
            }

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var terms = searchTerm
                    .Trim()
                    .ToLower()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var props = new List<string>();

                props.AddRange(typeof(T).GetProperties()
                    .Where(p => p.PropertyType == typeof(string))
                    .Select(p => p.Name));

                foreach (var nav in typeof(T).GetProperties()
                             .Where(p => !p.PropertyType.IsValueType && p.PropertyType != typeof(string)))
                {
                    var subProps = nav.PropertyType.GetProperties()
                        .Where(sp => sp.PropertyType == typeof(string))
                        .Select(sp => $"{nav.Name}.{sp.Name}")
                        .ToList();

                    props.AddRange(subProps);
                }

                foreach (var nav in typeof(T).GetProperties()
                             .Where(p => !p.PropertyType.IsValueType && p.PropertyType != typeof(string)))
                {
                    foreach (var subNav in nav.PropertyType.GetProperties()
                                 .Where(sp => !sp.PropertyType.IsValueType && sp.PropertyType != typeof(string)))
                    {
                        var subSubProps = subNav.PropertyType.GetProperties()
                            .Where(ssp => ssp.PropertyType == typeof(string))
                            .Select(ssp => $"{nav.Name}.{subNav.Name}.{ssp.Name}")
                            .ToList();

                        props.AddRange(subSubProps);
                    }
                }

                if (props.Any())
                {
                    var combinedExpr = string.Join(" + \" \" + ", props.Select(p => $"({p} ?? \"\")"));

                    var filters = new List<string>();
                    var args = new List<object>();

                    for (int i = 0; i < terms.Length; i++)
                    {
                        filters.Add($"({combinedExpr}).ToLower().Contains(@{i})");
                        args.Add(terms[i]);
                    }

                    var predicate = string.Join(" && ", filters);

                    Console.WriteLine($"[Dynamic Search] Predicate: {predicate}");
                    Console.WriteLine($"[Dynamic Search] Args: {string.Join(", ", args)}");

                    query = query.Where(predicate, args.ToArray());
                }
            }

            foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp.Trim());

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                sortDirection = sortDirection?.ToLower() == "desc" ? "desc" : "asc";

                try
                {
                    query = query.OrderBy($"{sortColumn} {sortDirection}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invalid sort column: {sortColumn}. Error: {ex.Message}");
                    query = query.OrderBy("Id");
                }
            }

            else if (orderBy != null)
            {
                query = orderBy(query);
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        public async Task<T?> GetByIdAsync(int id, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp.Trim());

            return await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            SetEntityField(entity, "IsActive", false);
            SetEntityField(entity, "ModifiedDate", DateTime.UtcNow); 

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        private void SetEntityField(T entity, string propName, object? value)
        {
            var prop = typeof(T).GetProperty(propName);
            if (prop != null && prop.CanWrite)
                prop.SetValue(entity, value);
        }

        private int? GetUserId(ClaimsPrincipal user)
        {
            var claim = user.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? user.FindFirstValue("UserId")
                        ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return int.TryParse(claim, out var id) ? id : null;
        }

        private void ConvertDateTimePropertiesToPakistaniTime(T entity)
        {
            if (typeof(T).GetProperty("CreationDate") != null)
            {
                var creationDate = EF.Property<DateTime>(entity, "CreationDate");
                typeof(T).GetProperty("CreationDate")?.SetValue(entity, TimeHelper.ConvertUtcToPakistaniTime(creationDate));
            }

            if (typeof(T).GetProperty("ModifiedDate") != null)
            {
                var modifiedDate = EF.Property<DateTime?>(entity, "ModifiedDate");
                if (modifiedDate.HasValue)
                {
                    typeof(T).GetProperty("ModifiedDate")?.SetValue(entity, TimeHelper.ConvertUtcToPakistaniTime(modifiedDate.Value));
                }
            }
        }
    }

}