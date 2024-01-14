using BulkyBook.DataAccess.Data;
using BulkyBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductsRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.ID == obj.ID);
            if(objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryID = obj.CategoryID;
                objFromDb.Author = obj.Author;
                if(obj.ImageURL != null)
                {
                  objFromDb.ImageURL = obj.ImageURL;
                }

            }
        }

       

       
    }
}
