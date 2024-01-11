using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Data.Models.Entities;

namespace Backend.Data.Models
{
    public class StoreContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<NoteCategories> NoteCategories { get; set; }

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
      
        }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         // Many-to-many relationship between Note and Category
    modelBuilder.Entity<NoteCategories>()
        .HasKey(nc => new { nc.NoteId, nc.CategoryId });

    modelBuilder.Entity<NoteCategories>()
        .HasOne(nc => nc.Note)
        .WithMany(n => n.NoteCategories)
        .HasForeignKey(nc => nc.NoteId)
        .OnDelete(DeleteBehavior.Cascade); // <-- Add this line for cascading delete

    modelBuilder.Entity<NoteCategories>()
        .HasOne(nc => nc.Category)
        .WithMany(c => c.NoteCategories)
        .HasForeignKey(nc => nc.CategoryId)
        .OnDelete(DeleteBehavior.Cascade); // <-- Add this line for cascading delete
    }
    }
}