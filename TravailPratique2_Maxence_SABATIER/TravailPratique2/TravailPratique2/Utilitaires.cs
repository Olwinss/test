using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailPratique2
{
    public class Utilitaires
    {

        private CMessagesUI messagesUI = new CMessagesUI();


        #region Méthodes de saisie
        public int SaisieEntier(string texte, bool saisieVideAutorisee, string valeurDefaut = "")
        {

            string id_str = Interaction.InputBox(texte, $"Saisie de données", valeurDefaut);

            if (id_str == "" && saisieVideAutorisee)
                return -1;

            try
            {
                int id = int.Parse(id_str);
                return id;
            }
            catch (Exception)
            {
                messagesUI.MessageErreur("La valeur saisie doit être un entier, ou laissée vide si le champ le permet.");
                throw new InvalidDataException();
            }

        }

        public string SaisieChaine(string texte, string valeurDefaut = "")
        {
            string saisie = Interaction.InputBox(texte, $"Saisie de données", valeurDefaut);

            if (!ChaineCorrecte(saisie))
            {
                messagesUI.MessageErreur("La donnée saisie ne doit pas être vide, ni dépasser 255 caractères.");
                throw new InvalidDataException();
            }
            else
                return saisie;
        }

        public DateTime? SaisieDate(string texte, string valeurDefaut = "")
        {
            string saisie = Interaction.InputBox(texte, $"Saisie de données", valeurDefaut);

            try
            {
                if (saisie == "")
                    return null;

                DateTime date = DateTime.ParseExact(saisie, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                return date;
            }
            catch (Exception)
            {
                messagesUI.MessageErreur("La date saisie est invalide.");
                throw new InvalidDataException();
            }

        }

        public decimal SaisieDecimale(string texte, string valeurDefaut = "")
        {
            string saisie = Interaction.InputBox(texte, $"Saisie de données", valeurDefaut);
            saisie = saisie.Replace('.', ',');

            try
            {
                decimal valeur = decimal.Parse(saisie, CultureInfo.CurrentCulture);
                return valeur;
            }
            catch (Exception)
            {
                messagesUI.MessageErreur("La donnée saisie est invalide. Il doit s'agir d'un nombre.");
                throw new Exception();
            }
        }
        #endregion


        #region Méthodes de vérification
        public bool VerificationID(int id, TableBD table)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    switch (table)
                    {
                        case TableBD.Employes:
                            Employe emp = (Employe)GetLigneDonnees(id, table);
                            return emp?.employee_id == id;

                        case TableBD.Departements:
                            Departement dept = (Departement)GetLigneDonnees(id, table);
                            return dept?.department_id == id;

                        case TableBD.Projets:
                            Projet proj = (Projet)GetLigneDonnees(id, table);
                            return proj?.project_id == id;

                        default:
                            throw new NotImplementedException();
                    }

                }

            }
            catch (Exception)
            {
                messagesUI.MessageErreur("Une erreur inattendue est survenue lors de la vérification de l'ID.", "Erreur de vérification");
                throw;
            }
        }

        public int GetNbProjetsEmploye(int idEmploye)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    return context.EmployeProjets.Where(e=> e.employee_id == idEmploye).Count();
                }
            }
            catch (Exception)
            {
                messagesUI.MessageErreur("Une erreur inattendue est survenue lors de la vérification de l'ID.", "Erreur de vérification");
                throw;
            }

        }

        public bool VerificationID_EmployesProjets(int idEmploye, int idProjet)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    EmployeProjet emproj = GetLigneDonnees_EmployesProjet(idEmploye, idProjet);
                    return emproj?.employee_id == idEmploye && emproj?.project_id == idProjet;
                }
            }
            catch (Exception)
            {
                messagesUI.MessageErreur("Une erreur inattendue est survenue lors de la vérification de l'ID.", "Erreur de vérification");
                throw;
            }

        }

        public bool VerificationListeManipulation(int id, TableBD table, List<object> liste)
        {
            try
            {
                foreach (object element in liste)
                {
                    switch (table)
                    {
                        case TableBD.Employes:
                            if ((element as Employe) != null)
                            {
                                if ((element as Employe).employee_id == id)
                                {

                                    return true;
                                }
                            }
                            break;
                        case TableBD.Departements:
                            if ((element as Departement) != null)
                            {
                                if ((element as Departement).department_id == id)
                                {

                                    return true;
                                }
                            }
                            break;
                        case TableBD.Projets:
                            if ((element as Projet) != null)
                            {
                                if ((element as Projet).project_id == id)
                                {
                                    return true;
                                }
                            }
                            break;
                        case TableBD.EmployesProjets:
                            throw new NotImplementedException();


                    }

                }

                return false;

            }
            catch (Exception e)
            {
                messagesUI.MessageErreur("Une erreur inattendue est survenue lors de la vérification de l'ID.", "Erreur de vérification");
                throw e;
            }
        }

        public bool VerificationListeManipulation_EmployesProjets(int idEmploye, int idProjet, List<object> liste)
        {
            try
            {
                foreach (object element in liste)
                {
                    var employeProjet = (element as EmployeProjet);

                    if (employeProjet.employee_id == idEmploye && employeProjet.project_id == idProjet)
                    {
                        return true;
                    }

                }

                return false;

            }
            catch (Exception e)
            {
                messagesUI.MessageErreur("Une erreur inattendue est survenue lors de la vérification de l'ID.", "Erreur de vérification");
                throw e;
            }
        }

        private bool ChaineCorrecte(string chaine)
        {
            if (chaine == "")
                return false;
            else if (chaine.Length > 255)
                return false;
            else return true;
        }


        #endregion


        #region Récupération d'une ligne de données
        public object GetLigneDonnees(int id, TableBD table)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    switch (table)
                    {
                        case TableBD.Employes:
                            return context.Employes.Find(id);

                        case TableBD.Departements:
                            return context.Departements.Find(id);

                        case TableBD.Projets:
                            return context.Projets.Find(id);

                        default:
                            throw new NotImplementedException();
                    }

                }

            }
            catch (Exception)
            {
                messagesUI.MessageErreur("Une erreur inattendue est survenue lors de la récupération des données", "Erreur de récupération");
                throw;
            }
        }

        public EmployeProjet GetLigneDonnees_EmployesProjet(int idEmploye, int idProjet)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    return context.EmployeProjets.Where(x => x.employee_id == idEmploye && x.project_id == idProjet).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                messagesUI.MessageErreur("Une erreur inattendue est survenue lors de la récupération des données", "Erreur de récupération");
                throw;
            }
        }

        #endregion

    }

}
