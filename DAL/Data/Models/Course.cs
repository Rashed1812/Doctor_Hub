using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string TargetAudience { get; set; }
        public string TeaserVideoUrl { get; set; }
        public string Benefits { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public CourseVideo Videos { get; set; }
        public ICollection<CourseChapter> Chapters { get; set; } = new List<CourseChapter>();
        public ICollection<CoursePurchaseRequest> PurchaseRequests { get; set; } = new List<CoursePurchaseRequest>();
    }
}
