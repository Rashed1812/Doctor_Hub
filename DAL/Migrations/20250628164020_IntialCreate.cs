using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorJoinRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    EducationalDegree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificatesAndCourses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GraduationCertificatePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTermsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorJoinRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalSpecialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisibleToPatient = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalSpecialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partnerships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PartnershipType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartnershipDetails = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CompanyDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ServicesOffered = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProposedBudget = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PreferredContactMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsUrgent = table.Column<bool>(type: "bit", nullable: false),
                    ReferenceSource = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partnerships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    ConsultationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BirthCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkOrStudyPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalSpecialtyId = table.Column<int>(type: "int", nullable: false),
                    ConsultationFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    ReceiptSent = table.Column<bool>(type: "bit", nullable: false),
                    ReceiptToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ChronicDiseases = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Allergies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PastSurgeries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentMedications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InfectiousDiseases = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasRespiratoryIssues = table.Column<bool>(type: "bit", nullable: true),
                    HasDigestiveIssues = table.Column<bool>(type: "bit", nullable: true),
                    HasNeurologicalIssues = table.Column<bool>(type: "bit", nullable: true),
                    HasUrinaryOrReproductiveIssues = table.Column<bool>(type: "bit", nullable: true),
                    FamilyDiseases = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MentalHealthIssues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PsychiatricMedications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousTestsResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastVaccinationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecommendedTreatment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.ConsultationId);
                    table.ForeignKey(
                        name: "FK_Consultations_MedicalSpecialties_MedicalSpecialtyId",
                        column: x => x.MedicalSpecialtyId,
                        principalTable: "MedicalSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSpecialties",
                columns: table => new
                {
                    DoctorJoinRequestId = table.Column<int>(type: "int", nullable: false),
                    MedicalSpecialtyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSpecialties", x => new { x.DoctorJoinRequestId, x.MedicalSpecialtyId });
                    table.ForeignKey(
                        name: "FK_DoctorSpecialties_DoctorJoinRequests_DoctorJoinRequestId",
                        column: x => x.DoctorJoinRequestId,
                        principalTable: "DoctorJoinRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorSpecialties_MedicalSpecialties_MedicalSpecialtyId",
                        column: x => x.MedicalSpecialtyId,
                        principalTable: "MedicalSpecialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ConsultationId = table.Column<int>(type: "int", nullable: true),
                    CustomerTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerImage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerReviews_Consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "Consultations",
                        principalColumn: "ConsultationId");
                });

            migrationBuilder.InsertData(
                table: "CustomerReviews",
                columns: new[] { "Id", "ConsultationId", "CustomerImage", "CustomerName", "CustomerTitle", "IsApproved", "IsVisible", "Rating", "ReviewDate", "ReviewText" },
                values: new object[,]
                {
                    { 1, null, null, "أحمد محمد", "مريض", true, true, 5, new DateTime(2025, 6, 14, 19, 40, 19, 391, DateTimeKind.Local).AddTicks(1595), "خدمة ممتازة ومتابعة دقيقة من الدكتور. استفدت كثيراً من برنامج التغذية العلاجية وحققت نتائج رائعة." },
                    { 2, null, null, "فاطمة علي", "مريضة", true, true, 5, new DateTime(2025, 5, 28, 19, 40, 19, 391, DateTimeKind.Local).AddTicks(1636), "أشكر الدكتور على الاهتمام والمتابعة المستمرة. العلاج النفسي ساعدني كثيراً في تحسين حالتي." },
                    { 3, null, null, "محمد سالم", "مريض", true, true, 4, new DateTime(2025, 6, 7, 19, 40, 19, 391, DateTimeKind.Local).AddTicks(1641), "تجربة جيدة جداً مع العيادة. الطاقم الطبي محترف والخدمة سريعة ومنظمة." }
                });

            migrationBuilder.InsertData(
                table: "MedicalSpecialties",
                columns: new[] { "Id", "Description", "IconClass", "IsVisibleToPatient", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "خطط غذائية متخصصة وعلاج اضطرابات التغذية", "🥗", true, "التغذية العلاجية", 30m },
                    { 2, "استشارات جراحية متقدمة وتقييم الحالات", "⚕️", true, "الجراحة العامة", 50m },
                    { 3, "علاج الاضطرابات النفسية والدعم النفسي", "🧠", true, "الطب النفسي", 40m },
                    { 4, null, "❤️", false, "أمراض القلب", null },
                    { 5, null, "🩺", false, "الأمراض الجلدية", null },
                    { 6, null, "👶", false, "طب الأطفال", null },
                    { 7, null, "🦴", false, "العظام", null },
                    { 8, null, "🧠", false, "الأعصاب", null },
                    { 9, null, "🏥", false, "تخصص آخر", null }
                });

            migrationBuilder.InsertData(
                table: "Partnerships",
                columns: new[] { "Id", "AdminNotes", "CompanyAddress", "CompanyDescription", "CompanyName", "ContactPersonName", "Email", "FullPhoneNumber", "IsUrgent", "PartnershipDetails", "PartnershipType", "PhoneNumber", "Position", "PreferredContactMethod", "ProposedBudget", "ReferenceSource", "RejectionReason", "ResponseDate", "ServicesOffered", "Status", "SubmissionDate", "Website", "WhatsAppNumber" },
                values: new object[,]
                {
                    { 1, null, "الرياض، المملكة العربية السعودية", null, "مستشفى الأمل التخصصي", "د. خالد محمود", "khalid.mahmoud@alamalhospital.com", "+966501234567", false, "نبحث عن شراكة لتقديم خدمات استشارية في التغذية العلاجية لمرضانا.", "شراكة طبية", "+966501234567", "مدير المستشفى", null, null, null, null, null, null, 1, new DateTime(2025, 4, 28, 19, 40, 19, 391, DateTimeKind.Local).AddTicks(1673), null, null },
                    { 2, null, "جدة، المملكة العربية السعودية", null, "شركة الإبداع للتسويق", "ليلى فهد", "layla.fahad@alibdaa.com", "+966557654321", false, "نرغب في إطلاق حملة إعلانية مشتركة لزيادة الوعي بالخدمات الطبية.", "شراكة إعلانية", "+966557654321", "مدير التسويق", null, null, null, null, null, null, 0, new DateTime(2025, 5, 28, 19, 40, 19, 391, DateTimeKind.Local).AddTicks(1678), null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_MedicalSpecialtyId",
                table: "Consultations",
                column: "MedicalSpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReviews_ConsultationId",
                table: "CustomerReviews",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialties_MedicalSpecialtyId",
                table: "DoctorSpecialties",
                column: "MedicalSpecialtyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerReviews");

            migrationBuilder.DropTable(
                name: "DoctorSpecialties");

            migrationBuilder.DropTable(
                name: "Partnerships");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "DoctorJoinRequests");

            migrationBuilder.DropTable(
                name: "MedicalSpecialties");
        }
    }
}
