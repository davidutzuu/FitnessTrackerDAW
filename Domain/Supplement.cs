namespace FitnessTrackerPAW.Domain
{
    public class Supplement
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CaloriesPerServing { get; set; }
        public int ProteinPerServing { get; set; }
        public bool IsMassGainer { get; set; }
        public DateTime DateConsumed { get; set; }

        // Relatie 1-N inapoi catre User
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
    }
}