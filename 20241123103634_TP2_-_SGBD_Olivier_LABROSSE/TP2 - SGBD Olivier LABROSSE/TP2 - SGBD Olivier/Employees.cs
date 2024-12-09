using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*!
 * \file  "Employees.cs"
 *
 * \brief 
 *      Fichier contenant le modèle de la table employees du schéma public
 *      Ce modèle est utilisé pour la création de la table employees dans la base de données
 *      Il y a également des attributs permettant de gérer les relations de la table employees avec les tables Departments et Employee_projects
 *        
 * \author Olivier LABROSSE
 * \date 4/11/2024
 * \last update 23/11/2024
 *
 */


namespace TP2___SGBD_Olivier
{
    [Table("employees", Schema = "public")]
    public class Employees : CTable
    {
        [Browsable(false)]
        public static int m_nFirstFreeID = 1;

        public Employees()
        { }
        public Employees(string first_name, string last_name, DateTime hire_date, int? department_id, float? salary, Departments? departement, ICollection<Employee_projects>? employee_projects, int? id = null)
        {
            if (id != null)
                employee_id = id.Value;
            else
                employee_id = m_nFirstFreeID++;
            this.first_name = first_name;
            this.last_name = last_name;
            this.hire_date = hire_date;
            this.department_id = department_id;
            this.salary = salary;
            this.departement = departement;
            this.employee_projects = employee_projects;
        }

        [Key]
        public int employee_id { get; set; }
        [MaxLength(255)]
        public string first_name { get; set; }
        [MaxLength(255)]
        public string last_name { get; set; }
        public DateTime hire_date { get; set; }
        public int? department_id { get; set; }
        [MaxLength(10)]
        public float? salary { get; set; }

        [ForeignKey("department_id")]
        [Browsable(false)]
        public virtual Departments? departement { get; set; }

        public string NomDepartement => departement?.department_name ?? "Aucun département";

        [Browsable(false)]
        virtual public ICollection<Employee_projects>? employee_projects { get; set; } = new List<Employee_projects>();

    }
}
