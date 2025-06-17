using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AcademicController : Controller
    {
        private readonly ApplicationDbContext context;
        public AcademicController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {
            // Get all VTRIds that are already used
            var usedVtrIds = await context.AcademicDetails
                                          .Select(g => g.TraineeId)
                                          .ToListAsync();

            // Fetch only VTRIds that are NOT in the used list
            var availableVtrs = await context.Vtr
                                             .Where(v => !usedVtrIds.Contains(v.Id))
                                             .ToListAsync();

            ViewData["Vtr"] = new SelectList(availableVtrs, "Id", "VTRId");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Academic viewModel)
        {
            var student = new Academic
            {
                TraineeId = viewModel.TraineeId,
                Examination = viewModel.Examination,
                Board = viewModel.Board,
                Passing_year = viewModel.Passing_year,
                School = viewModel.School,
                Percentage = viewModel.Percentage,
                
            };
            await context.AcademicDetails.AddAsync(student);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Academic");
        }
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // Define sorting parameters
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            ViewData["CollegeSortParam"] = sortOrder == "college" ? "college_desc" : "college";
            var students = context.AcademicDetails
                                    .Include(s => s.Trainee)

                    .AsQueryable();
            // Apply search filter if search string is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Examination.Contains(searchString));
            }

            // Apply sorting based on the provided sort order
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.TraineeId);
                    break;
                case "id":
                    students = students.OrderBy(s => s.Examination);
                    break;
                case "id_desc":
                    students = students.OrderByDescending(s => s.Examination);
                    break;

                default:
                    students = students.OrderBy(s => s.TraineeId);
                    break;
            }
            return View(await students.AsNoTracking().ToListAsync());
        }


        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {

            var student = await context.AcademicDetails.FindAsync(id);
            return View(student);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Academic viewModel)
        {
            var student = await context.AcademicDetails.FindAsync(viewModel.Id);
            if (student is not null)
            {
                //student.TraineeId = viewModel.TraineeId;
                student.Examination = viewModel.Examination;
                student.Board = viewModel.Board;
                student.Passing_year = viewModel.Passing_year;
                student.School = viewModel.School;
                student.Percentage = viewModel.Percentage;
               

            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Academic");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.AcademicDetails.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var s = await context.AcademicDetails.FindAsync(id);
            if (s == null)
            {
                return NotFound();
            }

            context.AcademicDetails.Remove(s);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to the list page
        }
    }
}

