using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FamilyController : Controller
    {
        private readonly ApplicationDbContext context;
        public FamilyController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {
            // Get all VTRIds that are already used in GeneralDetails
            var usedVtrIds = await context.FamilyDetails
                                          .Select(g => g.TraineeId)
                                          .ToListAsync();

            // Check if there is existing data in GeneralDetails
            var availableVtrs = await context.Vtr
                                             .Where(v => !usedVtrIds.Contains(v.Id)) // Show all if empty
                                             .ToListAsync();

            ViewData["Vtr"] = new SelectList(availableVtrs, "Id", "VTRId");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Family viewModel)
        {
            var student = new Family
            {
                TraineeId = viewModel.TraineeId,
                FatherName = viewModel.FatherName,
                FatherAge = viewModel.FatherAge,
                FatherQulification = viewModel.FatherQulification,
                FatherProfession = viewModel.FatherProfession,
                FatherIncome = viewModel.FatherIncome,
                MotherName = viewModel.MotherName,
                MotherAge = viewModel.MotherAge,
                MotherQulification = viewModel.MotherQulification,
                MotherIncome = viewModel.MotherIncome,
                MotherProfession = viewModel.MotherProfession,
                GuardianName = viewModel.GuardianName,
                GuardianAge = viewModel.GuardianAge,
                GuardianQulification = viewModel.GuardianQulification,
                GuardianProfession = viewModel.GuardianProfession,
                GuardianIncome = viewModel.GuardianIncome,
                SisterName = viewModel.SisterName,
                SisterAge = viewModel.SisterAge,
                SisterQulification = viewModel.SisterQulification,
                SisterProfession = viewModel.SisterProfession,
                SisterIncome = viewModel.SisterIncome,
                BrotherName = viewModel.BrotherName,
                BrotherAge = viewModel.BrotherAge,
                BrotherQulification = viewModel.BrotherQulification,
                BrotherProgession = viewModel.BrotherProgession,
                BrotherIncome = viewModel.BrotherIncome,
            };
            await context.FamilyDetails.AddAsync(student);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Family");
        }
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // Define sorting parameters
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            ViewData["CollegeSortParam"] = sortOrder == "college" ? "college_desc" : "college";
            var students = context.FamilyDetails
                .Include(s => s.Trainee) // Ensure department data is loaded
                    .AsQueryable();
            // Apply search filter if search string is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.FatherName.Contains(searchString) || s.MotherName.Contains(searchString) );
            }

            // Apply sorting based on the provided sort order
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.FatherName);
                    break;
                case "id":
                    students = students.OrderBy(s => s.TraineeId);
                    break;
                case "id_desc":
                    students = students.OrderByDescending(s => s.TraineeId);
                    break;
                case "college":
                    students = students.OrderBy(s => s.MotherName);
                    break;
                case "college_desc":
                    students = students.OrderByDescending(s => s.MotherName);
                    break;
                default:
                    students = students.OrderBy(s => s.FatherName);
                    break;
            }
            return View(await students.AsNoTracking().ToListAsync());
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {

            var student = await context.FamilyDetails.FindAsync(id);
            return View(student);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Family viewModel)
        {
            var student = await context.FamilyDetails.FindAsync(viewModel.Id);
            if (student is not null)
            {
                //student.TraineeId = viewModel.TraineeId;
                student.FatherName = viewModel.FatherName;
                student.FatherAge = viewModel.FatherAge;
                student.FatherQulification = viewModel.FatherQulification;
                student.FatherProfession = viewModel.FatherProfession;
                student.FatherIncome = viewModel.FatherIncome;
                student.MotherName = viewModel.MotherName;
                student.MotherAge = viewModel.MotherAge;
                student.MotherQulification = viewModel.MotherQulification;
                student.MotherIncome = viewModel.MotherIncome;
                student.MotherProfession = viewModel.MotherProfession;
                student.GuardianName = viewModel.GuardianName;
                student.GuardianAge = viewModel.GuardianAge;
                student.GuardianQulification = viewModel.GuardianQulification;
                student.GuardianProfession = viewModel.GuardianProfession;
                student.GuardianIncome = viewModel.GuardianIncome;
                student.SisterName = viewModel.SisterName;
                student.SisterAge = viewModel.SisterAge;
                student.SisterQulification = viewModel.SisterQulification;
                student.SisterProfession = viewModel.SisterProfession;
                student.SisterIncome = viewModel.SisterIncome;
                student.BrotherName = viewModel.BrotherName;
                student.BrotherAge = viewModel.BrotherAge;
                student.BrotherQulification = viewModel.BrotherQulification;
                student.BrotherProgession = viewModel.BrotherProgession;
                student.BrotherIncome = viewModel.BrotherIncome;

            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Family");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.FamilyDetails.FindAsync(id);
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
            var product = await context.FamilyDetails.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            context.FamilyDetails.Remove(product);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to the list page
        }
    }
}

