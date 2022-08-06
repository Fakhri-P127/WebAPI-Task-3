using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Task_3.DTOs.Books
{
    public class BooksPostDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public short PageCount { get; set; }
        public string Description { get; set; }
        public bool? IsHardCover { get; set; } = false;
        public string BookCode { get; set; }
        public int CategoryId { get; set; }
    }

    public class BooksPostDtoValidation : AbstractValidator<BooksPostDto>
    {
        public BooksPostDtoValidation()
        {
            RuleFor(x => x.Author).NotEmpty().WithMessage("enter name").MaximumLength(50).WithMessage("max length is 50 characters");
            RuleFor(x => x.Title).NotEmpty().WithMessage("enter title").MaximumLength(50).WithMessage("max length is 50 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("enter description").MaximumLength(250).WithMessage("max length is 250 characters");
            RuleFor(x => x.PageCount).NotEmpty().WithMessage("enter page count").GreaterThanOrEqualTo((short)25).WithMessage("min count is 25 pages")
                .LessThan((short)1000).WithMessage("that's not a book bro");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("category id can't be empty");
        }
    }
}
