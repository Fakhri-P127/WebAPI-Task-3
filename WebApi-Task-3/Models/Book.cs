using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Task_3.Models.Base;

namespace WebApi_Task_3.Models
{
    public class Book:BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public short PageCount { get; set; }
        public string Description { get; set; }
        public bool? IsHardCover { get; set; }
        public string BookCode { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
