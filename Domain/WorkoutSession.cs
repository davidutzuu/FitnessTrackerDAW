namespace FitnessTrackerPAW.Domain
{
    public class WorkoutSession
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }

        // Metrica de la 1 la 10 pentru evaluarea gradului de "pump" obtinut
        public int PumpLevel { get; set; }

        // Relatie 1-N cu User
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        // Relatie N-N cu Exercise
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}