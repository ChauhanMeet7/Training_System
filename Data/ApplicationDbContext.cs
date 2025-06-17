using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
//using WebApplication1.netCore.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<Trainee> Vtr { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Gnfc> GeneralDetails { get; set; }

        public DbSet<PersonalDetail> PersonalDetails { get; set; }
        public DbSet<Family> FamilyDetails { get; set; }
        public DbSet<Academic> AcademicDetails { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<Plant> Plants { get; set; }

        public DbSet<Posting> Postings { get; set; }

        public DbSet<Attendance> Attendences { get; set; }

        
        public DbSet<TraineePerformance> TraineePerformances { get; set; }
        public DbSet<TrainingCompletionReport> TrainingCompletionReports { get; set; }


    }
}

