namespace MvcProject.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string? NameSurname { get; set; } = "";
        public string Username { get; set; }
        public bool Locked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Role { get; set; } = "user";

    }
}
