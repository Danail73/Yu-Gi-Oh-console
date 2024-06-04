using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;

namespace DataLayer
{
    public class YuGiOhDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(@"Server=DANI\SQLEXPRESS;Database=YuGiOhCardDatabase;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public YuGiOhDbContext() { }
        public YuGiOhDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }

        public override int SaveChanges()
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    Database.ExecuteSqlRaw("SET IDENTITY_INSERT Decks ON");

                    var result = base.SaveChanges();

                    Database.ExecuteSqlRaw("SET IDENTITY_INSERT Decks OFF");

                    transaction.Commit();
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
