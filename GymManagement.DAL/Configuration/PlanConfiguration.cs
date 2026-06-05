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
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                    .HasColumnType("varchar")
                    .HasMaxLength(50);

            builder.Property(p => p.Description)
                    .HasColumnType("varchar")
                    .HasMaxLength(200);

            builder.Property(p => p.Price)
                    .HasPrecision(10, 2);

            builder.Property(p => p.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

            builder.HasData(
                new Plan {
                    Id = 1,
                    Name = "Basic Plan",
                    Description = "Basic subscription plan",
                    DurationDays = 30,
                    Price = 300m,
                    IsActive = true
                },
                new Plan
                {
                    Id = 2,
                    Name = "Standard Plan",
                    Description = "Professional subscription plan",
                    DurationDays = 60,
                    Price = 500m,
                    IsActive = false
                },
                new Plan
                {
                    Id = 3,
                    Name = "Premium Plan",
                    Description = "Full features plan",
                    DurationDays = 90,
                    Price = 900m,
                    IsActive = true
                }
                );

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CKDurationDays", "DurationDays BETWEEN 1 AND 365 ");
            });


        }
    }
}
