using BasicBlog.Core.Models;

namespace BasicBlog.Core.ViewModels
{
    public class ReadWithCommentsViewModel
    {
        public Blog Blog { get; set; } = new Blog();
        public Comment Comment { get; set; } = new Comment();
    }
}

