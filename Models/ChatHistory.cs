using System;
using System.Collections.Generic;

namespace KimTaiPhongThuy.Models
{
    public partial class ChatHistory
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; } = null!;
        public int Sender { get; set; }
        public DateTime? Timestamp { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
