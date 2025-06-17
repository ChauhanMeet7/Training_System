using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PostingController : Controller
    {
        private readonly ApplicationDbContext context;
        public PostingController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {
            // Get all VTRIds that are already used
            var usedVtrIds = await context.Postings
                                          .Select(g => g.TraineeId)
                                          .ToListAsync();

            // Fetch only VTRIds that are NOT in the used list
            var availableVtrs = await context.Vtr
                                             .Where(v => !usedVtrIds.Contains(v.Id))
                                             .ToListAsync();
            ViewData["Colleges"] = new SelectList(await context.Colleges.ToListAsync(), "Id", "CollegeName");
            ViewData["Plants"] = new SelectList(await context.Plants.ToListAsync(), "Id", "Name");

            ViewData["Vtr"] = new SelectList(availableVtrs, "Id", "VTRId");
            //ViewData["Departments"] = new SelectList(await context.Departments.ToListAsync(), "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Posting viewModel)
        {


            var student = new Posting
            {
                TraineeId = viewModel.TraineeId,
               PlantId = viewModel.PlantId,
                
            };
            await context.Postings.AddAsync(student);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Posting");
        }
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            // Define sorting parameters
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            //ViewData["CollegeSortParam"] = sortOrder == "college" ? "college_desc" : "college";
            var students = context.Postings
                    .Include(s => s.Trainee)
                    .Include(s => s.Plant) // Load Department data                                                       // Ensure department data is loaded
                    .AsQueryable();
            
            return View(await students.AsNoTracking().ToListAsync());
        }


        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Plants"] = new SelectList(await context.Plants.ToListAsync(), "Id", "Name");

            //ViewData["Departments"] = new SelectList(await context.Departments.ToListAsync(), "Id", "Name");
            ViewData["Colleges"] = new SelectList(await context.Colleges.ToListAsync(), "Id", "CollegeName");

            var student = await context.Postings.FindAsync(id);
            return View(student);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Posting viewModel)
        {
            var student = await context.Postings.FindAsync(viewModel.Id);
            if (student is not null)
            {
                //student.TraineeId = viewModel.TraineeId;
                student.PlantId = viewModel.PlantId;
                

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

            var product = await context.Postings.FindAsync(id);
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
            var s = await context.Postings.FindAsync(id);
            if (s == null)
            {
                return NotFound();
            }

            context.Postings.Remove(s);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redirect to the list page
        }
    }
}

