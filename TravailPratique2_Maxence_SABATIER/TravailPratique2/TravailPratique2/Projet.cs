using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailPratique2
{
    [Table("projects", Schema = "public")]
    public class Projet
    {
        [Key]
        public int project_id { get; set; }
        public string project_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime? end_date { get; set; }

        public virtual ICollection<EmployeProjet>? employesProjet { get; set; } = new List<EmployeProjet>();

        public bool ValeursIdentiques(Projet projet)
        {
            return
                project_id == projet.project_id &&
                project_name == projet.project_name &&
                start_date.ToUniversalTime() == projet.start_date.ToUniversalTime() &&
                end_date?.ToUniversalTime() == projet.end_date?.ToUniversalTime();
        }

        public ProjetAffichage GetFormatAffichage()
        {
            return new ProjetAffichage
            {
                ID = project_id,
                Nom = project_name,
                Date_debut = start_date.ToShortDateString(),
                Date_fin = end_date?.ToShortDateString()
            };
        }
    }
}
