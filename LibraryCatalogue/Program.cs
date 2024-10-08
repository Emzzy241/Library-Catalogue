using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibraryCatalogue.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryCatalogue.Models;
    class Program
    {
        static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // Configure entity framework and Identity
            builder.Services.AddDbContext<LibraryCatalogueContext>(dbContextOptions =>
                dbContextOptions.UseMySql(
                    builder.Configuration["ConnectionStrings:DefaultConnection"],
                    ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"])
                )
            );

            builder.Services.AddIdentity<LibraryUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<LibraryCatalogueContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            WebApplication app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/500");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Custom error handling middleware
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            // Role seeding logic (you might want to make this async and handle exceptions)
            var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<LibraryUser>>();

            string[] roleNames = { "Librarian", "Patron", "Admin" };

            async Task RoleDecider()
            {
                foreach (var roleName in roleNames)
                {
                    var roleExists = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }

            RoleDecider();

            app.Run();
        }
    }











/*

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibraryCatalogue.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryCatalogue;

  class Program
  {
    static void Main(string[] args)
    {

      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

    // Configure entity framework and Identity
      builder.Services.AddDbContext<LibraryCatalogueContext>(
                        dbContextOptions => dbContextOptions
                          .UseMySql(
                            builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
                          )
                        )
                      );
      
      /* New code below!!
        We set up Identity as a service with the line builder.Services.AddIdentity<ApplicationUser, IdentityRole>(). Notice that we specify <ApplicationUser, IdentityRole> — these are the two models that we're using to designate the user and the role. Just like IdentityUser, IdentityRole is a built-in class to Identity, and it allows us to use the default configurations for roles. We won't be configuring roles beyond the defaults, so we use the built-in IdentityRole class here.
        We chain two more method calls to the Identity service that we set up: .AddEntityFrameworkStores<LibraryCatalogueContext>() and .AddDefaultTokenProviders();. The first method ensures that the Identity user data is saved via EF Core to our database (as represented by the LibraryCatalogueContext class). The second method sets up Identity's providers for tokens, which are created during password reset or two factor authentication, for example. Note that we won't go over how to implement either of those two things in the coursework, and you are encouraged to look into them on your own.
        Finally, we configure our web application app to .UseAuthentication() and .UseAuthorization(). Remember two things here:

        Whenever we call a method on our WebApplication app, we are configuring how our application handles HTTP requests (the "pipeline"). We configure the request pipeline by setting up middleware. Middleware is software that we add to our request pipeline that determines how the request should be processed. Each middleware decides whether to do some work, or to pass the request onto the next middleware. To optionally review more about this topic, visit the MS Docs.
        The order in which we set up the middleware matters! If these methods are called in the wrong order, you may run into unhandled exceptions or issues logging in with Identity. Fortunately, the Microsoft Docs has a list of how middleware should be ordered.
      

      builder.Services.AddMvc();
      builder.Services.AddEntityFrameworkMySql();
      // override Identity's default settings by configuring our Identity service

      // Add identity and role services
      builder.Services.AddIdentity<LibraryUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<LibraryCatalogueContext>()
                .AddDefaultTokenProviders();

        // WORKING WITH USER ROLES IN APP
        builder.Services.AddAuthorization( options =>
        {
          options.AddPolicy("LibrarianPolicy", policy => policy.RequireRole("Librarian"));
          options.AddPolicy("PatrinPolicy", policy => policy.RequireRole("Patron"));

        });

        // Default Password settings. For Production mode
        // The middleware for better passwords
      builder.Services.Configure<IdentityOptions>(options =>
      {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

      });
      
       /*
       
        // Changing the default settings only for development mode... Do not do it for production mode
        builder.Services.Configure<IdentityOptions>(options =>
        {
          options.Password.RequireDigit = false;
          options.Password.RequireLowercase = false;
          options.Password.RequireNonAlphanumeric = false;
          options.Password.RequireUppercase = false;
          options.Password.RequiredLength = 0;
          options.Password.RequiredUniqueChars = 0;

          
            The configuration above allows us to input a password of a single character to create a new user. Even though the RequiredLength property is 0, we can't actually put in an empty password because we have a validation attribute in place that states that some input for the RegisterViewModel.Password property is required.
            Keep in mind that the above settings should never be used in a production environment — only during development to make our lives a bit easier.
            Finally, note that when we change our password requirements in Program.cs, we need to make a corresponding update to our [RegularExpression] validation attribute for the RegisterViewModel.Password property.
        });
          */

        /*
          Update the Startup.cs or Program.cs (for Middleware Configuration)
          If you're using ASP.NET Core 6 or higher, you need to ensure that the middleware for handling AccessDeniedPath is properly configured.
      
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login"; // Redirect to login if not authenticated
            options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect to AccessDenied page
        });


      WebApplication app = builder.Build();

      // app.UseDeveloperExceptionPage();
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        /*
            Purpose of app.UseHsts();
            When you add app.UseHsts(); to your middleware pipeline:
            Enforces HTTPS: HSTS instructs the browser to always use HTTPS instead of HTTP for all subsequent requests to the web application. Once HSTS is enabled, if a user tries to connect to your website over HTTP, the browser will automatically switch to HTTPS.
            Prevents Protocol Downgrade Attacks: HSTS helps prevent attackers from tricking a user’s browser into using a less secure HTTP connection, which can be exploited for man-in-the-middle (MITM) attacks.
            Instructs the Browser to Cache the HTTPS Policy: The browser caches the HSTS policy for a specified period (the "max-age"), which means that even if a user manually tries to connect via HTTP, the browser will still use HTTPS.
    
    }
    else
    {
        // Don't enable HSTS in development
        app.UseDeveloperExceptionPage();
    }

      app.UseRouting();

      // New code below!
      app.UseAuthentication(); 
      app.UseAuthorization();

      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}"
        );

        // Configure Role Management
        // Seed roles into your application during startup to ensure you have a "Librarian" and "Patron" role from the beginning.
        var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<LibraryUser>>();

        string[] roleNames = {"Librarian", "Patron", "Admin"};

        // Wriitng a Method for dsetermining whether a certain role exists 
        async void  RoleDecider()
        {
          foreach (var roleName in roleNames)
          {
              var roleExists = await roleManager.RoleExistsAsync(roleName);
              if(!roleExists)
              {
                  await roleManager.CreateAsync(new IdentityRole(roleName));
              }
          }
          
        }

        RoleDecider();

        // Optionally create a default Library user
        // async void DefaultUser()
        // {
        //     var librarianUser = await userManager.FindByEmailAsync("librarian@library.com");
        //     if (librarianUser == null)
        //     {
        //         var librarian = new ApplicationUser
        //         {
        //             UserName = "librarian@library.com",
        //             Email = "librarian@library.com"
        //         };
        //         var result = await userManager.CreateAsync(librarian, "Librarian123!");
        //         if (result.Succeeded)
        //         {
        //             await userManager.AddToRoleAsync(librarian, "Librarian");
        //         }
        //     }
        // }

        // This setup tells the application to redirect to the /Error/{0} page when an error occurs, where {0} is replaced by the HTTP status code.
        app.UseStatusCodePagesWithReExecute("/Error/{0}");
        // Or we can choose to handle each error and create multiple Razor pages for each error

        // configuring application to use cutom 404 page
        // for .NET 6 and later
        // app.UseExceptionHandler("/Error");
        // app.UseStatusCodePages(async context =>
        // {
        //   // When using middleware, you often don’t need explicit actions for each error page. Instead, you configure the middleware to redirect to specific pages based on the error type.
        //     var statusCode = context.HttpContext.Response.StatusCode;
        //     switch (statusCode)
        //     {
        //         case StatusCodes.Status404NotFound:
        //             context.HttpContext.Response.Redirect("/404");
        //             break;
        //         case StatusCodes.Status403Forbidden:
        //             context.HttpContext.Response.Redirect("/403");
        //             break;
        //         case StatusCodes.Status500InternalServerError:
        //             context.HttpContext.Response.Redirect("/500");
        //             break;
        //     }
        // });



      app.Run();
    }
  }

*/