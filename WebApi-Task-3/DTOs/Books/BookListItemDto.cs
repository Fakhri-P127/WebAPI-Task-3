using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Task_3.DTOs.Books
{
    public class BookListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public short PageCount { get; set; }
        public string Description { get; set; }
        public bool? IsHardCover { get; set; } = false;
        public string BookCode { get; set; }
        public CategoryInBooksGetDto Category { get; set; }

    }

 
}
