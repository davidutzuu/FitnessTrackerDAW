namespace FitnessTrackerPAW.Application.DTOs
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TargetMuscle { get; set; } = string.Empty;
    }
}