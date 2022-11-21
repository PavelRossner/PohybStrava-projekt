namespace PohybStrava.Models
{
    public class UserActivity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int ActivitiesId { get; set; }
        public virtual Activities Activities { get; set; }
    }
}
