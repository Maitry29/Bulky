using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Model.ViewModels;
using System.IO;


namespace BulkyBookweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> ObjectProductList = _UnitOfWork.product.GetAll(includeProperties:"Category").ToList();

            return View(ObjectProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _UnitOfWork.product.Get(u=>u.ID == id);
                return View(productVM);
            }
        }

        //ViewBag.CategoryList = CategoryList;
        //ViewData["CategoryList"] = CategoryList;

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)

        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _WebHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageURL))
                    {
                        //delete the old Image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageURL.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                                                
                    }
                    productVM.Product.ImageURL = @"\Images\Product\" + fileName;
                }
                if(productVM.Product.ID == 0)
                {
                    _UnitOfWork.product.Add(productVM.Product);
                }
                else
                {
                    _UnitOfWork.product.Update(productVM.Product);
                }
               
                _UnitOfWork.Save();
                TempData["success"] = "Product Created Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                });
                return View(productVM);
            }

        }   

       
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> ObjectProductList = _UnitOfWork.product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = ObjectProductList });
        }
       

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            
            var productToBeDeleted = _UnitOfWork.product.Get(u=>u.ID == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false,  message = "Error while Deleting" });
            }
            //delete the old Image
            var oldImagePath = Path.Combine(_WebHostEnvironment.WebRootPath,
                productToBeDeleted.ImageURL.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _UnitOfWork.product.Remove(productToBeDeleted);
            _UnitOfWork.Save();

             return Json(new { success = true,  message = "Delete Successful" });
        }
        #endregion
    }
}
