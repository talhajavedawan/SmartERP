using ERP_BL.Data;
using ERP_BL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
namespace ERP_REPO.Repo
{//
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepo<Vendor> Vendors { get; }
        IGenericRepo<Currency> Currencies { get; }
        IGenericRepo<VendorNature> VendorNatures { get; }
        IVendorContactRepo VendorContacts { get; }
        ApplicationDbContext Context { get; }
        void Detach<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveAsync();
    }




    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGenericRepo<Vendor> Vendors { get; private set; }
        public IGenericRepo<Currency> Currencies { get; private set; }
        public IGenericRepo<VendorNature> VendorNatures { get; private set; }
        public IVendorContactRepo VendorContacts { get; private set; } 
        public ApplicationDbContext Context => _context;


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Vendors = new GenericService<Vendor>(_context);
            Currencies = new GenericService<Currency>(_context);  
            VendorNatures = new GenericService<VendorNature>(_context);
            VendorContacts = new VendorContactService(_context); 
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                return; 
            }
            entry.State = EntityState.Detached; 
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
