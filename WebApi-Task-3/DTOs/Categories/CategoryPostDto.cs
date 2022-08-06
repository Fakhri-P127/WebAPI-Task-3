using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Task_3.DTOs.Categories
{
    public class CategoryPostDto
    {
        public string Name { get; set; }

    }

    public class CategoryPostDtoValidation : AbstractValidator<CategoryPostDto>
    {
        public CategoryPostDtoValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("enter a category name").MaximumLength(30).WithMessage("max length 30 characters");
        }
    }
}
