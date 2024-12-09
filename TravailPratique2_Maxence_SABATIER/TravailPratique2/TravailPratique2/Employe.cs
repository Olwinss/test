using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravailPratique2
{
    //Classe Enmploye (entité) qui représente la table "employees" de la BD.

    [Table("employees", Schema = "public")]
    public class Employe
    {
        [Key]
        public int employee_id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public DateTime hire_date { get; set; }

        public int? department_id { get; set; }

        public decimal salary { get; set; }

        [ForeignKey("department_id")]
        public virtual Departement? departement { get; set; }

        public virtual ICollection<EmployeProjet>? employesProjet { get; set; } = new List<EmployeProjet>();


        public bool ValeursIdentiques(Employe employe)
        {
            return 
                employee_id == employe.employee_id &&
                first_name == employe.first_name && 
                last_name == employe.last_name &&
                hire_date.ToUniversalTime() == employe.hire_date.ToUniversalTime() &&
                department_id == employe.department_id &&
                salary == employe.salary;
        }

        public EmployeAffichage GetFormatAffichage()
        {
            return new EmployeAffichage
            {
                ID = employee_id,
                Prenom = first_name,
                Nom = last_name,
                Date_embauche = hire_date.ToShortDateString(),
                Salaire = salary,
                ID_departement = department_id
            };
        }
    }
}
