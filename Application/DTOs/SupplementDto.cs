namespace FitnessTrackerPAW.Application.DTOs
{
    public class SupplementDto
    {
        public string UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CaloriesPerServing { get; set; }
        public int ProteinPerServing { get; set; }
        public bool IsMassGainer { get; set; }
    }
}