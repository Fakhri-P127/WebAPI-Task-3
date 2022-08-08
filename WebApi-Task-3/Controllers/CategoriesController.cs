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
using WebApi_Task_3.DTOs.Categories;
using WebApi_Task_3.Models;

namespace WebApi_Task_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly APIDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(APIDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Category category = _context.Categories.Include(b => b.Books).FirstOrDefault(b => b.Id == id);
            if (category is null) return BadRequest();
            CategoryGetDto dto = _mapper.Map<CategoryGetDto>(category);
            return Ok(dto);
        }
        [HttpGet("")]
        public IActionResult GetAll(int page = 1, byte itemsPerPage = 8, string title = null)
        {
            var query = _context.Categories.AsQueryable();
           
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Name.Contains(title));
            }
           

            ListDto<CategoryListItemDto> dto = new ListDto<CategoryListItemDto>
            {
                ListItemDtos = _mapper.Map<List<CategoryListItemDto>>(query.Include(q => q.Books)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList()),
                TotalCount = query.Count(),
            };

            return Ok(dto);
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create(CategoryPostDto dto)
        {
            if (dto is null) return NotFound();
            if (_context.Categories.Any(c => c.Name == dto.Name)) return BadRequest();
            Category category = _mapper.Map<Category>(dto);
            if (category is null) return NotFound();
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return StatusCode(201, dto);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, CategoryPostDto dto)
        {
            if (dto is null) return NotFound();

                Category existed = await _context.Categories.FirstOrDefaultAsync(b => b.Id == id);
                if (existed is null) return NotFound();

            existed = _mapper.Map(dto,existed);
            await _context.SaveChangesAsync();
            return Ok(new { category = dto });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(b => b.Id == id);
            if (category is null) return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
