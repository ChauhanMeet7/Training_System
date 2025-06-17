using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PersonalDetailController : Controller
    {
        private readonly ApplicationDbContext context;
        public PersonalDetailController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {
            // Get all VTRIds that are already used
            var usedVtrIds = await context.PersonalDetails
                                          .Select(g => g.TraineeId)
                                          .ToListAsync();

            // Fetch only VTRIds that are NOT in the used list
            var availableVtrs = await context.Vtr
                                             .Where(v => !usedVtrIds.Contains(v.Id))
                                             .ToListAsync();

            ViewData["Vtr"] = new SelectList(availableVtrs, "Id", "VTRId");
            //ViewData["Departments"] = new SelectList(await context.Departments.ToListAsync(), "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonalDetail viewModel)
        {
        
            var student = new PersonalDetail
            {
                TraineeId = viewModel.TraineeId,
                Religion = viewModel.Religion,
                Blood_Group = viewModel.Blood_Group,
                PhysicallyHandicapped = viewModel.PhysicallyHandicapped,
                MaritalStatus = viewModel.MaritalStatus,
                Height = viewModel.Height,
                Weight = viewModel.Weight,
                LocalAddress = viewModel.LocalAddress,
                PermanentAddress = viewModel.PermanentAddress,
                Gender = viewModel.Gender
            };
            await context.PersonalDetails.AddAsync(student);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "PersonalDetail");
        }
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // Define sorting parameters
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            ViewData["CollegeSortParam"] = sortOrder == "college" ? "college_desc" : "college";
            var students = context.PersonalDetails
                    .Include(s => s.Trainee) // Ensure department data is loaded
                    .AsQueryable();
            // Apply search filter if search string is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Religion.Contains(searchString));
            }

            // Apply sorting based on the provided sort order
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Religion);
                    break;
                case "id":
                    students = students.OrderBy(s => s.TraineeId);
                    break;
                case "id_desc":
                    students = students.OrderByDescending(s => s.TraineeId);
                    break;
              
                default:
                    students = students.OrderBy(s => s.Religion);
                    break;
            }
            return View(await students.AsNoTracking().ToListAsync());
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {

            var student = await context.PersonalDetails.FindAsync(id);
            return View(student);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PersonalDetail viewModel)
        {
            var student = await context.PersonalDetails.FindAsync(viewModel.Id);
            if (student is not null)
            {
                //student.TraineeId = viewModel.TraineeId;
                student.Religion = viewModel.Religion;
                student.Gender = viewModel.Gender;
                student.Blood_Group = viewModel.Blood_Group;
                student.PhysicallyHandicapped = viewModel.PhysicallyHandicapped;
                student.MaritalStatus = viewModel.MaritalStatus;
                student.Height = viewModel.Height;
                student.Weight = viewModel.Weight;
                student.LocalAddress = viewModel.LocalAddress;
                student.PermanentAddress = viewModel.PermanentAddress;
            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "PersonalDetail");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await context.PersonalDetails.FindAsync(id);
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
            var s = await context.PersonalDetails.FindAsync(id);
            if (s == null)
            {
                return NotFound();
            }

            context.PersonalDetails.Remove(s);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to the list page
        }
    }
}

