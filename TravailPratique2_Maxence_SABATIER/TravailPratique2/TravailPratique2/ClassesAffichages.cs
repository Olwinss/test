using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailPratique2
{
    public class EmployeAffichage
    {
        public int ID { get; set; }
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string Date_embauche { get; set; }
        public decimal Salaire { get; set; }
        public int? ID_departement { get; set; }

    }

    public class DepartementAffichage
    {
        public int ID { get; set; }
        public string Nom { get; set; }
    }

    public class ProjetAffichage
    {
        public int ID { get; set; }
        public string Nom { get; set; }
        public string Date_debut { get; set; }
        public string Date_fin { get; set; }

    }

    public class EmployeProjetAffichage
    {
        public int ID_employe { get; set; }
        public int ID_projet { get; set; }
        public string? role { get; set; }
    }

    public class DepartementEmployeAffichage
    {
        public int ID_departement { get; set; }
        public string Nom_departement { get; set; }
        public int ID_employe { get; set; }
        public string Prenom_employe { get; set; }
        public string Nom_employe { get; set; }
        public string Date_embauche { get; set; }
        public decimal Salaire { get; set; }
    }

    public class ProjetsEmployesAffichage
    {
        public int ID_Projet { get; set; }
        public string Nom_Projet { get; set; }
        public int ID_employe { get; set; }
        public string Prenom_employe { get; set; }
        public string Nom_employe { get; set; }
        public string Role { get; set; }
        public string Date_embauche { get; set; }
        public decimal Salaire { get; set; }
    }



}
