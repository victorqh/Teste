using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appUPN.Models
{
    [Table("chatmessages")]
    public class ChatMessage
    {
        [Key]
        [Column("chatmessageid")]
        public int ChatMessageId { get; set; }

        [Column("userid")]
        public int? UserId { get; set; }

        [Required]
        [Column("sessionid")]
        [StringLength(200)]
        public string SessionId { get; set; } = string.Empty;

        [Required]
        [Column("role")]
        [StringLength(20)]
        public string Role { get; set; } = string.Empty; // 'user' o 'assistant'

        [Required]
        [Column("message")]
        public string Message { get; set; } = string.Empty;

        [Column("createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
