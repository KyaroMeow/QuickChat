using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string? Avatarurl { get; set; }

    public DateTime? Lastonline { get; set; }

    public bool? Isonline { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();
}
