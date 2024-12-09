using Microsoft.EntityFrameworkCore;

namespace TravailPratique2
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Departement> Departements { get; set; }
        public DbSet<Projet> Projets { get; set; }
        public DbSet<EmployeProjet> EmployeProjets { get; set; }

        public static string chaineConnexion =
            "Host=localhost;" +
            "Database=postgres;" +
            "Username=postgres;" +
            $"Password={Environment.GetEnvironmentVariable("BD_PostgreSQL_Postgres")}";

        public AppDbContext() : base(OptionConnexionPostgresSQL())
        { }

        protected static DbContextOptions OptionConnexionPostgresSQL()
        {
            return new DbContextOptionsBuilder()
                .UseNpgsql(chaineConnexion)
                .Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employe>()
                .Property(e => e.employee_id)
                .ValueGeneratedOnAdd(); // Spécifie que la valeur est générée par la base de données

            modelBuilder.Entity<Departement>()
                .Property(e => e.department_id)
                .ValueGeneratedOnAdd(); // Spécifie que la valeur est générée par la base de données

            modelBuilder.Entity<Projet>()
                .Property(e => e.project_id)
                .ValueGeneratedOnAdd(); // Spécifie que la valeur est générée par la base de données
        }

    }


}
