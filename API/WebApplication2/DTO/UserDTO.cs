namespace WebApplication2.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public DateTime? LastOnline { get; set; }
        public bool IsOnline { get; set; }
        public List<int> ChatIds { get; set; } = new List<int>();
    }

    public class UserCreateDTO
    {
        public string Login { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? AvatarUrl { get; set; }
    }

    public class UserUpdateDTO
    {
        public string? Username { get; set; }
        public string? AvatarUrl { get; set; }
        public bool? IsOnline { get; set; }
    }

    public class UserLoginDTO
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
