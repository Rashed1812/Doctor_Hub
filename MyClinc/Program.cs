using DAL.Data;
using DAL.Data.Repositories.ConsultationRepo;
using DAL.Data.Repositories.DoctorRequest;
using DAL.Data.Repositories.MedicalRepo;
using DAL.Data.Repositories.PartnershipRepo;
using DAL.Data.Repositories.WhatsAppRepo;
using Microsoft.EntityFrameworkCore;
using MyClinc.Services;
using System;

namespace MyClinc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            // إضافة Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2); // انتهاء الجلسة بعد ساعتين من عدم النشاط
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "MyClinc.Admin.Session";
            });
            
            builder.Services.AddDbContext<ClincDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaiultConnection"));
            });
            builder.Services.AddScoped<IDoctorJoinRequestRepo, DoctorJoinRequestRepo>();
            builder.Services.AddScoped<IMedicalSpecialtyRepository, MedicalSpecialtyRepository>();
            builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
            builder.Services.AddScoped<IPartnershipRepository, PartnershipRepository>();
            builder.Services.AddScoped<IPdfService, PdfService>();
            builder.Services.AddScoped<IWhatsAppBotService, WhatsAppBotService>();
            builder.Services.AddScoped<IWhatsAppApiService, WhatsAppApiService>();
            builder.Services.AddScoped<IWhatsAppSessionRepository, WhatsAppSessionRepository>();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                //The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

                   app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            // إضافة Session middleware
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}


