using System;
using System.Linq;
using System.Threading.Tasks;
using BasicBlog.Core.Data;
using BasicBlog.Core.Models;
using BasicBlog.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BasicBlog.Core.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Blog
        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            var totalCount = await _db.Blogs.CountAsync();
            var blogs = await _db.Blogs
                .OrderByDescending(b => b.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var users = await _db.Users.ToListAsync();
            var viewModel = new ShowAllBlogsViewModel
            {
                BlogList = blogs,
                Users = users,
                ViewOrEdit = "View",
                PageNumber = page,
                PageCount = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> ViewOwnBlogs(int page = 1, int pageSize = 3)
        {
            var userId = _userManager.GetUserId(User);
            var query = _db.Blogs.Where(b => b.BlogOwnerId == userId);

            var totalCount = await query.CountAsync();
            var blogs = await query
                .OrderByDescending(b => b.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var users = await _db.Users.ToListAsync();
            var viewModel = new ShowAllBlogsViewModel
            {
                BlogList = blogs,
                Users = users,
                ViewOrEdit = "Edit",
                PageNumber = page,
                PageCount = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
            return View("Index", viewModel);
        }

        public async Task<IActionResult> Read(int id)
        {
            var blog = await _db.Blogs
                .Include(b => b.Comments)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return NotFound();

            var viewModel = new ReadWithCommentsViewModel
            {
                Blog = blog,
                Comment = new Comment()
            };
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return View(new Blog());
            }

            var blog = await _db.Blogs.FindAsync(id);
            if (blog == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            if (blog.BlogOwnerId != currentUserId) return Forbid();

            return View(blog);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", blog);
            }

            if (blog.Id == 0)
            {
                blog.CreatedOn = DateTime.Now;
                blog.BlogOwnerId = _userManager.GetUserId(User);
                _db.Blogs.Add(blog);
            }
            else
            {
                var existing = await _db.Blogs.FirstOrDefaultAsync(b => b.Id == blog.Id);
                if (existing == null) return View("Edit", blog);

                var currentUserId = _userManager.GetUserId(User);
                if (existing.BlogOwnerId != currentUserId) return Forbid();

                existing.Title = blog.Title;
                existing.BlogText = blog.BlogText;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Read", new { id = blog.Id == 0 ? _db.Blogs.OrderByDescending(b => b.Id).Select(b => b.Id).First() : blog.Id });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _db.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            if (blog.BlogOwnerId != currentUserId) return Forbid();

            _db.Blogs.Remove(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction("ViewOwnBlogs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(ReadWithCommentsViewModel viewModel)
        {
            var comment = viewModel.Comment;
            comment.TimeCommented = DateTime.Now;
            comment.Username = User.Identity?.Name ?? "unknown";
            comment.BlogId = viewModel.Blog.Id;
            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();
            return RedirectToAction("Read", new { id = viewModel.Blog.Id });
        }
    }
}

