using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


/*!
 * \file  "Projects.cs"
 *
 * \brief 
 *      Fichier contenant le modèle de la table projects du schéma public
 *      Ce modèle est utilisé pour la création de la table employees dans la base de données
 *      Il y a également des attributs permettant de gérer les relations de la table projects avec la table Employee_projects
 *        
 * \author Olivier LABROSSE
 * \date 4/11/2024
 * \last update 10/11/2024
 *
 */


namespace TP2___SGBD_Olivier
{
    [Table("projects", Schema = "public")]
    public class Projects : CTable
    {
        [Browsable(false)]
        public static int m_nFirstFreeID = 1;
        public Projects()
        {
        }
        public Projects(string nproject_name, DateTime nstart_date, DateTime nend_date)
        {
            project_id = m_nFirstFreeID++;
            project_name = nproject_name;
            start_date = nstart_date;
            end_date = nend_date;
        }
        [Key]
        public int project_id { get; set; }

        [MaxLength(255)]
        public string project_name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }

        [Browsable(false)]
        public virtual ICollection<Employee_projects>? employee_projects { get; set; } = new List<Employee_projects>();
    }
}
