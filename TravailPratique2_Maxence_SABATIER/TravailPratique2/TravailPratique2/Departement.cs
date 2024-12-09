using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravailPratique2
{
    [Table("departments", Schema = "public")]
    public class Departement
    {
        [Key]
        public int department_id { get; set; }

        public string department_name { get; set; }

        public virtual ICollection<Employe>? employes { get; set; } = new List<Employe>();

        public bool ValeursIdentiques(Departement departement)
        {
            return
                department_id == departement.department_id &&
                department_name == departement.department_name;
        }

        public DepartementAffichage GetFormatAffichage()
        {
            return new DepartementAffichage
            {
                ID = department_id,
                Nom = department_name
            };
        }

    }
}
