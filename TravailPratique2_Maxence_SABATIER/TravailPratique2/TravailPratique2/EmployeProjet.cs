using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailPratique2
{
    [Table("employee_projects", Schema = "public")]
    [PrimaryKey("employee_id", "project_id")]
    public class EmployeProjet
    {
        public int employee_id { get; set; }

        public int project_id { get; set; }

        public string? role { get; set; }

        [ForeignKey("employee_id")]
        public virtual Employe? employe { get; set; }

        [ForeignKey("project_id")]
        public virtual Projet? projet { get; set; }


        public bool ValeursIdentiques(EmployeProjet employeProjet)
        {
            return
                employee_id == employeProjet.employee_id &&
                project_id == employeProjet.project_id &&
                role == employeProjet.role;
        }

        public EmployeProjetAffichage GetFormatAffichage()
        {
            return new EmployeProjetAffichage
            {
                ID_employe = employee_id,
                ID_projet = project_id,
                role = role
            };
        }

    }
}
