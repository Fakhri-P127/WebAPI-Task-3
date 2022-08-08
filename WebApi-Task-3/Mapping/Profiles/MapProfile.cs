using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Task_3.DTOs.Accounts;
using WebApi_Task_3.DTOs.Books;
using WebApi_Task_3.DTOs.Categories;
using WebApi_Task_3.Models;

namespace WebApi_Task_3.Mapping.Profiles
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Book, BooksPostDto>();
            CreateMap<Book, BooksPostDto>();
            CreateMap<Book, BooksGetDto>();
            CreateMap<BooksPostDto, Book>();
            CreateMap<Book, BookListItemDto>();

            CreateMap<CategoryPostDto,Category>();
            CreateMap<Category, CategoryGetDto>();
            CreateMap<Category, CategoryListItemDto>();
            CreateMap<Category, CategoryInBooksGetDto>();

            CreateMap<RegisterDto, AppUser>();

        }
    }
}
