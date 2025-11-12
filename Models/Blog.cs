using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BasicBlog.Core.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }

        [Display(Name = "Your Blog Text")]
        [Required]
        public string BlogText { get; set; } = string.Empty;

        public string? BlogOwnerId { get; set; }
        public ApplicationUser? BlogOwner { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

