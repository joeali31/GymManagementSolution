using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name)
                .HasColumnType("varchar")
                .HasMaxLength(20);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasData(
                new Category { Id = 1 , Name = "Cardio"},
                new Category { Id = 2 , Name = "Strength"},
                new Category { Id = 3 , Name = "Yoga"},
                new Category { Id = 4 , Name = "Boxing"},
                new Category { Id = 5 , Name = "CrossFit"}
                );
        }
    }
}
