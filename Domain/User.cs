using Microsoft.AspNetCore.Identity;

namespace FitnessTrackerPAW.Domain
{
    // Moștenind IdentityUser, primim automat Id, UserName, Email, PasswordHash etc.
    public class User : IdentityUser
    {
        public ICollection<WorkoutSession> WorkoutSessions { get; set; } = new List<WorkoutSession>();
        public ICollection<Supplement> Supplements { get; set; } = new List<Supplement>();
    }
}