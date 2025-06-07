using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Chat
{
    public int Id { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
