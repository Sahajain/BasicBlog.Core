using Microsoft.AspNetCore.Identity;
using BasicBlog.Core.Models;

namespace BasicBlog.Core.Data
{
 public static class DbSeeder
 {
 public static async Task SeedAsync(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
 {
 // Ensure roles
 const string adminRole = "Admin";
 if (!await roleManager.RoleExistsAsync(adminRole))
 {
 await roleManager.CreateAsync(new IdentityRole(adminRole));
 }

 // Ensure admin user
 var adminEmail = "admin@local.test";
 var adminUser = await userManager.FindByEmailAsync(adminEmail);
 if (adminUser == null)
 {
 adminUser = new ApplicationUser
 {
 UserName = "admin",
 Email = adminEmail,
 EmailConfirmed = true
 };
 var createResult = await userManager.CreateAsync(adminUser, "P@ssw0rd!");
 if (createResult.Succeeded)
 {
 await userManager.AddToRoleAsync(adminUser, adminRole);
 }
 }

 // Seed sample blogs if none exist
 if (!db.Blogs.Any())
 {
 var blog1 = new Blog
 {
 Title = "Welcome to BasicBlog",
 CreatedOn = DateTime.UtcNow,
 BlogText = "This is a sample blog post created during seeding.",
 BlogOwnerId = adminUser.Id
 };

 var blog2 = new Blog
 {
 Title = "Second Post",
 CreatedOn = DateTime.UtcNow.AddMinutes(-30),
 BlogText = "Another sample post.",
 BlogOwnerId = adminUser.Id
 };

 db.Blogs.AddRange(blog1, blog2);
 await db.SaveChangesAsync();
 }
 }
 }
}
