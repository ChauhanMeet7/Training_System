using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class TraineeController : Controller
    {
        private readonly ApplicationDbContext context;
        public TraineeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // Define sorting parameters
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            ViewData["CollegeSortParam"] = sortOrder == "college" ? "college_desc" : "college";
            var students = context.Vtr
                
                    .AsQueryable();
            // Apply search filter if search string is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.VTRId.Contains(searchString));
            }

            // Apply sorting based on the provided sort order
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.VTRId);
                    break;
                case "id":
                    students = students.OrderBy(s => s.TrainingYear);
                    break;
                case "id_desc":
                    students = students.OrderByDescending(s => s.TrainingYear);
                    break;

                default:
                    students = students.OrderBy(s => s.VTRId);
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
        public async Task<IActionResult> Create(Trainee gnfc)
        {
            var gnfcId = new Trainee
            {
                Id = gnfc.Id,
                VTRId = gnfc.VTRId,
                TrainingYear = gnfc.TrainingYear,
            };
            await context.Vtr.AddAsync(gnfcId);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Trainee");
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {

            var student = await context.Vtr.FindAsync(id);
            return View(student);

        }
        [HttpPost]

        public async Task<IActionResult> Edit(Trainee viewModel)
        {
            var student = await context.Vtr.FindAsync(viewModel.Id);
            if (student is not null)
            {
                student.VTRId = viewModel.VTRId;
                student.TrainingYear = viewModel.TrainingYear;
            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Trainee");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await context.Vtr.FindAsync(id);
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
            var product = await context.Vtr.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            context.Vtr.Remove(product);
            await context.SaveChangesAsync();

            return RedirectToAction("Index","Trainee"); // Redirect to the list page
        }
    }

}
