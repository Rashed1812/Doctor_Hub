using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Models
{
    public class CourseVideo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string GoogleDriveLink { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
