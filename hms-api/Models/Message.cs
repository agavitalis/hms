using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace HMS.Models
{
    public class Message
    {
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x,x)));
            Subject = subject;
            Content = content;
        }
        public List<MailboxAddress> To { get; }
        public string Subject { get; }
        public string Content { get; }
    }
}
