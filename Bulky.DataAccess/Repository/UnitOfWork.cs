using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;

namespace BulkyBook.DataAccess.Repository
{
  /*  public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        void Save();
    }*/

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public ICategoryRepository Category { get;private set; }
        public IProductRepository product { get; private set; }
 

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            product = new ProductsRepository(_db);


        }

        public void Save()
        {
           
            // Save changes
            _db.SaveChanges();
  }


        // Implement IDisposable
        /*  private bool disposed = false;

          protected virtual void Dispose(bool disposing)
          {
              if (!disposed)
              {
                  if (disposing)
                  {
                      _db.Dispose();
                  }
                  disposed = true;
              }
          }

          public void Dispose()
          {
              Dispose(true);
              GC.SuppressFinalize(this);
          }*/
    }
}
