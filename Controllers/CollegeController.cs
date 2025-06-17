using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class CollegeController : Controller
    {
        private readonly ApplicationDbContext context;
        public CollegeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // Define sorting parameters
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            ViewData["CollegeSortParam"] = sortOrder == "college" ? "college_desc" : "college";
            var students = context.Colleges
                    .AsQueryable();
            // Apply search filter if search string is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.CollegeName.Contains(searchString));
            }

            // Apply sorting based on the provided sort order
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.CollegeName);
                    break;
                case "id":
                    students = students.OrderBy(s => s.Id);
                    break;
                case "id_desc":
                    students = students.OrderByDescending(s => s.Id);
                    break;

                default:
                    students = students.OrderBy(s => s.CollegeName);
                    break;
            }
            return View(await students.AsNoTracking().ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(College gnfc)
        {
            var gnfcId = new College
            {
                Id = gnfc.Id,
                CollegeName = gnfc.CollegeName,
                Address = gnfc.Address,
                State = gnfc.State,
                City = gnfc.City,

            };
            await context.Colleges.AddAsync(gnfcId);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "College");
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {

            var student = await context.Colleges.FindAsync(id);
            return View(student);

        }
        [HttpPost]

        public async Task<IActionResult> Edit(College viewModel)
        {
            var student = await context.Colleges.FindAsync(viewModel.Id);
            if (student is not null)
            {
                student.CollegeName = viewModel.CollegeName;
                student.Address = viewModel.Address;
                student.State = viewModel.State;
                student.City = viewModel.City;

            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "College");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await context.Colleges.FindAsync(id);
            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await context.Colleges.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            context.Colleges.Remove(product);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "College"); // Redirect to the list page
        }
    }

}
