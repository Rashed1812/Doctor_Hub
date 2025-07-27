using System;
using System.Linq;
using DAL.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class ClincDbContext : IdentityDbContext<ApplicationUser>
    {
        public ClincDbContext(DbContextOptions<ClincDbContext> options)
            : base(options)
        {
        }

        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<MedicalSpecialty> MedicalSpecialties { get; set; }
        public DbSet<DoctorJoinRequest> DoctorJoinRequests { get; set; }
        public DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }
        public DbSet<CustomerReview> CustomerReviews { get; set; }
        public DbSet<Partnership> Partnerships { get; set; }
        public DbSet<WhatsAppUserSession> WhatsAppUserSessions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseChapter> CourseChapters { get; set; }
        public DbSet<CoursePurchaseRequest> CoursePurchases { get; set; }
        public DbSet<CourseVideo> CourseVideos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // مهم جداً لتهيئة Identity

            // اجعل جميع الخصائص النصية تدعم Unicode (العربية)
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var stringProps = entity.ClrType
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(string));

                foreach (var prop in stringProps)
                {
                    modelBuilder.Entity(entity.Name)
                                .Property(prop.Name)
                                .IsUnicode(true);
                }
            }

            // تكوين علاقات DoctorSpecialty مع Composite Key
            modelBuilder.Entity<DoctorSpecialty>()
                .HasKey(ds => new { ds.DoctorJoinRequestId, ds.MedicalSpecialtyId });

            modelBuilder.Entity<DoctorSpecialty>()
                .HasOne(ds => ds.DoctorJoinRequest)
                .WithMany(d => d.DoctorSpecialties)
                .HasForeignKey(ds => ds.DoctorJoinRequestId);

            modelBuilder.Entity<DoctorSpecialty>()
                .HasOne(ds => ds.MedicalSpecialty)
                .WithMany(ms => ms.DoctorSpecialties)
                .HasForeignKey(ds => ds.MedicalSpecialtyId);

            // CourseVideo - Course (واحد إلى واحد)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Videos)
                .WithOne(v => v.Course)
                .HasForeignKey<CourseVideo>(v => v.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // CourseChapter - Course (واحد إلى كثير)
            modelBuilder.Entity<CourseChapter>()
                .HasOne(cc => cc.Course)
                .WithMany(c => c.Chapters)
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // CoursePurchaseRequest - Course (واحد إلى كثير)
            modelBuilder.Entity<CoursePurchaseRequest>()
                .HasOne(cpr => cpr.Course)
                .WithMany(c => c.PurchaseRequests)
                .HasForeignKey(cpr => cpr.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // CoursePurchaseRequest - ApplicationUser (واحد إلى كثير)
            modelBuilder.Entity<CoursePurchaseRequest>()
                .HasOne(cpr => cpr.User)
                .WithMany(u => u.CoursePurchaseRequests)
                .HasForeignKey(cpr => cpr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed MedicalSpecialties
            modelBuilder.Entity<MedicalSpecialty>().HasData(
                new MedicalSpecialty
                {
                    Id = 1,
                    Name = "التغذية العلاجية",
                    IconClass = "🥗",
                    Description = "خطط غذائية متخصصة وعلاج اضطرابات التغذية",
                    Price = 30,
                    IsVisibleToPatient = true
                },
                new MedicalSpecialty
                {
                    Id = 2,
                    Name = "الجراحة العامة",
                    IconClass = "⚕️",
                    Description = "استشارات جراحية متقدمة وتقييم الحالات",
                    Price = 50,
                    IsVisibleToPatient = true
                },
                new MedicalSpecialty
                {
                    Id = 3,
                    Name = "الطب النفسي",
                    IconClass = "🧠",
                    Description = "علاج الاضطرابات النفسية والدعم النفسي",
                    Price = 40,
                    IsVisibleToPatient = true
                },
                new MedicalSpecialty
                {
                    Id = 4,
                    Name = "أمراض القلب",
                    IconClass = "❤️",
                    IsVisibleToPatient = false
                },
                new MedicalSpecialty
                {
                    Id = 5,
                    Name = "الأمراض الجلدية",
                    IconClass = "🩺",
                    IsVisibleToPatient = false
                },
                new MedicalSpecialty
                {
                    Id = 6,
                    Name = "طب الأطفال",
                    IconClass = "👶",
                    IsVisibleToPatient = false
                },
                new MedicalSpecialty
                {
                    Id = 7,
                    Name = "العظام",
                    IconClass = "🦴",
                    IsVisibleToPatient = false
                },
                new MedicalSpecialty
                {
                    Id = 8,
                    Name = "الأعصاب",
                    IconClass = "🧠",
                    IsVisibleToPatient = false
                },
                new MedicalSpecialty
                {
                    Id = 9,
                    Name = "تخصص آخر",
                    IconClass = "🏥",
                    IsVisibleToPatient = false
                }
            );

            // Seed CustomerReviews
            modelBuilder.Entity<CustomerReview>().HasData(
                new CustomerReview
                {
                    Id = 1,
                    CustomerName = "أحمد محمد",
                    Rating = 5,
                    ReviewText = "خدمة ممتازة ومتابعة دقيقة من الدكتور. استفدت كثيراً من برنامج التغذية العلاجية وحققت نتائج رائعة.",
                    ReviewDate = DateTime.Now.AddDays(-14),
                    IsApproved = true,
                    CustomerTitle = "مريض",
                    IsVisible = true
                },
                new CustomerReview
                {
                    Id = 2,
                    CustomerName = "فاطمة علي",
                    Rating = 5,
                    ReviewText = "أشكر الدكتور على الاهتمام والمتابعة المستمرة. العلاج النفسي ساعدني كثيراً في تحسين حالتي.",
                    ReviewDate = DateTime.Now.AddMonths(-1),
                    IsApproved = true,
                    CustomerTitle = "مريضة",
                    IsVisible = true
                },
                new CustomerReview
                {
                    Id = 3,
                    CustomerName = "محمد سالم",
                    Rating = 4,
                    ReviewText = "تجربة جيدة جداً مع العيادة. الطاقم الطبي محترف والخدمة سريعة ومنظمة.",
                    ReviewDate = DateTime.Now.AddDays(-21),
                    IsApproved = true,
                    CustomerTitle = "مريض",
                    IsVisible = true
                }
            );

            // Seed Partnerships
            modelBuilder.Entity<Partnership>().HasData(
                new Partnership
                {
                    Id = 1,
                    CompanyName = "مستشفى الأمل التخصصي",
                    ContactPersonName = "د. خالد محمود",
                    Position = "مدير المستشفى",
                    Email = "khalid.mahmoud@alamalhospital.com",
                    PhoneNumber = "+966501234567",
                    FullPhoneNumber = "+966501234567",
                    CompanyAddress = "الرياض، المملكة العربية السعودية",
                    PartnershipType = "شراكة طبية",
                    PartnershipDetails = "نبحث عن شراكة لتقديم خدمات استشارية في التغذية العلاجية لمرضانا.",
                    SubmissionDate = DateTime.Now.AddMonths(-2),
                    Status = PartnershipStatus.Approved
                },
                new Partnership
                {
                    Id = 2,
                    CompanyName = "شركة الإبداع للتسويق",
                    ContactPersonName = "ليلى فهد",
                    Position = "مدير التسويق",
                    Email = "layla.fahad@alibdaa.com",
                    PhoneNumber = "+966557654321",
                    FullPhoneNumber = "+966557654321",
                    CompanyAddress = "جدة، المملكة العربية السعودية",
                    PartnershipType = "شراكة إعلانية",
                    PartnershipDetails = "نرغب في إطلاق حملة إعلانية مشتركة لزيادة الوعي بالخدمات الطبية.",
                    SubmissionDate = DateTime.Now.AddMonths(-1),
                    Status = PartnershipStatus.Pending
                }
            );
        }
    }
}
