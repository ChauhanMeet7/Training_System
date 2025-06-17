using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class TrainingCompletionReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingCompletionReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 Display All Training Completion Records
        public async Task<IActionResult> Index()
        {
            var reports = await _context.TrainingCompletionReports.ToListAsync();
            return View(reports);
        }

        // 📌 Show Create Page
        public IActionResult Create()
        {
            return View();
        }

        // 📌 Save New Training Completion Report
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingCompletionReport report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // 📌 Show Edit Page
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var report = await _context.TrainingCompletionReports.FindAsync(id);
            if (report == null) return NotFound();

            return View(report);
        }

        // 📌 Save Edited Training Completion Report
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingCompletionReport report)
        {
            if (id != report.ReportID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // 📌 Show Delete Confirmation Page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var report = await _context.TrainingCompletionReports.FindAsync(id);
            if (report == null) return NotFound();

            return View(report);
        }

        // 📌 Confirm Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.TrainingCompletionReports.FindAsync(id);
            if (report != null)
            {
                _context.TrainingCompletionReports.Remove(report);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // 📌 View Report Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var report = await _context.TrainingCompletionReports.FindAsync(id);
            if (report == null) return NotFound();

            return View(report);
        }

        // 📌 Generate PDF Report
        public async Task<IActionResult> GeneratePDF(int id)
        {
            var report = await _context.TrainingCompletionReports.FindAsync(id);
            if (report == null) return NotFound();

            using (MemoryStream stream = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 20, 20, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                doc.Open();

                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                doc.Add(new Paragraph("Training Completion Report\n\n", titleFont));
                doc.Add(new Paragraph($"Trainee ID: {report.StudentId}", contentFont));
                doc.Add(new Paragraph($"Training Start Date: {report.TrainingStartDate.ToShortDateString()}", contentFont));
                doc.Add(new Paragraph($"Training End Date: {report.TrainingEndDate.ToShortDateString()}", contentFont));
                doc.Add(new Paragraph($"Completion Status: {report.CompletionStatus}", contentFont));
                doc.Add(new Paragraph($"Written Test Score: {report.WrittenTestScore}", contentFont));
                doc.Add(new Paragraph($"Performance Remarks: {report.PerformanceRemarks}", contentFont));

                doc.Close();
                writer.Close();

                return File(stream.ToArray(), "application/pdf", $"TrainingCompletion_{report.StudentId}.pdf");
            }
        }
    }
}
