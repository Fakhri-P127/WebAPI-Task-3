using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Task_3.DAL.Configurations;
using WebApi_Task_3.Models;

namespace WebApi_Task_3.DAL
{
    public class APIDbContext:IdentityDbContext<AppUser>
    {
        public APIDbContext(DbContextOptions<APIDbContext> opt) : base(opt)
        {

        }

        public DbSet<Book>  Books{ get; set; }
        public DbSet<Category> Categories { get; set; }
         

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfig());
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
