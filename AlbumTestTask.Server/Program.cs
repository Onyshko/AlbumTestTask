
using AlbumTestTask.Domain.Entities;
using AlbumTestTask.Repository.Context;
using AlbumTestTask.Repository.Implementations;
using AlbumTestTask.Repository.Interfaces;
using AlbumTestTask.Services.Implementation;
using AlbumTestTask.Services.Interfaces;
using AlbumTestTask.Services.Mapping.Profiles;
using AlbumTestTask.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AlbumTestTask.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped(typeof(IBaseRepository<Album>), typeof(AlbumRepository));
            builder.Services.AddScoped(typeof(IBaseRepository<Photo>), typeof(PhotoRepository));
            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(AlbumProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(PhotoProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(LikeProfile).Assembly);

            builder.Services.AddScoped<IBaseCrudService<AlbumModel>, BaseCrudService<Album, AlbumModel>>();
            builder.Services.AddScoped<IBaseCrudService<PhotoModel>, BaseCrudService<Photo, PhotoModel>>();
            builder.Services.AddScoped<IAlbumService, AlbumService>();


            builder.Services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAuthorization();
            builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.Use(async (ctx, next) =>
            {
                ctx.Response.Headers["Access-Control-Allow-Origin"] = "https://localhost:5173";

                if (HttpMethods.IsOptions(ctx.Request.Method))
                {
                    ctx.Response.Headers["Access-Control-Allow-Headers"] = "*";

                    await ctx.Response.CompleteAsync();
                    return;
                }

                await next();
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.MapIdentityApi<ApplicationUser>();

            app.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager) =>
            {

                await signInManager.SignOutAsync();
                return Results.Ok();

            }).RequireAuthorization();


            app.MapGet("/pingauth", (ClaimsPrincipal user) =>
            {
                var email = user.FindFirstValue(ClaimTypes.Email); // get the user's email from the claim
                return Results.Json(new { Email = email }); ; // return the email as a plain text response
            }).RequireAuthorization();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
