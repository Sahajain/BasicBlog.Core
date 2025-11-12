using System;
using System.ComponentModel.DataAnnotations;

namespace BasicBlog.Core.Models
{
    public class Comment
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string CommentText { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Left at:")]
        public DateTime TimeCommented { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; } = null!;
    }
}

