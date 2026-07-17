using GymManagement.BLL.MappProfile;
using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL;
using GymManagement.DAL.DataSeeding;
using GymManagement.DAL.DbContexts;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Class;
using GymManagement.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagement.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IPlanRepository , PlanRepository>();
            builder.Services.AddScoped<IPlanService , PlanService>();
            builder.Services.AddScoped<ISessionRepository , SessionRepository>();
            builder.Services.AddScoped<IBookingRepository , BookingRepository>();
            builder.Services.AddScoped<IMembershipRepository , MembershipRepository>();
            builder.Services.AddScoped<IMemberService , MemberService>();
            builder.Services.AddScoped<IMembershipService , MembershipService>();
            builder.Services.AddScoped<ISessionService , SessionService>();
            builder.Services.AddScoped<ITrainerService , TrainerService>();
            builder.Services.AddScoped<IAnalyticsService , AnalyticsService>();
            builder.Services.AddScoped<IBookingService , BookingService>();
            builder.Services.AddScoped<IAttachmentService , AttachmentService>();
            builder.Services.AddScoped(typeof(IGenericRepository<>) , typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
            builder.Services.AddDbContext<GymDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
            });

            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<GymDbContext>();

            var app = builder.Build();

            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await context.Database.MigrateAsync();
            }

            await IdentityDataSeeding.SeedIdentityDataAsync(userManager , roleManager , logger);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
