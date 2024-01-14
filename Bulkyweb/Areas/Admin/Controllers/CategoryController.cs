using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.Model;
using Microsoft.AspNetCore.Mvc;


namespace BulkyBookweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Category> ObjectCategoryList = _UnitOfWork.Category.GetAll().ToList();
            return View(ObjectCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cant not match the Name.");
            }

            if (obj.Name != null && obj.Name.ToLower() == "Test") //modelonly
            {
                ModelState.AddModelError("", "Test is an invalid value.");
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "category Created Successfully!";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? CategoryFromDb = _UnitOfWork.Category.Get(u => u.ID == id);
            //  Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.ID == id);
            //  Category? categoryFromDb2= _db.Categories.Where(u => u.ID == id).FirstOrDefault();

            if (CategoryFromDb == null)
            {
                return NotFound();
            }
            return View(CategoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "category Updated Successfully!";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _UnitOfWork.Category.Get(u => u.ID == id);
            // Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u => u.ID == id);
            // Category? categoryFromDb2 = _db.Categories.Where(u => u.ID == id).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? obj = _UnitOfWork.Category.Get(u => u.ID == id);
            if (obj == null)
            {
                return NotFound();
            }
            _UnitOfWork.Category.Remove(obj);
            _UnitOfWork.Save();
            TempData["success"] = "category Deteted Successfully!";
            return RedirectToAction("Index");

        }
    }

}
