using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Project_API_NH.Models;

namespace Project_API_NH.Models;

public partial class SalesNhContext : DbContext
{
    public SalesNhContext()
    {
    }

    public SalesNhContext(DbContextOptions<SalesNhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Typeproduct> Typeproducts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost,1433; Database=salesNH;User Id=sa;Password=Password.1;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orders__3213E83F3B1863C3");

            entity.ToTable("orders");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ngaytao)
                .HasColumnType("date")
                .HasColumnName("ngaytao");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Totalmoney)
                .HasColumnType("decimal(12, 4)")
                .HasColumnName("totalmoney");
            entity.Property(e => e.UsersId).HasColumnName("usersId");

            entity.HasOne(d => d.Users).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UsersId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__usersId__47DBAE45");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orderite__3213E83F82E7AE23");

            entity.ToTable("orderitems");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Totalmoney)
                .HasColumnType("decimal(12, 4)")
                .HasColumnName("totalmoney");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__orderitem__order__5070F446");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orderitem__produ__4E88ABD4");

            entity.HasOne(d => d.User).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orderitem__userI__4F7CD00D");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__products__3213E83F0A298912");

            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brandname)
                .HasMaxLength(255)
                .HasColumnName("brandname");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(12, 4)")
                .HasColumnName("price");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.SupplierId).HasColumnName("supplierId");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .HasColumnName("thumbnail");
            entity.Property(e => e.TypeproductId).HasColumnName("typeproductId");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__products__suppli__44FF419A");

            entity.HasOne(d => d.Typeproduct).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeproductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__products__typepr__440B1D61");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supplier__3213E83F17222459");

            entity.ToTable("suppliers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.Thumnail)
                .HasMaxLength(255)
                .HasColumnName("thumnail");
        });

        modelBuilder.Entity<Typeproduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__typeprod__3213E83FF04835B0");

            entity.ToTable("typeproducts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.Thumnail)
                .HasMaxLength(255)
                .HasColumnName("thumnail");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FCC1DC806");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164D9517EF1").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.JobTitle)
                .HasMaxLength(255)
                .HasColumnName("jobTitle");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleTitle)
                .HasMaxLength(255)
                .HasColumnName("roleTitle");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.Tel)
                .HasMaxLength(255)
                .HasColumnName("tel");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .HasColumnName("thumbnail");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
