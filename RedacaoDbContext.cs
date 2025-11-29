using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using RedacaoAPI.Models;

namespace RedacaoAPI;

public partial class RedacaoDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Essay> Essays { get; set; }
    public DbSet<Plan> Plans { get; set; }

    public RedacaoDbContext() { }

    public RedacaoDbContext(DbContextOptions<RedacaoDbContext> options)
        : base(options)
    {
    }

    // Dados sensíveis armazenados em variáveis de ambiente
    private string user = Environment.GetEnvironmentVariable("MYSQL_USER");
    private string password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql($"server=localhost;user id={user};password={password};database=db_redacao", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.3.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
