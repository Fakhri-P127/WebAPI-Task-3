using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_Task_3.Models;

namespace WebApi_Task_3.DAL.Configurations
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
            builder.HasIndex(x => x.BookCode).IsUnique();
            builder.Property(x => x.Description).HasMaxLength(250).IsRequired();
            builder.Property(x => x.IsHardCover).HasDefaultValue(false);
            builder.Property(x => x.PageCount).IsRequired();
            builder.Property(x => x.Author).HasMaxLength(30).IsRequired();
        }
    }
}
