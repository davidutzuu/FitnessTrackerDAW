namespace FitnessTrackerPAW.Domain
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TargetMuscle { get; set; } = string.Empty;

        // Relatie N-N
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}