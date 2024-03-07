using ExamSupportToolAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ExamSupportToolAPI.DataAccess
{
    public class ExaminationSessionDbContext : DbContext
    {
        public ExaminationSessionDbContext(DbContextOptions<ExaminationSessionDbContext> options)
            : base(options)
        {

        }
        public DbSet<ExaminationSession> ExaminationSessions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<CommitteeMember> CommitteeMembers { get; set; }
        public DbSet<ExaminationTicket> ExaminationTickets { get; set; }
        public DbSet<SecretaryMember> SecretaryMembers { get; set; }
        public DbSet<StudentExaminationSession> StudentExaminationSessions { get; set; }
        public DbSet<CommitteeExaminationSession> CommitteeExaminationSessions { get; set; }
        public DbSet<PresentationSchedule> PresentationSchedules { get; set; }
        public DbSet<PresentationScheduleEntry> PresentationScheduleEntries { get; set; }
        public DbSet<StudentPresentation> StudentPresentations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExaminationSession>()
                .HasMany(es => es.Students)
                .WithMany(s => s.ExaminationSessions)
                .UsingEntity<StudentExaminationSession>();

            modelBuilder.Entity<ExaminationSession>()
                .HasMany(ec => ec.CommitteeMembers)
                .WithMany(c => c.ExaminationSessions)
                .UsingEntity<CommitteeExaminationSession>();

            modelBuilder.Entity<ExaminationSession>()
                .HasMany(et => et.ExaminationTickets);

            modelBuilder.Entity<ExaminationSession>()
                .HasOne(es => es.PresentationSchedule)
                .WithOne()
                .HasForeignKey<ExaminationSession>(es => es.PresentationScheduleId);

            modelBuilder.Entity<ExaminationSession>()
                .HasMany(esp => esp.StudentPresentations)
                .WithOne()
                .HasForeignKey(sp => sp.ExaminationSessionId);


            modelBuilder.Entity<PresentationSchedule>()
                .HasMany(ps => ps.PresentationScheduleEntries)
                .WithOne()
                .HasForeignKey(entry => entry.PresentationScheduleId);

            modelBuilder.Entity<PresentationScheduleEntry>()
                .HasOne(entry => entry.Student)
                .WithMany(s => s.PresentationScheduleEntries)
                .HasForeignKey(entry => entry.StudentId);

            modelBuilder.Entity<StudentPresentation>()
                .HasOne(sp => sp.Student)
                .WithMany()
                .HasForeignKey(sp => sp.StudentId);

            modelBuilder.Entity<StudentPresentation>()
                .HasOne(sp => sp.ExaminationTicket)
                .WithMany()
                .HasForeignKey(sp => sp.ExaminationTicketId);

            modelBuilder.Entity<SecretaryMember>()
                .HasMany(es => es.ExaminationSessions);

            modelBuilder.Entity<Student>()
                .HasIndex(u => u.Email);

            modelBuilder.Entity<CommitteeMember>()
                .HasIndex(u => u.Email);

            modelBuilder.Entity<SecretaryMember>()
                .HasIndex(u => u.Email);
            //modelBuilder.SeedData();

            modelBuilder.Entity<Student>()
                .HasIndex(u => u.ExternalId)
                .IsUnique(true);

            modelBuilder.Entity<SecretaryMember>()
                .HasIndex(u => u.ExternalId)
                .IsUnique(true);

            modelBuilder.Entity<CommitteeMember>()
                .HasIndex(u => u.ExternalId)
                .IsUnique(true);

        }
    }
}
