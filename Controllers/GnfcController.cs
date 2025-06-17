using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GnfcController : Controller
    {
        private readonly ApplicationDbContext context;
        public GnfcController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {
            // Get all VTRIds that are already used
            var usedVtrIds = await context.GeneralDetails
                                          .Select(g => g.TraineeId)
                                          .ToListAsync();

            // Fetch only VTRIds that are NOT in the used list
            var availableVtrs = await context.Vtr
                                             .Where(v => !usedVtrIds.Contains(v.Id))
                                             .ToListAsync();
            ViewData["Colleges"] = new SelectList(await context.Colleges.ToListAsync(), "Id", "CollegeName");

            ViewData["Vtr"] = new SelectList(availableVtrs, "Id", "VTRId");
            ViewData["Departments"] = new SelectList(await context.Departments.ToListAsync(), "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gnfc viewModel)
        {
            

            var student = new Gnfc
            {
                TraineeId = viewModel.TraineeId,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                MiddleName = viewModel.MiddleName,
                DepartmentId = viewModel.DepartmentId,
                Email = viewModel.Email,
                Birth_Date = viewModel.Birth_Date,
                PhoneNumber = viewModel.PhoneNumber,
                CollegeId= viewModel.CollegeId,
                AcadamicYear = viewModel.AcadamicYear,
                Semester = viewModel.Semester
            };
            await context.GeneralDetails.AddAsync(student);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Gnfc");
        }
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // Define sorting parameters
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            //ViewData["CollegeSortParam"] = sortOrder == "college" ? "college_desc" : "college";
            var students = context.GeneralDetails
                    .Include(s => s.Trainee)
                    .Include(s => s.Department) // Load Department data                                                       // Ensure department data is loaded
                    .AsQueryable();
            // Apply search filter if search string is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
            }

            // Apply sorting based on the provided sort order
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                case "id":
                    students = students.OrderBy(s => s.TraineeId);
                    break;
                case "id_desc":
                    students = students.OrderByDescending(s => s.TraineeId);
                    break;
                case "college":
                    students = students.OrderBy(s => s.LastName);
                    break;
                case "college_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                default:
                    students = students.OrderBy(s => s.FirstName);
                    break;
            }
            return View(await students.AsNoTracking().ToListAsync());
        }


        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Departments"] = new SelectList(await context.Departments.ToListAsync(), "Id", "Name");
            ViewData["Colleges"] = new SelectList(await context.Colleges.ToListAsync(), "Id", "CollegeName");

            var student = await context.GeneralDetails.FindAsync(id);
            return View(student);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Gnfc viewModel)
        {
            var student = await context.GeneralDetails.FindAsync(viewModel.Id);
            if (student is not null)
            {
                //student.TraineeId = viewModel.TraineeId;
                student.FirstName = viewModel.FirstName;
                student.LastName = viewModel.LastName;
                student.MiddleName = viewModel.MiddleName;
                student.DepartmentId = viewModel.DepartmentId;
                student.Email = viewModel.Email;
                student.Birth_Date = viewModel.Birth_Date;
                student.PhoneNumber = viewModel.PhoneNumber;
                student.CollegeId = viewModel.CollegeId;
                student.AcadamicYear = viewModel.AcadamicYear;
                student.Semester = viewModel.Semester;

            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Gnfc");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.GeneralDetails.FindAsync(id);
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
            var s = await context.GeneralDetails.FindAsync(id);
            if (s == null)
            {
                return NotFound();
            }

            context.GeneralDetails.Remove(s);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to the list page
        }
    }
}

