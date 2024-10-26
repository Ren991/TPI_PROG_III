using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        

        

   
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id); 
                entity.Property(p => p.Id)
                      .ValueGeneratedOnAdd(); 
            });

            // Configuración de User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                      .ValueGeneratedOnAdd();
            });

            // Configuración de Cart
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id)
                      .ValueGeneratedOnAdd();

                // Relación uno a muchos entre User y Cart
                entity.HasOne(c => c.User)
                      .WithMany(u => u.Carts)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);  // Borrar los carritos si se borra el usuario

                // Relación uno a muchos entre Cart y SaleLine
                entity.HasMany(c => c.CartLineList)
                      .WithOne(sl => sl.Cart)
                      .HasForeignKey(sl => sl.CartId)
                      .OnDelete(DeleteBehavior.Cascade);  // Borrar las líneas de venta si se borra el carrito
            });

            // Configuración de SaleLine
            modelBuilder.Entity<CartLine>(entity =>
            {
                entity.HasKey(sl => sl.Id);
                entity.Property(sl => sl.Id)
                      .ValueGeneratedOnAdd();

                // Relación uno a uno entre SaleLine y Product
                entity.HasOne(sl => sl.Product)
                      .WithMany()  // Un producto puede aparecer en varias líneas de venta
                      .HasForeignKey(sl => sl.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);  // No permitir borrar un producto si está en una línea de venta

                // Relación muchos a uno entre SaleLine y Cart
                entity.HasOne(sl => sl.Cart)
                      .WithMany(c => c.CartLineList)
                      .HasForeignKey(sl => sl.CartId);
            });


        }


    }
}