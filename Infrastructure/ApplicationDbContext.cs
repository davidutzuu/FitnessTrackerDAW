using FitnessTrackerPAW.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FitnessTrackerPAW.Infrastructure
{
    // Folosim IdentityDbContext pentru a bifa automat tabelele de securitate
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Maparea claselor noastre catre tabelele din SQL
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<Supplement> Supplements { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Obligatoriu de apelat pentru a nu strica configurarile Identity
            base.OnModelCreating(builder);

            // Configurare cheie primara compusa pentru tabelul de legatura (Many-to-Many)
            builder.Entity<WorkoutExercise>()
                .HasKey(we => new { we.WorkoutSessionId, we.ExerciseId });

            builder.Entity<WorkoutExercise>()
                .HasOne(we => we.WorkoutSession)
                .WithMany(ws => ws.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutSessionId);

            builder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId);
        }
    }
}