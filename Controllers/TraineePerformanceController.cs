using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using Rotativa.AspNetCore;

namespace WebApplication1.Controllers
{
    public class TraineePerformanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TraineePerformanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Show List of Trainees
        public IActionResult Index()
        {
            var trainees = _context.TraineePerformances.ToList();
            return View(trainees);
        }

        // ✅ Fetch Trainee Details by VTR ID
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = _context.TraineePerformances.FirstOrDefault(t => t.VTRId == id);

            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // ✅ Generate PDF Report for a Trainee
        public IActionResult GenerateReport(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = _context.TraineePerformances.FirstOrDefault(t => t.VTRId == id);

            if (trainee == null)
            {
                return NotFound();
            }

            return new ViewAsPdf("ReportTemplate", trainee)
            {
                FileName = $"Trainee_Report_{trainee.VTRId}.pdf"
            };
        }
    }
}
