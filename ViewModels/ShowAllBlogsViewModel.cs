using System.Collections.Generic;
using BasicBlog.Core.Models;

namespace BasicBlog.Core.ViewModels
{
    public class ShowAllBlogsViewModel
    {
        public IReadOnlyList<Blog> BlogList { get; set; } = new List<Blog>();
        public IReadOnlyList<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public string ViewOrEdit { get; set; } = "View";
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
    }
}

