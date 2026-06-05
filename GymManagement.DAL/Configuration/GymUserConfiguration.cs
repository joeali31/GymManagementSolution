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
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(u => u.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);


            builder.Property(u => u.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(u => u.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);

            builder.Property(u => u.Gender)
                .HasConversion<string>();

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Phone).IsUnique();

            builder.OwnsOne(u => u.Address, ad =>
            {
                ad.Property(a => a.Street)
                    .HasColumnName("Street")
                    .HasColumnType("varchar")
                    .HasMaxLength(30);

                ad.Property(a => a.City)
                    .HasColumnName("City")
                    .HasColumnType("varchar")
                    .HasMaxLength(30);

                ad.Property(a => a.BuildingNumber)
                    .HasColumnName("BuildingNumber");
            });

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CKEmail", "Email Like '_%@_%._%'");
                tb.HasCheckConstraint("CKPhone", "Phone Like '010%' Or Phone Like '011%' Or Phone Like '012%' Or Phone Like '015%'");
            });
        }
    }
}
