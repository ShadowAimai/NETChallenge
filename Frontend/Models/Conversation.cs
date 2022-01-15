using System;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{

    public class Conversation
    {
        [Key]
        public int Id { get; set; }
        public string ChatRoom { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime date { get; set; }
    }

}
