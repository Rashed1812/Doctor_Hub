using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public class ClincDbContext : DbContext
    {
        public ClincDbContext(DbContextOptions<ClincDbContext> options)
            : base(options)
        {
        }

        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<MedicalSpecialty> MedicalSpecialties { get; set; }
        public DbSet<DoctorJoinRequest> DoctorJoinRequests { get; set; }
        public DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }
        public DbSet<CustomerReview> CustomerReviews { get; set; } // Added DbSet for CustomerReview
        public DbSet<Partnership> Partnerships { get; set; } // Added DbSet for Partnership
        public DbSet<WhatsAppUserSession> WhatsAppUserSessions { get; set; } // Added DbSet for WhatsApp sessions

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // اجعل جميع الخصائص النصية تدعم اللغة العربية
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


