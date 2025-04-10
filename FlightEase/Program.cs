using FlightEase.Data;
using FlightEase.Domains.Data;
using FlightEase.Domains.Entities;
using FlightEase.Repositories;
using FlightEase.Repositories.Interfaces;
using FlightEase.Services;
using FlightEase.Services.Interfaces;
using FlightEase.Util.Mail.Interfaces;
using FlightEase.Util.Mail;
using FlightEase.Util.PDF.Interfaces;
using FlightEase.Util.PDF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//services
builder.Services.AddTransient<IDAO<Flight>, FlightDAO>();
builder.Services.AddTransient<IService<Flight>, FlightService>();
//Ticket
builder.Services.AddTransient<IDAO<Ticket>, TicketDAO>();
builder.Services.AddTransient<IService<Ticket>, TicketService>();
//Season
builder.Services.AddTransient<IDAO<Season>, SeasonDAO>();
builder.Services.AddTransient<IService<Season>, SeasonService>();
//ClassType
builder.Services.AddTransient<IDAO<ClassType>, ClassTypeDAO>();
builder.Services.AddTransient<IService<ClassType>, ClassTypeService>();
//Meal
builder.Services.AddTransient<IDAO<Meal>, MealDAO>();
builder.Services.AddTransient<IService<Meal>, MealService>();
//Seat
builder.Services.AddTransient<IDAO<Seat>, SeatDAO>();
builder.Services.AddTransient<IService<Seat>, SeatService>();

//mail en pdf
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
// Configuration.GetSection("EmailSettings")) zal de instellingen opvragen uit de
//AppSettings.json file en vervolgens wordt er een emailsettings - object
//aangemaakt en de waarden worden geïnjecteerd in het object

builder.Services.AddSingleton<IEmailSend, EmailSend>();
builder.Services.AddSingleton<ICreatePDF, CreatePDF>();

//Add automapper
builder.Services.AddAutoMapper(typeof(Program));

//session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "be.VIVES.Session";
    //3 minuten
    options.IdleTimeout = TimeSpan.FromMinutes(3);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//add session
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
