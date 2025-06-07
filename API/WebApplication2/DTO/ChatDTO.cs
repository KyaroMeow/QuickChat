namespace WebApplication2.DTO
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }

    public class ChatCreateDTO
    {
        public List<int> UserIds { get; set; } = new List<int>();
    }

    public class ChatUpdateDTO
    {
        public List<int>? UserIds { get; set; }
    }
}
