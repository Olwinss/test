using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravailPratique2
{
    public  class CMessagesUI
    {

        #region Messages d'erreur
        public void ErreurManipulationBloquee()
        {
            MessageBox.Show(
                "Une manipulation a déjà été effectuée sur cette ligne. " +
                "Vous devez annuler ou sauvegarder les changements avant de réaliser cette action.", "Manipulation bloquée", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void MessageErreur(string texte, string titre = "Erreur de saisie")
        {
            MessageBox.Show(texte, titre, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region Messages d'information

        public void MessageInformation(string texte, string titre)
        {
            MessageBox.Show(texte, titre, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Messages d'alerte

        public void AlerteAucuneModification()
        {
            MessageBox.Show("Aucune modification n'a été apportée", "Annulation de l'opération", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public DialogResult AlerteModificationsNonSauvegardees(int nbModifications)
        {
            return MessageBox.Show($"{nbModifications} modifications n'ont pas été sauvegardées dans la base de données." +
                    $" Voulez-vous sauvegarder ?", "Modifications non sauvegardées", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }

        #endregion

        #region Messages de confirmations de manipulation

        public DialogResult MessageConfirmation_Employe(Employe employe, string message, string titre)
        {
            string idDepartement_str = employe.department_id == null ? "AUCUN" : employe.department_id.ToString();

            return MessageBox.Show(
                $"Données : \n" +
                $"ID : {employe.employee_id}\n" +
                $"Prénom : {employe.first_name}\n" +
                $"Nom : {employe.last_name}\n" +
                $"Date embauche : {employe.hire_date.ToShortDateString()}\n" +
                $"Salaire : {employe.salary}\n" +
                $"ID Département : {idDepartement_str}" +
                $"\n\n{message}",
                $"Résumé : {titre}", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        public DialogResult MessageConfirmation_Departement(Departement departement, string message, string titre)
        {
            return MessageBox.Show(
                    $"Données : \n" +
                    $"ID : {departement.department_id}\n" +
                    $"Nom département : {departement.department_name}\n" +
                    $"\n\n{message}",
                    $"Résumé : {titre}", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        public DialogResult MessageConfirmation_Projet(Projet projet, string message, string titre)
        {
            string dateFin_str = projet.end_date == null ? "AUCUNE" : projet.end_date.Value.ToShortDateString();
            
            return MessageBox.Show(
                $"Données : \n" +
                $"ID : {projet.project_id}\n" +
                $"Nom projet : {projet.project_name}\n" +
                $"Date de début : {projet.start_date.ToShortDateString()}\n" +
                $"Date de fin : {dateFin_str}\n" +
                $"\n\n{message}",
                $"Résumé : {titre}", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        public DialogResult MessageConfirmation_EmployeProjet(EmployeProjet employeProjet, string message, string titre)
        {
            return MessageBox.Show(
                    $"Données : \n" +
                    $"ID Employé : {employeProjet.employee_id}\n" +
                    $"ID Projet : {employeProjet.project_id}\n" +
                    $"Rôle de l'employé : {employeProjet.role}\n" +
                    $"\n\n {message}",
                    $"Résumé : {titre}", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        #endregion
    }


}
