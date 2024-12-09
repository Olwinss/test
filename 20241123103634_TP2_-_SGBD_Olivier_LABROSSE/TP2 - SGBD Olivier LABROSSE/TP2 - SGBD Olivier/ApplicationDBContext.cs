using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Text;

/*!
 * \file  "ApplicationDBContext.cs"
 *
 * \brief 
 *      Classe permettant de gérer la connexion à la base de donnée PostgreSQL
 *        
 * \author Olivier LABROSSE
 * \date 4/11/2024
 * \last update 23/11/2024
 *
 */


namespace TP2___SGBD_Olivier
{
    internal class ApplicationDBContext : DbContext
    {
        public DbSet<Departments> departments { get; set; }
        public DbSet<Employee_projects> employee_projects { get; set; }
        public DbSet<Employees> employees { get; set; }
        public DbSet<Projects> projects { get; set; }

        static public DbConnection Connexion { get; set; }

        public static string connectionString = $"Host=localhost;Port=5432;Database=postgres;Username=postgres;Password={Environment.GetEnvironmentVariable("BD_PostgreSQL_postgres")}";


        public ApplicationDBContext() : base(OptionsConnexionPostgreSQL())
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); // format pour les dates sans timezone

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                Connexion = new Npgsql.NpgsqlConnection(connectionString);
                optionsBuilder.UseNpgsql(Connexion);
            }
        }
        protected static DbContextOptions OptionsConnexionPostgreSQL()
        {
            return new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseNpgsql(connectionString)
                .EnableSensitiveDataLogging()
                .LogTo(LogQuery)
                .Options;
        }
        //Méthode qui reçoit les logs du contexte de connexion et traite ces logs
        public static void LogQuery(string logMessage)
        {
            if (logMessage.Contains("Executing DbCommand")) // Vérifie si c'est une requête SQL
            {
                Console.WriteLine(logMessage); // Affiche la requête SQL (optionnel)
            }
        }

        public class TextBoxWriter : TextWriter
        {
            private TextBox _output;

            public TextBoxWriter(TextBox output)
            {
                _output = output;
            }

            public override void Write(char value)
            {
                _output.Invoke((MethodInvoker)(() => _output.AppendText(value.ToString())));
            }

            public override void Write(string value)
            {
                _output.Invoke((MethodInvoker)(() => _output.AppendText(value)));
            }

            public override Encoding Encoding => Encoding.UTF8;
            
        }
    
    }
}


