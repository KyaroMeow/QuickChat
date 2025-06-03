namespace WebApplication2.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string Text { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }

    public class MessageCreateDTO
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string Text { get; set; } = null!;
    }

    public class MessageUpdateDTO
    {
        public bool? IsRead { get; set; }
    }
}
