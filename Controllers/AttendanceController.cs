using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 Display All Attendance Records
        public IActionResult Index()
        {
            var attendanceList = _context.Attendences
                .Include(a => a.Student)
                .ThenInclude(s => s.Trainee)
                .OrderByDescending(a => a.Date)
                .ToList();

            return View(attendanceList);
        }

        // 📌 Mark Attendance Page
        public IActionResult MarkAttendance()
        {
            ViewBag.Students = _context.GeneralDetails.Include(s => s.Trainee).ToList();
            return View();
        }

        // 📌 Save Attendance Record
        [HttpPost]
        public IActionResult MarkAttendance(int studentId, string status, DateTime date)
        {
            if (string.IsNullOrEmpty(status))
            {
                ModelState.AddModelError("", "Please select an attendance status.");
                return View();
            }

            // Check if attendance already exists for this student on the selected date
            var existingRecord = _context.Attendences
                .FirstOrDefault(a => a.StudentId == studentId && a.Date.Date == date.Date);

            if (existingRecord != null)
            {
                ModelState.AddModelError("", "Attendance for this student on this date already exists.");
                ViewBag.Students = _context.GeneralDetails.Include(s => s.Trainee).ToList();
                return View();
            }

            var attendance = new Attendance
            {
                StudentId = studentId,
                Date = date,
                Status = status,
                TimeRecorded = DateTime.Now.TimeOfDay
            };

            _context.Attendences.Add(attendance);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // 📌 Delete Attendance Record
        [HttpPost]
        public IActionResult DeleteAttendance(int studentId, DateTime date)
        {
            var attendanceRecord = _context.Attendences
                .FirstOrDefault(a => a.StudentId == studentId && a.Date.Date == date.Date);

            if (attendanceRecord != null)
            {
                _context.Attendences.Remove(attendanceRecord);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Attendance record deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "No attendance record found.";
            }

            return RedirectToAction("MarkAttendance");
        }

        // 📌 Monthly Attendance Report
        public IActionResult MonthlyReport(int month = 0, int year = 0, string vtrId = "")
        {
            if (month == 0) month = DateTime.Now.Month;
            if (year == 0) year = DateTime.Now.Year;

            // Fetch attendance records for the given month & year
            var attendanceQuery = _context.Attendences
                .Include(a => a.Student)
                .ThenInclude(s => s.Trainee)
                .Where(a => a.Date.Month == month && a.Date.Year == year);

            // Apply VTR ID filter if provided
            if (!string.IsNullOrEmpty(vtrId))
            {
                attendanceQuery = attendanceQuery.Where(a => a.Student.Trainee.VTRId == vtrId);
            }

            var attendanceRecords = attendanceQuery.ToList();

            if (!attendanceRecords.Any())
            {
                ViewBag.Message = "No attendance records found for the selected criteria.";
                ViewBag.SelectedMonth = month;
                ViewBag.SelectedYear = year;
                ViewBag.SelectedVtrId = vtrId;
                return View(new List<StudentAttendanceSummary>());
            }

            // Group attendance by Student and calculate totals
            var report = attendanceRecords
                .GroupBy(a => a.Student)
                .Select(g => new StudentAttendanceSummary
                {
                    VTRId = g.Key.Trainee != null ? g.Key.Trainee.VTRId : "N/A",
                    StudentName = $"{g.Key.FirstName} {g.Key.LastName}",
                    TotalPresent = g.Count(a => a.Status == "Present"),
                    TotalAbsent = g.Count(a => a.Status == "Absent"),
                    TotalLate = g.Count(a => a.Status == "Late"),
                    TotalDays = g.Count()
                })
                .ToList();

            ViewBag.SelectedMonth = month;
            ViewBag.SelectedYear = year;
            ViewBag.SelectedVtrId = vtrId;

            return View(report);
        }
    }

    // 📌 Summary Model for Attendance Report
    public class StudentAttendanceSummary
    {
        public string VTRId { get; set; }
        public string StudentName { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalLate { get; set; }
        public int TotalDays { get; set; }

        public double AttendancePercentage => TotalDays > 0 ? ((double)TotalPresent / TotalDays) * 100 : 0;
    }
}
