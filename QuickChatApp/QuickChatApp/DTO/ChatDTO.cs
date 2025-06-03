namespace WebApplication2.DTO
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsGroup { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }

    public class ChatCreateDTO
    {
        public string Name { get; set; } = null!;
        public bool IsGroup { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }

    public class ChatUpdateDTO
    {
        public string? Name { get; set; }
        public List<int>? UserIds { get; set; }
    }
}
