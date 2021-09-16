using Microsoft.EntityFrameworkCore;

namespace GithubConnection.Database
{
    public class DatabaseContext : DbContext
    {
         public DbSet<SingleCommitDbModel> CommitsTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer("Server=mssql6.webio.pl,2401; Database=codetown1_devtest01; User Id=codetown1_devtest_user01; Password=OQA2V75#/8^[rK_:41LX&m60'Toc;trustServerCertificate=true");
        }
        //
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SingleCommitDbModel>().HasKey(n => n.Id);
            builder.Entity<SingleCommitDbModel>().HasIndex(u => u.Sha).IsUnique();
        }
    }
}