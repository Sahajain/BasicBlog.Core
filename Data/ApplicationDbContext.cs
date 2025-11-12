using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BasicBlog.Core.Models;

namespace BasicBlog.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Blog entity
            builder.Entity<Blog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.BlogText).IsRequired();
                entity.Property(e => e.CreatedOn).IsRequired();

                // Configure relationship with ApplicationUser
                entity.HasOne(e => e.BlogOwner)
                    .WithMany()
                    .HasForeignKey(e => e.BlogOwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Comments
                entity.HasMany(e => e.Comments)
                    .WithOne(c => c.Blog)
                    .HasForeignKey(c => c.BlogId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Comment entity
            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CommentText).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.TimeCommented).IsRequired();
            });
        }
    }
}

