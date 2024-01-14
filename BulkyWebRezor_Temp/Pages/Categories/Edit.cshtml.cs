using BulkyWebRezor_Temp.Data;
using BulkyWebRezor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRezor_Temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category? Categories { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;

        }
        public void OnGet(int? id )
        {
            if ( id == null  && id == 0)
            {
                Categories = _db.Categories.Find(id);
            }
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(Categories);
                _db.SaveChanges();
                //TempData["success"] = "category Updated Successfully!";
                return RedirectToPage("Index");
            }

            
        }
    }
}
