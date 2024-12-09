using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*!
 * \file  "Departments.cs"
 *
 * \brief 
 *      Fichier contenant le modèle de la table Departments du schéma public
 *      Ce modèle est utilisé pour la création de la table Departments dans la base de données
 *      Il y a également des attributs permettant de gérer les relations de la table Departement avec la table Employees
 *        
 * \author Olivier LABROSSE
 * \date 4/11/2024
 * \last update 21/11/2024
 *
 */

namespace TP2___SGBD_Olivier
{
    [Table("departments", Schema = "public")]
    public class Departments : CTable
    {
        [Browsable(false)]
        public static int m_nFirstFreeID = 1;
        public Departments() {
            
        }
        public Departments(string ndepartment_name)
        {
            department_id = m_nFirstFreeID++;
            department_name = ndepartment_name;
        }

        public Departments(string ndepartment_name, int id)
        {
            department_id = id;
            department_name = ndepartment_name;
        }



        [Key]
        public int department_id { get; set; }
        [MaxLength(255)]
        public string department_name { get; set; }

        [Browsable(false)]
        public virtual ICollection<Employees>? employees { get; set; } = new List<Employees>();
    }
}
