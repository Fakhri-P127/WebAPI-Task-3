using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi_Task_3.DTOs.Accounts;
using WebApi_Task_3.Models;

namespace WebApi_Task_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountsController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,IMapper mapper,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            AppUser user = await _userManager.FindByNameAsync(dto.Username);
            if (user != null) return BadRequest(new ValidationFailure("Username","This user already exists"));

            user = _mapper.Map<AppUser>(dto);

            IdentityResult userResult = await _userManager.CreateAsync(user, dto.Password);

            if (!userResult.Succeeded) return BadRequest(userResult.Errors);
           IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            AppUser user = await _userManager.FindByNameAsync(dto.Username);
            if (user is null) return NotFound();

            bool result = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!result) return BadRequest(new
            {
                code = "Password or Username",
                description = "Password or Username is incorrect."
            });

            List<Claim> claims = new List<Claim>
            {
                new Claim("Id",user.Id),
                new Claim(ClaimTypes.Name, user.Firstname),
                new Claim(ClaimTypes.Surname, user.Lastname),
                new Claim(ClaimTypes.UserData, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            IList<string> roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim("Role", role)));

            string keyStr = "76d615c5-9000-4b4a-97eb-273988423ffb";

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                  issuer: _configuration.GetSection("Jwt:Issuer").Value,
                  audience: _configuration.GetSection("Jwt:Audience").Value,
                  claims:claims,
                  signingCredentials:credentials,
                  expires:DateTime.Now.AddMinutes(3)
                  );

            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            
            return Ok(tokenStr);
        }
        [HttpGet("getuser")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetUserAsync()
        {
            AppUser user = await _userManager.FindByNameAsync("fakhriP127");

            return Ok(user);
        }

        //[HttpGet("createRoles")]
        //public async Task CreateRoles()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("User"));
        //    await _roleManager.CreateAsync(new IdentityRole("Moderator"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //}
    }
}
