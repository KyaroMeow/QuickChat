using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Message
{
    public int Id { get; set; }

    public int Chatid { get; set; }

    public int Senderid { get; set; }

    public string Text { get; set; } = null!;

    public DateTime? Sentat { get; set; }

    public bool? Isread { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
