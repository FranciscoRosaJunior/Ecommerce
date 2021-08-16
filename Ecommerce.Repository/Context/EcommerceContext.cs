using Ecommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repository.Context
{
    public class EcommerceContext : DbContext
    {

        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
        {

        }

        public DbSet<Fruta> Frutas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fruta>().Property(p => p.Nome).HasMaxLength(80);
            modelBuilder.Entity<Fruta>().Property(p => p.Valor).HasPrecision(10, 2);

            modelBuilder.Entity<Usuario>().Property(p => p.NomeCompleto).HasMaxLength(80);
            modelBuilder.Entity<Usuario>().Property(p => p.Email).HasMaxLength(80);

            modelBuilder.Entity<Fruta>()
                .HasData(
                    new Fruta { Id = 1, Nome = "Banana", Descricao = "Banana Prata", Foto = "banana.png", Quantidade = 15, Estoque = 35, Valor = 4.99M },
                    new Fruta { Id = 2, Nome = "Mamão", Descricao = "Mamão Papaia", Foto = "mamao.png", Quantidade = 21, Estoque = 40, Valor = 5.75M },
                    new Fruta { Id = 3, Nome = "Maça", Descricao = "Maça Fuji", Foto = "maça.png", Quantidade = 35, Estoque = 51, Valor = 3.25M },
                    new Fruta { Id = 4, Nome = "Laranja", Descricao = "Laranja Bahia", Foto = "laranja.png", Quantidade = 42, Estoque = 57, Valor = 6.46M });
        }
    }

}
