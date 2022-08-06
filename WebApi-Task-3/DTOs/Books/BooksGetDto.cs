using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Task_3.Models;

namespace WebApi_Task_3.DTOs.Books
{
    public class BooksGetDto
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

    public class CategoryInBooksGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BooksCount { get; set; }

    }
}
