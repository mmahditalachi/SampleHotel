using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleHotel.Infrastructure;
using SampleHotel.Models;
using System;
using static System.Formats.Asn1.AsnWriter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>
    (option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(config =>
{
    config.User.RequireUniqueEmail = true;
    config.Password.RequiredLength = 6;
    config.Password.RequireDigit = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
    config.SignIn.RequireConfirmedPhoneNumber = false;
    config.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();
//for custom Email confrimation Time         

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddSingleton<DapperContext>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

if (context.Database.IsSqlServer())
{
    context.Database.Migrate();
}
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

if (!roleManager.Roles.Any())
{
    await roleManager.CreateAsync(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "Admin" });
    await roleManager.CreateAsync(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Client", NormalizedName = "Client" });
}

var user = new ApplicationUser
{
    UserName = "Admin",
    FullName = "Admin Admin",
    EmailConfirmed = true,
    Country = "Turkey",
    City = "Ankara",
    Street = "First Ave",
    No = "12 Ab",
    Email = "admin@admin.com",
    PhoneNumber = "02214891892",   
};

var result = await userManager.CreateAsync(user , "admintest");
await userManager.AddToRoleAsync(user, "Admin");



if (!context.Rooms.Any())
{
    context.Add(new Room
    {
        Name = "Suite room",
        Capacity = 10,
        Floor = 2,
        HasBreakfast = true,
        HasJacuzzi = true,
        ImageUrl = "room1.jpg",
        RoomTypeId = RoomType.Luxury.Id,
        Location = "Jungle view",        
    });
    context.Add(new Room
    {
        Name = "Standard room",
        Capacity = 2,
        Floor = 2,
        HasBreakfast = false,
        HasJacuzzi = true,
        ImageUrl = "room3.jpg",
        RoomTypeId = RoomType.Standard.Id,
        Location = "Street view",
    });
    context.SaveChanges();
}

if (!context.Employees.Any())
{
    context.Add(new Employee
    {
        FirstName = "Ezabella",
        LastName = "Tatarin",
        City = "Izmir",        
        ImageUrl = "manager.png",
        EmployeePositionId = EmployeePosition.Waitress.Id,
        PhoneNumber = "0229856314",
        Street = "st.1",
        DependentFirstName = "Poli",
        DependentLastName = "Tatarin",
        DependentRelation = "Mother",
    });
    context.Add(new Employee
    {
        FirstName = "John",
        LastName = "Big",
        City = "Izmir",
        ImageUrl = "cheff.png",
        EmployeePositionId = EmployeePosition.Chef.Id,
        PhoneNumber = "05236987",
        Street = "st.2",
        DependentFirstName = "kent",
        DependentLastName = "akra",
        DependentRelation = "Father",
    });
    context.SaveChanges();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
