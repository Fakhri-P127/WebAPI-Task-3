using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Task_3.DAL;
using WebApi_Task_3.DTOs;
using WebApi_Task_3.DTOs.Books;
using WebApi_Task_3.Models;

namespace WebApi_Task_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly APIDbContext _context;
        private readonly IMapper _mapper;

        public BooksController(APIDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Book book = _context.Books.Include(b=>b.Category).ThenInclude(b=>b.Books).FirstOrDefault(b => b.Id == id);
            if (book is null) return BadRequest();
            BooksGetDto dto = _mapper.Map<BooksGetDto>(book);
            return Ok(dto);
        }
        [HttpGet("")]
        public IActionResult GetAll(int page = 1,byte itemsPerPage = 8,string title=null,bool? cover=null)
        {
            var query =  _context.Books.AsQueryable();
            if (!string.IsNullOrEmpty(title) && cover != null)
            {
                query = query.Where(q => q.Title.Contains(title) && q.IsHardCover==cover);
            }
            else if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Title.Contains(title));
            }
            else if (cover != null)
            {
                query = query.Where(q => q.IsHardCover == cover);
            }

            ListDto<BookListItemDto> dto = new ListDto<BookListItemDto>
            {
                ListItemDtos=  _mapper.Map<List<BookListItemDto>>(query.Include(q => q.Category).ThenInclude(q => q.Books)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList()),
                TotalCount=query.Count(),
            };

            return Ok(dto);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(BooksPostDto dto)
        {
            if (dto is null) return NotFound();
            if (_context.Books.Any(c=> c.BookCode==dto.BookCode)) return BadRequest();


            Book book = _mapper.Map<Book>(dto);
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return StatusCode(201,new { id=book.Id, book = dto});
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id,BooksPostDto dto)
        {
            if (dto is null) return NotFound();

            Book existed = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (existed is null) return NotFound();
            existed = _mapper.Map(dto,existed);

            await _context.SaveChangesAsync();
            return Ok(new {book=dto });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Book book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
