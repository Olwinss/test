using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

/*!
 * \file  "Employee_projects.cs"
 *
 * \brief 
 *      Fichier contenant le modèle de la table employee_projects du schéma public
 *      Ce modèle est utilisé pour la création de la table employee_projects dans la base de données
 *      Il y a également des attributs permettant de gérer les relations de la table employee_projects avec les tables Employees et Projects
 *        
 * \author Olivier LABROSSE
 * \date 4/11/2024
 * \last update 20/11/2024
 *
 */

namespace TP2___SGBD_Olivier
{
    [Table("employee_projects", Schema = "public")]
    [PrimaryKey(nameof(employee_id),nameof(project_id))]
    public class Employee_projects : CTable
    {
        public Employee_projects() { }
        public Employee_projects(int nemployee_id, int nproject_id, string nrole)
        {
            employee_id = nemployee_id;
            project_id = nproject_id;
            role = nrole;
        }
        public int employee_id { get; set; }
        public int project_id { get; set; }
        public string? role { get; set; }

        [ForeignKey("project_id")]
        [Browsable(false)]
        public virtual Projects project { get; set; }

        public string NomProjet => project?.project_name ?? "Erreur de récupération";

        [ForeignKey("employee_id")]
        [Browsable(false)]
        public virtual Employees employee { get; set; }

        public string NomEmployee => $"{employee.first_name} {employee.last_name}";
    }
}
