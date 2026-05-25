using Educationservices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace Educationservices.Data
{
    public class AppDataContext(DbContextOptions<AppDataContext> options) : DbContext(options)
    {
        public DbSet<ProgramModel> Programs { get; set; } = default!;
        public DbSet<SchoolClass> SchoolClasses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Degree> Degrees { get; set; }
    }
}