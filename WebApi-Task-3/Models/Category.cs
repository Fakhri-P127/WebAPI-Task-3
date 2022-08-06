using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Task_3.Models.Base;

namespace WebApi_Task_3.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
