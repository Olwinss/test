using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;

namespace TravailPratique2
{

    /*
        ---------------------------------------- ENT�TE DE PROJET ----------------------------------------
        Application : Application SGBD

        D�veloppeur : Maxence SABATIER

        Version : 1.0
        Derni�re modification : 24/11


        Description g�n�rale du projet :
        Cette application repr�sente un SGBD, capable de manipuler des donn�es dans des tables et d'afficher les relations de ces tables


        Organisation du projet :
        - ApplicationForm.cs - Classe principale, g�re l'application
        - Employe, EmployeProjet, Departement, Projet.cs - Tables de la BDD
        - Utilitaires.cs - Contient des m�thodes utilitaires
        - CMessagesUI.cs - Contient les messages utilis�s par l'application
        - ClassesAffichage.cs - Contient toutes les classes d'affichage
        --------------------------------------------------------------------------------------------------
 */





    //Enum repr�sentant les 4 tables pr�sentes
    public enum TableBD
    {
        Employes,
        Departements,
        Projets,
        EmployesProjets
    }

    public enum RelationBD
    {
        DepartementsEmployes,
        ProjetsEmployes
    }

    public partial class ApplicationForm : Form
    {

        #region Variables de classe

        //Liste des tables existantes
        static private List<string> nomsTables = new List<string> { "Employ�s", "D�partements", "Projets", "Employ�s-Projets" };

        //Liste des relations existantes
        static private List<string> nomsRelations = new List<string> { "D�pt. -> Employ�s", "Projets -> Employ�s" };

        //Nombre de lignes � afficher par pages
        private const int NB_ELEM_AFFICHAGE = 10000;

        //Liste du prochain ID d'auto incr�mentation pour chaque table concern�e
        private int[] prochaineValeurID = new int[3];

        //Nombre d'�l�ment � partir duquel commence l'affichage
        static private int nbElementsMinimum = 0;

        //Nombre d'�l�ments affich�s au total
        static private int nbElementsTotal = 0;

        //Nombre d'�l�ments maximums de la table
        static private int nbElementsMaximumTable = 0;

        //ID de la derni�re table consult�e
        static private int idDerniereTableChoisie = 0;

        //ID de la dernir�e relation consult�e
        static private int idDerniereRelationChoisie = 0;

        //Nombre de modifications/ajouts/suppressions en attente
        static private int nbModifications;

        //Permet de savoir si l'�v�nement "SelectedIndexChanged" a �t� lanc� apr�s un changement manuel de l'index
        private bool modificationManuelleIndex_Table = false;
        private bool modificationManuelleIndex_Relation = false;

        //Instance de la classe utilitaire
        private Utilitaires utilitaires = new Utilitaires();

        //Instance de la classe CMessagesUI
        private CMessagesUI messagesUI = new CMessagesUI();

        //Liste des manipulations effectu�es et non sauvegard�es
        private List<object> listeAjouts = new List<object>();
        private List<object> listeModifications = new List<object>();
        private List<object> listeSuppressions = new List<object>();

        #endregion


        #region Initialisation de l'application

        public ApplicationForm()
        {
            InitializeComponent();

            //Met � jour le titre de la fen�tre
            Text = "Application - SGBD";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Initialise la listbox des tables 
            listeTables.Items.Clear();
            listeRelations.Items.Clear();

            //Initialise la listbox des relations
            listeTables.DataSource = nomsTables;
            listeRelations.DataSource = nomsRelations;

            //Initialise les relations
            modificationManuelleIndex_Relation = true;
            listeRelations.SelectedIndex = -1;

            //Initialise l'aide pour les �l�ments du GUI
            GestionnaireAide.SetToolTip(btnPremiersElems, "Afficher les 10 000 premi�res lignes");
            GestionnaireAide.SetToolTip(btnMoinsElem, "Afficher les 10 000 lignes pr�c�dentes");
            GestionnaireAide.SetToolTip(btnMoinsUnElem, "Reculer d'une ligne");
            GestionnaireAide.SetToolTip(btnPlusUnElem, "Avancer d'une ligne");
            GestionnaireAide.SetToolTip(btnPlusElem, "Afficher les 10 000 lignes suivantes");
            GestionnaireAide.SetToolTip(btnDerniersElems, "Afficher les 10 000 derni�res lignes");

            GestionnaireAide.SetToolTip(btnAjouterDonnee, "Ajouter une nouvelle ligne de donn�es � la table actuelle");
            GestionnaireAide.SetToolTip(btnSupprimerDonnee, "Supprimer une ligne de donn�es de la table actuelle");
            GestionnaireAide.SetToolTip(btnModifierDonnee, "Modifier une ligne de donn�es de la table actuelle");

            GestionnaireAide.SetToolTip(btnAnnulerSauvegarde, "Annuler les manipulations en attente de sauvegarde");
            GestionnaireAide.SetToolTip(btnSauvegarder, "Sauvegarder les manipulations en attente");

            GestionnaireAide.SetToolTip(listeTables, "Choisir la table � afficher");
            GestionnaireAide.SetToolTip(labelNbDonnees, "S�rie de lignes actuellement affich�es");

            GrilleDonnees.EditMode = DataGridViewEditMode.EditProgrammatically;

            //Initialise les IDs automatiques
            using (AppDbContext context = new AppDbContext())
            {
                prochaineValeurID[0] = context.Employes.Count() + 1;
                prochaineValeurID[1] = context.Departements.Count() + 1;
                prochaineValeurID[2] = context.Projets.Count() + 1;
            }
        }
        #endregion


        #region S�lection d'une table/relation

        //Comportement a effectuer � l'affichage d'une table
        private void listeTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Si l'ID a �t� change manuellement, on emp�che l'�v�nement
            if (modificationManuelleIndex_Table)
            {
                modificationManuelleIndex_Table = false;
                return;
            }

            //Si des modifications sont en cours, on sauvegarde avant le changement de table
            if (nbModifications > 0)
            {
                DialogResult resultat = messagesUI.AlerteModificationsNonSauvegardees(nbModifications);

                if (resultat == DialogResult.Yes)
                {
                    SauvegarderModifications();
                }
                else if (resultat == DialogResult.No)
                {
                    AnnulerModifications();
                }
                else if (resultat == DialogResult.Cancel) //Annule le changement de table
                {
                    modificationManuelleIndex_Table = true;
                    listeTables.SelectedIndex = idDerniereTableChoisie;
                }

            }

            modificationManuelleIndex_Relation = true;
            listeRelations.SelectedIndex = -1;

            //R�cup�re l'ID de la table selectionn�e
            int idElement = listeTables.SelectedIndex;

            //Si il y a eu un changement de table, on r�initialise le nombre minimal d'�l�ments
            if (idElement != idDerniereTableChoisie)
                nbElementsMinimum = 0;

            //Affiche la table correspondante
            RafrachirAffichage_Table((TableBD)idElement);

            //Enregistre l'ID choisi que le dernier selectionn�.
            idDerniereTableChoisie = idElement;


        }

        private void listeRelations_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Si l'ID a �t� change manuellement, on emp�che l'�v�nement
            if (modificationManuelleIndex_Relation)
            {
                modificationManuelleIndex_Relation = false;
                return;
            }

            //Si des modifications sont en cours, on sauvegarde avant le changement de table
            if (nbModifications > 0)
            {
                DialogResult resultat = messagesUI.AlerteModificationsNonSauvegardees(nbModifications);

                if (resultat == DialogResult.Yes)
                {
                    SauvegarderModifications();
                }
                else if (resultat == DialogResult.No)
                {
                    AnnulerModifications();
                }
                else if (resultat == DialogResult.Cancel) //Annule le changement de table
                {
                    return;
                }

            }

            modificationManuelleIndex_Table = true;
            listeTables.SelectedIndex = -1;


            int relationChoisie = listeRelations.SelectedIndex;

            if (relationChoisie != idDerniereRelationChoisie)
                nbElementsMinimum = 0;

            RafrachirAffichage_Relation((RelationBD)relationChoisie);

            idDerniereRelationChoisie = relationChoisie;





        }
        #endregion


        #region �v�nements de navigation

        //M�thode permettant d'afficher les 10 000 premiers �l�ments
        private void btnPremiersElems_Click(object sender, EventArgs e)
        {
            nbElementsMinimum = 0;

            if (listeTables.SelectedIndex != -1)
            {
                RafrachirAffichage_Table((TableBD)idDerniereTableChoisie);
            }
            else
            {
                RafrachirAffichage_Relation((RelationBD)idDerniereRelationChoisie);
            }

        }


        //M�thode permettant d'afficher les 10 000 �l�ments pr�c�dents
        private void btnMoinsElem_Click(object sender, EventArgs e)
        {
            nbElementsMinimum -= NB_ELEM_AFFICHAGE;

            if (nbElementsMinimum < 0)
                nbElementsMinimum = 0;

            if (listeTables.SelectedIndex != -1)
            {
                RafrachirAffichage_Table((TableBD)idDerniereTableChoisie);
            }
            else
            {
                RafrachirAffichage_Relation((RelationBD)idDerniereRelationChoisie);
            }
        }

        //M�thode permettant de reculer d'un �l�m�ment dans l'affichage
        private void btnMoinsUnElem_Click(object sender, EventArgs e)
        {
            nbElementsMinimum -= 1;

            if (listeTables.SelectedIndex != -1)
            {
                RafrachirAffichage_Table((TableBD)idDerniereTableChoisie);
            }
            else
            {
                RafrachirAffichage_Relation((RelationBD)idDerniereRelationChoisie);
            }

            //S�lectionne et affiche la nouvelle premi�re ligne
            GrilleDonnees.Rows[0].Selected = true;
        }

        //M�thode permettant d'avancer d'un �l�m�ment dans l'affichage
        private void btnPlusUnElem_Click(object sender, EventArgs e)
        {
            nbElementsMinimum += 1;

            if (listeTables.SelectedIndex != -1)
            {
                RafrachirAffichage_Table((TableBD)idDerniereTableChoisie);
            }
            else
            {
                RafrachirAffichage_Relation((RelationBD)idDerniereRelationChoisie);
            }

            //S�lectionne et affiche la nouvelle derni�re ligne
            GrilleDonnees.FirstDisplayedScrollingRowIndex = GrilleDonnees.RowCount - 1;
            GrilleDonnees.Rows[GrilleDonnees.RowCount - 1].Selected = true;
        }

        //M�thode permettant d'afficher les 10 000 �l�ments suivants
        private void btnPlusElem_Click(object sender, EventArgs e)
        {
            nbElementsMinimum += NB_ELEM_AFFICHAGE;

            if (listeTables.SelectedIndex != -1)
            {
                RafrachirAffichage_Table((TableBD)idDerniereTableChoisie);
            }
            else
            {
                RafrachirAffichage_Relation((RelationBD)idDerniereRelationChoisie);
            }
        }

        //M�thode permettant d'afficher les 10 000 derniers �l�ments
        private void btnDerniersElems_Click(object sender, EventArgs e)
        {
            nbElementsMinimum = (nbElementsMaximumTable + listeAjouts.Count - listeSuppressions.Count) - NB_ELEM_AFFICHAGE;

            if (listeTables.SelectedIndex != -1)
            {
                RafrachirAffichage_Table((TableBD)idDerniereTableChoisie);
            }
            else
            {
                RafrachirAffichage_Relation((RelationBD)idDerniereRelationChoisie);
            }
        }

        #endregion


        #region �v�nements de manipulation de donn�es

        //M�thode charg�e de l'ajout d'une nouvelle ligne de donn�es
        private void btnAjouterDonnee_Click(object sender, EventArgs e)
        {
            //R�cup�ration de la table actuelle
            TableBD selectionTable = (TableBD)listeTables.SelectedIndex;

            try
            {
                //On effectue un comportement d�pendamment de la table.
                switch (selectionTable)
                {
                    case TableBD.Employes:

                        AjouterEmploye();
                        break;

                    case TableBD.Departements:

                        AjouterDepartement();
                        break;

                    case TableBD.Projets:

                        AjouterProjet();
                        break;

                    case TableBD.EmployesProjets:

                        ajouterEmployesProjet();
                        break;

                }
            }
            catch (Exception) { return; }

            RafrachirAffichage_Table(selectionTable);

        }

        private void btnSupprimerDonnee_Click(object sender, EventArgs e)
        {
            //R�cup�ration de la table actuelle
            TableBD selectionTable = (TableBD)listeTables.SelectedIndex;

            string messageConfirmation = "Confirmez-vous la suppression de ces donn�es ?";
            string titreConfirmation = "Donn�es supprim�es";

            try
            {
                //On effectue un comportement d�pendamment de la table.
                switch (selectionTable)
                {
                    case TableBD.EmployesProjets:

                        bool idExistant;

                        int idEmploye = utilitaires.SaisieEntier("Veuillez saisir l'ID Employ� de la donn�e � supprimer", false);
                        int idProjet = utilitaires.SaisieEntier("Veuillez saisir l'ID Projet de la donn�e � supprimer", false);

                        idExistant = utilitaires.VerificationID_EmployesProjets(idEmploye, idProjet);

                        if (!idExistant)
                        {
                            messagesUI.MessageErreur("Cette paire d'IDs n'existe pas dans la base de donn�es.");
                            return;
                        }

                        //On v�rifie que cet ID n'a pas d�j� �t� modifi�
                        if (utilitaires.VerificationListeManipulation_EmployesProjets(idEmploye, idProjet, listeModifications))
                        {
                            messagesUI.ErreurManipulationBloquee();
                            throw new Exception();
                        }

                        //On v�rifie que cet ID n'a pas d�j� �t� supprim�
                        if (utilitaires.VerificationListeManipulation_EmployesProjets(idEmploye, idProjet, listeSuppressions))
                        {
                            messagesUI.ErreurManipulationBloquee();
                            throw new Exception();
                        }

                        EmployeProjet employeProjet = utilitaires.GetLigneDonnees_EmployesProjet(idEmploye, idProjet);

                        //On affiche un r�sum� des donn�es saisies
                        DialogResult resultat = messagesUI.MessageConfirmation_EmployeProjet(employeProjet, messageConfirmation, titreConfirmation);

                        //Sauvegarde des donn�es
                        if (resultat == DialogResult.Yes)
                        {
                            listeSuppressions.Add(employeProjet);
                        }
                        else
                            return;

                        break;

                    default:

                        //Saisie de l'ID a supprimer
                        int id = utilitaires.SaisieEntier("Veuillez saisir l'ID de la donn�e � supprimer", false);

                        //Si l'ID existe dans la base de donn�es
                        if (utilitaires.VerificationID(id, selectionTable))
                        {
                            //On v�rifie que cet ID n'a pas d�j� �t� modifi�
                            if (utilitaires.VerificationListeManipulation(id, selectionTable, listeModifications))
                            {
                                messagesUI.ErreurManipulationBloquee();
                                throw new Exception();
                            }

                            //On v�rifie que cet ID n'a pas d�j� �t� supprim�
                            if (utilitaires.VerificationListeManipulation(id, selectionTable, listeSuppressions))
                            {
                                messagesUI.ErreurManipulationBloquee();
                                throw new Exception();
                            }

                            object element = utilitaires.GetLigneDonnees(id, selectionTable);

                            switch (selectionTable)
                            {
                                case TableBD.Employes:
                                    messagesUI.MessageConfirmation_Employe((Employe)element, messageConfirmation, titreConfirmation);
                                    break;
                                case TableBD.Departements:
                                    messagesUI.MessageConfirmation_Departement((Departement)element, messageConfirmation, titreConfirmation);
                                    break;
                                case TableBD.Projets:
                                    messagesUI.MessageConfirmation_Projet((Projet)element, messageConfirmation, titreConfirmation);
                                    break;
                            }

                            listeSuppressions.Add(element);

                        }
                        else
                        {
                            messagesUI.MessageErreur("L'ID saisi n'existe pas dans la base de donn�es.");
                            return;
                        }
                        break;

                }

            }
            catch (Exception) { return; }
            

            RafrachirAffichage_Table(selectionTable);

        }


        private void btnModifierDonnee_Click(object sender, EventArgs e)
        {
            //R�cup�ration de la table actuelle
            TableBD selectionTable = (TableBD)listeTables.SelectedIndex;

            try
            {
                switch (selectionTable)
                {
                    case TableBD.Employes:

                        ModifierEmploye();
                        break;

                    case TableBD.Departements:

                        ModifierDepartement();
                        break;

                    case TableBD.Projets:

                        ModifierProjet();
                        break;
                    
                    case TableBD.EmployesProjets:

                        ModifierEmployeProjet();
                        break;

                }
            }
            catch (Exception) { return; }


            RafrachirAffichage_Table(selectionTable);

        }


        private void btnSauvegarder_Click(object sender, EventArgs e)
        {
            
            SauvegarderModifications();
          
        }

        private void btnAnnulerSauvegarde_Click(object sender, EventArgs e)
        {
            AnnulerModifications();
        }


        #endregion


        #region �v�nement de fermeture de l'application

        //G�re la fermeture de l'application
        void ApplicationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && nbModifications > 0)
            {
                DialogResult resultat = messagesUI.AlerteModificationsNonSauvegardees(nbModifications);

                if (resultat == DialogResult.Yes)
                {
                    SauvegarderModifications();
                }
                else if (resultat == DialogResult.No)
                {
                    AnnulerModifications();
                }
                else if (resultat == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }

            }

        }

        #endregion


        #region M�thodes d'affichage des donn�es

        private void RafrachirAffichage_Table(TableBD table)
        {
                        //Charge la table correspondante
            switch (table)
            {
                case TableBD.Employes:
                    chargerEmployes();
                    break;
                case TableBD.Departements:
                    chargerDepartements();
                    break;
                case TableBD.Projets:
                    chargerProjets();
                    break;
                case TableBD.EmployesProjets:
                    chargerEmployesProjets();
                    break;
                default: break;
            }

            //Modifie les contr�les si n�cessaires
            ControleNavigation();

            ControleSauvegarde();
        }

        private void RafrachirAffichage_Relation(RelationBD relation)
        {
            switch (relation)
            {
                case RelationBD.DepartementsEmployes:

                    AfficherRelation_DepartementsEmployes();
                    break;
                case RelationBD.ProjetsEmployes:

                    AfficherRelation_ProjetsEmployes();
                    break;
            }

            ControleNavigation();
        }

        private void chargerEmployes()
        {
            //R�cup�re les  employ�s ajout�s sur la page actuelle
            List<EmployeAffichage> employesAjoutes = new List <EmployeAffichage>();
            foreach (var obj in listeAjouts)
            {
                int id = (obj as Employe).employee_id;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (id >= nbElementsMinimum)
                    {
                        employesAjoutes.Add((obj as Employe).GetFormatAffichage());
                    }
                }
                else
                {
                    if (id >= nbElementsMinimum && id < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        employesAjoutes.Add((obj as Employe).GetFormatAffichage());
                    }
                }

            }

            //Fait de m�me avec les employ�s supprim�s
            List<int> IndexEmployesSupprimes = new List<int>();
            for(int i = 0; i < listeSuppressions.Count; i++) 
            {
                int id = (listeSuppressions[i] as Employe).employee_id;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (id >= nbElementsMinimum)
                    {
                        nbElementsMinimum--;
                        IndexEmployesSupprimes.Add(id);
                    }
                }
                else
                {
                    if (id >= nbElementsMinimum && id < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        IndexEmployesSupprimes.Add(id);
                    }
                }

            }

            //Fait de m�me avec les employ�s modifi�s
            List<EmployeAffichage> employesModifies = new List<EmployeAffichage>();
            foreach (var obj in listeModifications)
            {
                int id = (obj as Employe).employee_id;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (id >= nbElementsMinimum)
                    {
                        employesModifies.Add((obj as Employe).GetFormatAffichage());
                    }
                }
                else
                {
                    if (id >= nbElementsMinimum && id < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        employesModifies.Add((obj as Employe).GetFormatAffichage());
                    }
                }

            }

            //Utilise une connexion temporaire � la BD
            using (var context = new AppDbContext())
            {
                //R�cup�re 10 000 lignes � partir du nombre voulu
                List<EmployeAffichage> listeEmployeAffichage = context.Employes
                    .OrderBy(elt => elt.employee_id)
                    .Skip(nbElementsMinimum)
                    .Take(NB_ELEM_AFFICHAGE - employesAjoutes.Count)
                    .Select(e => new EmployeAffichage
                    {
                        ID = e.employee_id,
                        Prenom = e.first_name,
                        Nom = e.last_name,
                        Date_embauche = e.hire_date.ToShortDateString(),
                        Salaire = e.salary,
                        ID_departement = e.department_id
                    }
                    ).ToList();

                //Modifie la liste des employ�s selon les modifications effectu�es
                listeEmployeAffichage.AddRange(employesAjoutes);

                foreach (var indexEmp in IndexEmployesSupprimes)
                {
                    listeEmployeAffichage.RemoveAll(e => e.ID == indexEmp);
                }

                foreach (var empModifie in employesModifies)
                {
                    for (int i = 0; i < listeEmployeAffichage.Count; i++)
                    {
                        if (listeEmployeAffichage[i].ID == empModifie.ID)
                        {
                            listeEmployeAffichage[i] = empModifie;
                        }
                    }
                }

                //R�cup�re le nombre d'�l�ments affich�s � l'�cran (10 000 ou moins)
                nbElementsTotal = listeEmployeAffichage.Count();

                //R�cup�re le nombre de lignes total de la table
                nbElementsMaximumTable = context.Employes.Count();

                //Met � jour les donn�es � afficher
                GrilleDonnees.DataSource = listeEmployeAffichage.OrderBy(e => e.ID).ToList();

            }
        }

        private void chargerDepartements()
        {

            //R�cup�re les d�partements ajout�s sur la page actuelle
            List<DepartementAffichage> departementsAjoutes = new List<DepartementAffichage>();
            foreach (var obj in listeAjouts)
            {
                int id = (obj as Departement).department_id;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (id >= nbElementsMinimum)
                    {
                        departementsAjoutes.Add((obj as Departement).GetFormatAffichage());
                    }
                }
                else
                {
                    if (id >= nbElementsMinimum && id < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        departementsAjoutes.Add((obj as Departement).GetFormatAffichage());
                    }
                }

            }

            //Fait de m�me avec les d�partements supprim�s
            List<int> IndexDepartementsSupprimes = new List<int>();
            for (int i = 0; i < listeSuppressions.Count; i++)
            {
                int id = (listeSuppressions[i] as Departement).department_id;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (id >= nbElementsMinimum)
                    {
                        nbElementsMinimum--;
                        IndexDepartementsSupprimes.Add(id);
                    }
                }
                else
                {
                    if (id >= nbElementsMinimum && id < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        IndexDepartementsSupprimes.Add(id);
                    }
                }

            }

            //Fait de m�me avec les d�partements modifi�s
            List<DepartementAffichage> departementsModifies = new List<DepartementAffichage>();
            foreach (var obj in listeModifications)
            {
                int id = (obj as Departement).department_id;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (id >= nbElementsMinimum)
                    {
                        departementsModifies.Add((obj as Departement).GetFormatAffichage());
                    }
                }
                else
                {
                    if (id >= nbElementsMinimum && id < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        departementsModifies.Add((obj as Departement).GetFormatAffichage());
                    }
                }

            }

            //Utilise une connexion temporaire � la BD
            using (var context = new AppDbContext())
            {
                //R�cup�re 10 000 lignes � partir du nombre voulu
                List<DepartementAffichage> listeDepartementsAffichage = context.Departements
                    .OrderBy(elt => elt.department_id)
                    .Skip(nbElementsMinimum)
                    .Take(NB_ELEM_AFFICHAGE - departementsAjoutes.Count)
                    .Select(e => new DepartementAffichage
                    {
                        ID = e.department_id,
                        Nom = e.department_name
                    }
                    ).ToList();

                //Modifie la liste des employ�s selon les modifications effectu�es
                listeDepartementsAffichage.AddRange(departementsAjoutes);

                foreach (var indexDep in IndexDepartementsSupprimes)
                {
                    listeDepartementsAffichage.RemoveAll(e => e.ID == indexDep);
                }

                foreach (var depModifie in departementsModifies)
                {
                    for (int i = 0; i < listeDepartementsAffichage.Count; i++)
                    {
                        if (listeDepartementsAffichage[i].ID == depModifie.ID)
                        {
                            listeDepartementsAffichage[i] = depModifie;
                        }
                    }
                }

                //R�cup�re le nombre d'�l�ments affich�s � l'�cran (10 000 ou moins)
                nbElementsTotal = listeDepartementsAffichage.Count();

                //R�cup�re le nombre de lignes total de la table
                nbElementsMaximumTable = context.Departements.Count();

                //Met � jour les donn�es � afficher
                GrilleDonnees.DataSource = listeDepartementsAffichage.OrderBy(e => e.ID).ToList();

            }
        }



        private void chargerProjets()
        {
            //R�cup�re les projets ajout�s sur la page actuelle
            List<ProjetAffichage> projetsAjoutes = new List<ProjetAffichage>();
            foreach (var obj in listeAjouts)
            {
                int id = (obj as Projet).project_id;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (id >= nbElementsMinimum)
                    {
                        projetsAjoutes.Add((obj as Projet).GetFormatAffichage());
                    }
                }
                else
                {
                    if (id >= nbElementsMinimum && id < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        projetsAjoutes.Add((obj as Projet).GetFormatAffichage());
                    }
                }

            }

            //Fait de m�me avec les projets supprim�s
            List<int> IndexProjetsSupprimes = new List<int>();
            for (int i = 0; i < listeSuppressions.Count; i++)
            {
                int id = (listeSuppressions[i] as Projet).project_id;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (id >= nbElementsMinimum)
                    {
                        nbElementsMinimum--;
                        IndexProjetsSupprimes.Add(id);
                    }
                }
                else
                {
                    if (id >= nbElementsMinimum && id < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        IndexProjetsSupprimes.Add(id);
                    }
                }

            }

            //Fait de m�me avec les projets modifi�s
            List<ProjetAffichage> projetsModifies = new List<ProjetAffichage>();
            foreach (var obj in listeModifications)
            {
                int id = (obj as Projet).project_id;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (id >= nbElementsMinimum)
                    {
                        projetsModifies.Add((obj as Projet).GetFormatAffichage());
                    }
                }
                else
                {
                    if (id >= nbElementsMinimum && id < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        projetsModifies.Add((obj as Projet).GetFormatAffichage());
                    }
                }

            }

            //Utilise une connexion temporaire � la BD
            using (var context = new AppDbContext())
            {
                //R�cup�re 10 000 lignes � partir du nombre voulu
                List<ProjetAffichage> listeProjetsAffichage = context.Projets
                    .OrderBy(elt => elt.project_id)
                    .Skip(nbElementsMinimum)
                    .Take(NB_ELEM_AFFICHAGE - projetsAjoutes.Count)
                    .Select(e => new ProjetAffichage
                    {
                        ID = e.project_id,
                        Nom = e.project_name,
                        Date_debut = e.start_date.ToShortDateString(),
                        Date_fin = e.end_date.Value.ToShortDateString(),
                    }
                    ).ToList();


                //Modifie la liste des employ�s selon les modifications effectu�es
                listeProjetsAffichage.AddRange(projetsAjoutes);

                foreach (var indexEmp in IndexProjetsSupprimes)
                {
                    listeProjetsAffichage.RemoveAll(e => e.ID == indexEmp);
                }

                foreach (var depModifie in projetsModifies)
                {
                    for (int i = 0; i < listeProjetsAffichage.Count; i++)
                    {
                        if (listeProjetsAffichage[i].ID == depModifie.ID)
                        {
                            listeProjetsAffichage[i] = depModifie;
                        }
                    }
                }


                //R�cup�re le nombre d'�l�ments affich�s � l'�cran (10 000 ou moins)
                nbElementsTotal = listeProjetsAffichage.Count();

                //R�cup�re le nombre de lignes total de la table
                nbElementsMaximumTable = context.Projets.Count();

                //Met � jour les donn�es � afficher
                GrilleDonnees.DataSource = listeProjetsAffichage.OrderBy(e => e.ID).ToList();

            }
        }


        private void chargerEmployesProjets()
        {

            //R�cup�re les employes-projets supprim�s sur la page actuelle
            List<Tuple<int, int>> IndexEmployesProjetsSupprimes = new List<Tuple<int, int>>();
            for (int i = 0; i < listeSuppressions.Count; i++)
            {
                int idEmploye = (listeSuppressions[i] as EmployeProjet).employee_id;
                int idProjet = (listeSuppressions[i] as EmployeProjet).project_id;

                // Calculez la plage des project_id correspondant � la page actuelle
                int projetIdMin = nbElementsMinimum;
                int projetIdMax = nbElementsMinimum + NB_ELEM_AFFICHAGE;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (idEmploye >= nbElementsMinimum)
                    {
                        // V�rifiez si l'ID de projet se trouve dans la plage de la page actuelle
                        //C'est le cas si l'ID de projet entre entre les bornes de la page actuelle OU s'il ya moins de 10 000 lignes avec le m�me ID d'employ�
                        if (idProjet >= projetIdMin && idProjet <= projetIdMax || utilitaires.GetNbProjetsEmploye(idEmploye) < NB_ELEM_AFFICHAGE)
                        {
                            nbElementsMinimum--;
                            IndexEmployesProjetsSupprimes.Add(new Tuple<int, int>(idEmploye, idProjet));
                        }

                    }
                }
                else
                {
                    if (idEmploye >= nbElementsMinimum && idEmploye < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        // V�rifiez si l'ID de projet se trouve dans la plage de la page actuelle
                        //C'est le cas si l'ID de projet entre entre les bornes de la page actuelle OU s'il ya moins de 10 000 lignes avec le m�me ID d'employ�
                        if (idProjet >= projetIdMin && idProjet <= projetIdMax || utilitaires.GetNbProjetsEmploye(idEmploye) < NB_ELEM_AFFICHAGE)
                        {
                            IndexEmployesProjetsSupprimes.Add(new Tuple<int, int>(idEmploye, idProjet));
                        }

                    }
                }

            }

            //R�cup�re les employes-projets ajout�s sur la page actuelle
            List<EmployeProjetAffichage> employesProjetsAjoutes = new List<EmployeProjetAffichage>();
            foreach (var obj in listeAjouts)
            {
                var employeProjet = (obj as EmployeProjet);

                int idEmploye = employeProjet.employee_id;
                int idProjet = employeProjet.project_id;

                // Calculez la plage des project_id correspondant � la page actuelle
                int projetIdMin = nbElementsMinimum;
                int projetIdMax = nbElementsMinimum + NB_ELEM_AFFICHAGE;

                if (nbElementsMinimum + NB_ELEM_AFFICHAGE == nbElementsMaximumTable + listeAjouts.Count())
                {
                    if (idEmploye >= nbElementsMinimum)
                    {
                        // V�rifiez si l'ID de projet se trouve dans la plage de la page actuelle
                        //C'est le cas si l'ID de projet entre entre les bornes de la page actuelle OU s'il ya moins de 10 000 lignes avec le m�me ID d'employ�
                        if (idProjet >= projetIdMin && idProjet <= projetIdMax || utilitaires.GetNbProjetsEmploye(idEmploye) < NB_ELEM_AFFICHAGE)
                        {
                            employesProjetsAjoutes.Add(employeProjet.GetFormatAffichage());
                        }
                    }
                }
                else
                {
                    if (idEmploye >= nbElementsMinimum && idEmploye < nbElementsMinimum + NB_ELEM_AFFICHAGE + 1)
                    {
                        // V�rifiez si l'ID de projet se trouve dans la plage de la page actuelle
                        //C'est le cas si l'ID de projet entre entre les bornes de la page actuelle OU s'il ya moins de 10 000 lignes avec le m�me ID d'employ�
                        if (idProjet >= projetIdMin && idProjet <= projetIdMax || utilitaires.GetNbProjetsEmploye(idEmploye) < NB_ELEM_AFFICHAGE)
                        {
                            employesProjetsAjoutes.Add(employeProjet.GetFormatAffichage());
                        }
                    }
                }

            }


            //Utilise une connexion temporaire � la BD
            using (var context = new AppDbContext())
            {
                //R�cup�re 10 000 lignes � partir du nombre voulu
                List<EmployeProjetAffichage> listeEmployeProjetAffichage = context.EmployeProjets
                    .OrderBy(elt => elt.employee_id)
                    .ThenBy(elt => elt.project_id)
                    .Skip(nbElementsMinimum)
                    .Take(NB_ELEM_AFFICHAGE - employesProjetsAjoutes.Count)
                    .Select(e => new EmployeProjetAffichage
                    {
                        ID_employe = e.employee_id,
                        ID_projet = e.project_id,
                        role = e.role
                    }
                    ).ToList();

                //Modifie la liste des employ�s selon les modifications effectu�es
                listeEmployeProjetAffichage.AddRange(employesProjetsAjoutes);

                foreach (var indexEmpProj in IndexEmployesProjetsSupprimes)
                {
                    listeEmployeProjetAffichage.RemoveAll(e => e.ID_employe == indexEmpProj.Item1 && e.ID_projet == indexEmpProj.Item2);
                }

                foreach (var elem  in listeModifications)
                {
                    var empProj = (EmployeProjet)elem;

                    for (int i = 0; i < listeEmployeProjetAffichage.Count; i++)
                    {
                        if (listeEmployeProjetAffichage[i].ID_employe == empProj.employee_id &&
                            listeEmployeProjetAffichage[i].ID_projet == empProj.project_id)
                        {
                            listeEmployeProjetAffichage[i] = empProj.GetFormatAffichage();
                        }
                    }

                }

                //R�cup�re le nombre d'�l�ments affich�s � l'�cran (10 000 ou moins)
                nbElementsTotal = listeEmployeProjetAffichage.Count();

                //R�cup�re le nombre d'�l�ments affich�s � l'�cran (10 000 ou moins)
                nbElementsMaximumTable = context.EmployeProjets.Count();

                //Met � jour les donn�es � afficher
                GrilleDonnees.DataSource = listeEmployeProjetAffichage.OrderBy(e => e.ID_employe).ThenBy(e => e.ID_projet).ToList();
            }
        }

        #endregion


        #region M�thodes d'ajout de donn�es

        private void AjouterEmploye()
        {

            int idDonnees = prochaineValeurID[idDerniereTableChoisie];

            //R�cup�re les autres champs n�cessaires
            string prenom = utilitaires.SaisieChaine("Saisissez un pr�nom");
            string nom = utilitaires.SaisieChaine("Saisissez un nom");

            DateTime dateEmbauche = utilitaires.SaisieDate($"Saisissez une date d'embauche (JJ/MM/AAAA) ou laissez vide pour prendre la date actuelle ({DateTime.UtcNow.ToShortDateString()})") ?? DateTime.UtcNow;

            decimal salaire = utilitaires.SaisieDecimale("Saisissez le montant du salaire.");

            //Saisie de l'ID de d�partement
            int idDepartement = utilitaires.SaisieEntier("Entrez un ID de d�partement ou laissez vide si aucune liaison aux d�partements n'est souhait�e.", true);
            bool cleEtrangerePresente = idDepartement != -1; //V�rifie si un ID a �t� saisi

            //Si un ID de d�partement a �t� saisi, on v�rifie qu'il existe
            if (cleEtrangerePresente)
            {
                if (!utilitaires.VerificationID(idDepartement, TableBD.Departements))
                {
                    messagesUI.MessageErreur("L'ID de d�partement saisi n'existe pas dans la base de donn�es.");
                    return;
                }
            }

            using (var context = new AppDbContext())
            {
                //Cr�ation du nouvel employ� a ajouter
                Employe employe = new Employe
                {
                    employee_id = idDonnees,
                    first_name = prenom,
                    last_name = nom,
                    hire_date = dateEmbauche.ToUniversalTime(),
                    salary = salaire,
                    department_id = cleEtrangerePresente ? idDepartement : null
                };

                //On affiche un r�sum� des donn�es saisies
                DialogResult resultat = messagesUI.MessageConfirmation_Employe(employe, "Confirmez-vous l'ajout de ces donn�es ?", "Donn�es ins�r�es");

                //Sauvegarde des donn�es
                if (resultat == DialogResult.Yes)
                {
                    listeAjouts.Add(employe);
                    
                    prochaineValeurID[idDerniereTableChoisie]++;

                }
                else
                    return;

            }
        }


        private void AjouterDepartement()
        {

            int idDepartement = prochaineValeurID[idDerniereTableChoisie];

            //R�cup�re le nom du d�partement
            string nomDepartement = utilitaires.SaisieChaine("Saisissez un nom de d�partement");

            using (var context = new AppDbContext())
            {
                //Cr�ation du nouvel employ� a ajouter
                Departement departement = departement = new Departement
                {
                    department_id = idDepartement,
                    department_name = nomDepartement
                };

                //On affiche un r�sum� des donn�es saisies
                DialogResult resultat = messagesUI.MessageConfirmation_Departement(departement, "Confirmez-vous l'ajout de ces donn�es ?", "Donn�es ins�r�es");

                //Sauvegarde des donn�es
                if (resultat == DialogResult.Yes)
                {
                    listeAjouts.Add(departement);

                    prochaineValeurID[idDerniereTableChoisie]++;

                }
                else
                    return;

            }
        }


        private void AjouterProjet()
        {

            int idProjet = prochaineValeurID[idDerniereTableChoisie];

            //R�cup�re le nom du projet
            string nomProjet = utilitaires.SaisieChaine("Saisissez un nom de projet");

            //Saisie des dates du projet
            DateTime dateDebut = utilitaires.SaisieDate($"Saisissez une date de d�but de projet (YYYY-MM-DD) ou laissez vide pour prendre la date actuelle ({DateTime.UtcNow.ToShortDateString()})") ?? DateTime.UtcNow;
            DateTime? dateFin = utilitaires.SaisieDate("Saisissez une date de fin de projet (YYYY-MM-DD) (facultatif)");

            if (dateFin != null)
            {
                if (dateDebut.Date > dateFin.Value.Date)
                {
                    messagesUI.MessageErreur("La date de fin ne peut pas �tre sup�rieure � la date de d�but.");
                    throw new ArgumentOutOfRangeException();
                }
            }


            using (var context = new AppDbContext())
            {
                //Cr�ation du nouveau projet a ajouter
                Projet projet = new Projet
                {
                    project_id = idProjet,
                    project_name = nomProjet,
                    start_date = dateDebut.ToUniversalTime(),
                    end_date = dateFin?.ToUniversalTime()
                };

                //On affiche un r�sum� des donn�es saisies
                DialogResult resultat = messagesUI.MessageConfirmation_Projet(projet, "Confirmez-vous l'ajout de ces donn�es ?", "Donn�es ins�r�es");

                //Sauvegarde des donn�es
                if (resultat == DialogResult.Yes)
                {
                    listeAjouts.Add(projet);

                    prochaineValeurID[idDerniereTableChoisie]++;

                }
                else
                    return;

            }
        }


        private void ajouterEmployesProjet()
        {

            bool idExistant = false;

            //Saisie de l'ID d'employ�
            int idEmploye = utilitaires.SaisieEntier("Entrez un ID d'employ� (Ne peut pas �tre vide)", false);

            //V�rifie son existence dans la table employ�s
            idExistant = utilitaires.VerificationID(idEmploye, TableBD.Employes);
            if (!idExistant)
            {
                messagesUI.MessageErreur("L'ID saisi n'existe pas dans la base de donn�es.");
                return;
            }

            //Saisie de l'ID du projet
            int idProjet = utilitaires.SaisieEntier("Entrez un ID de projet (Ne peut pas �tre vide)", false);

            //V�rifie son existence dans la table projets
            idExistant = utilitaires.VerificationID(idProjet, TableBD.Projets);
            if (!idExistant)
            {
                messagesUI.MessageErreur("L'ID saisi n'existe pas dans la base de donn�es.");
                return;
            }

            //V�rifie que la paire n'est pas d�j� pr�sente dans la table
            idExistant = utilitaires.VerificationID_EmployesProjets(idEmploye, idProjet);

            if (idExistant)
            {
                messagesUI.MessageErreur("Cette paire d'IDs existe d�j� dans cette table", "Erreur de duplication");
                return;
            }

            //Saisie du r�le
            string roleProjet = utilitaires.SaisieChaine("Veuillez saisir le r�le de l'employ�");


            using (var context = new AppDbContext())
            {
                //Cr�ation du nouvel employ� a ajouter
                EmployeProjet employeProjet;

                employeProjet = new EmployeProjet
                {
                    employee_id = idEmploye,
                    project_id = idProjet,
                    role = roleProjet,
                };

                //On affiche un r�sum� des donn�es saisies
                DialogResult resultat = messagesUI.MessageConfirmation_EmployeProjet(employeProjet, "Confirmez-vous l'ajout de ces donn�es ?", "Donn�es ins�r�es");

                //Sauvegarde des donn�es
                if (resultat == DialogResult.Yes)
                {
                    listeAjouts.Add(employeProjet);
                }
                else
                    return;

            }
        }

        #endregion


        #region M�thodes de modification de donn�es

        private void ModifierEmploye()
        {
            //Saisie de l'ID (cl� primaire)
            int idEmploye = utilitaires.SaisieEntier("Entrez l'ID de la ligne � modifier", false);

            bool idExistant = utilitaires.VerificationID(idEmploye, TableBD.Employes);

            if (!idExistant)
            {
                messagesUI.MessageErreur("L'ID saisi n'existe pas dans la base de donn�es");
                throw new Exception();
            }

            if (utilitaires.VerificationListeManipulation(idEmploye, (TableBD)idDerniereTableChoisie, listeSuppressions)
                || utilitaires.VerificationListeManipulation(idEmploye, (TableBD)idDerniereTableChoisie, listeModifications))
            {
                messagesUI.ErreurManipulationBloquee();
                throw new Exception();
            }

            Employe employe = (Employe)utilitaires.GetLigneDonnees(idEmploye, (TableBD)idDerniereTableChoisie);
            Employe originalemploye = employe;

            //R�cup�re les autres champs n�cessaires
            string prenom = utilitaires.SaisieChaine("Saisissez un pr�nom", employe.first_name);
            string nom = utilitaires.SaisieChaine("Saisissez un nom", employe.last_name);

            DateTime dateEmbauche = utilitaires.SaisieDate($"Saisissez une date d'embauche (JJ/MM/AAAA) ou laissez vide pour prendre la date actuelle ({DateTime.UtcNow.ToShortDateString()})", employe.hire_date.ToShortDateString()) ?? DateTime.UtcNow;

            decimal salaire = utilitaires.SaisieDecimale("Saisissez le montant du salaire.", employe.salary.ToString());

            //Saisie de l'ID de d�partement
            int idDepartement = utilitaires.SaisieEntier("Entrez un ID de d�partement ou laissez vide si aucune liaison aux d�partements n'est souhait�e.", true, employe.department_id.ToString() ?? "");
            bool cleEtrangerePresente = idDepartement != -1; //V�rifie si un ID a �t� saisi

            //Si un ID a �t� saisi, on v�rifie qu'il existe
            if (cleEtrangerePresente)
            {
                bool cleEtrangereValide = utilitaires.VerificationID(idDepartement, TableBD.Departements);

                if (!cleEtrangereValide)
                {
                    messagesUI.MessageErreur("L'ID de d�partement saisi n'existe pas dans la base de donn�es.");
                    throw new Exception();
                }
            }

            using (var context = new AppDbContext())
            {

                employe = new Employe
                {
                    employee_id = idEmploye,
                    first_name = prenom,
                    last_name = nom,
                    hire_date = dateEmbauche.ToUniversalTime(),
                    salary = salaire,
                    department_id = cleEtrangerePresente ? idDepartement : null
                };

                if (employe.ValeursIdentiques(originalemploye))
                {
                    messagesUI.AlerteAucuneModification();
                }
                else
                {
                    //On affiche un r�sum� des donn�es saisies
                    DialogResult resultat = messagesUI.MessageConfirmation_Employe(employe, "Confirmez-vous les nouvelles valeurs de ces donn�es ?", "Donn�es modifi�es");

                    //Sauvegarde des donn�es
                    if (resultat == DialogResult.Yes)
                    {
                        listeModifications.Add(employe);
                    }
                    else
                        return;
                }

            }
        }


        private void ModifierDepartement()
        {

            //Saisie de l'ID (cl� primaire)
            int idDepartement = utilitaires.SaisieEntier("Entrez l'ID du d�partement � modifier", false);

            bool idExistant = utilitaires.VerificationID(idDepartement, (TableBD)idDerniereTableChoisie);

            if (!idExistant)
            {
                messagesUI.MessageErreur("L'ID saisi n'existe pas dans la base de donn�es");
                throw new Exception();
            }

            if (utilitaires.VerificationListeManipulation(idDepartement, (TableBD)idDerniereTableChoisie, listeSuppressions)
                || utilitaires.VerificationListeManipulation(idDepartement, (TableBD)idDerniereTableChoisie, listeModifications))
            {
                messagesUI.ErreurManipulationBloquee();
                throw new Exception();
            }

            Departement departement = (Departement)utilitaires.GetLigneDonnees(idDepartement, (TableBD)idDerniereTableChoisie);
            Departement originalDepartement = departement;

            //R�cup�re le nom du d�partement
            string nomDepartement = utilitaires.SaisieChaine("Saisissez un nom de d�partement", departement.department_name);


            using (var context = new AppDbContext())
            {

                departement = new Departement
                {
                    department_id = idDepartement,
                    department_name = nomDepartement
                };

                if (departement.ValeursIdentiques(originalDepartement))
                {
                    messagesUI.AlerteAucuneModification();
                }
                else
                {
                    //On affiche un r�sum� des donn�es saisies
                    DialogResult resultat = messagesUI.MessageConfirmation_Departement(departement, "Confirmez-vous les nouvelles valeurs de ces donn�es ?", "Donn�es modifi�es");

                    //Sauvegarde des donn�es
                    if (resultat == DialogResult.Yes)
                    {
                        listeModifications.Add(departement);
                    }
                    else
                        return;
                }




            }
        }


        private void ModifierProjet()
        {
            //Saisie de l'ID (cl� primaire)
            int idProjet = utilitaires.SaisieEntier("Entrez l'ID du projet � modifier", false);

            bool idExistant = utilitaires.VerificationID(idProjet, TableBD.Projets);

            if (!idExistant)
            {
                messagesUI.MessageErreur("L'ID saisi n'existe pas dans la base de donn�es");
                throw new Exception();
            }

            if (utilitaires.VerificationListeManipulation(idProjet, (TableBD)idDerniereTableChoisie, listeSuppressions)
                || utilitaires.VerificationListeManipulation(idProjet, (TableBD)idDerniereTableChoisie, listeModifications))
            {
                messagesUI.ErreurManipulationBloquee();
                throw new Exception();
            }

            Projet projet = (Projet) utilitaires.GetLigneDonnees(idProjet, TableBD.Projets);
            Projet originalProjet = projet;

            //R�cup�re le nom du projet
            string nomProjet = utilitaires.SaisieChaine("Saisissez un nom de projet", projet.project_name);

            //Saisie des dates du projet
            DateTime dateDebut = utilitaires.SaisieDate($"Saisissez une date de d�but de projet (JJ/MM/AAAA) ou laissez vide pour prendre la date actuelle ({DateTime.UtcNow.ToShortDateString()})", projet.start_date.ToShortDateString()) ?? DateTime.UtcNow;
            DateTime? dateFin = utilitaires.SaisieDate("Saisissez une date de fin de projet (JJ/MM/AAAA) (facultatif)", projet.end_date?.ToShortDateString() ?? "");

            if (dateFin != null)
            {
                if (dateDebut.Date > dateFin.Value.Date)
                {
                    messagesUI.MessageErreur("La date de fin ne peut pas �tre sup�rieure � la date de d�but.");
                    throw new ArgumentOutOfRangeException();
                }
            }

            using (var context = new AppDbContext())
            {
                projet = new Projet
                {
                    project_id = idProjet,
                    project_name = nomProjet,
                    start_date = dateDebut.ToUniversalTime(),
                    end_date = dateFin?.ToUniversalTime()
                };

                if (projet.ValeursIdentiques(originalProjet))
                {
                    messagesUI.AlerteAucuneModification();
                }
                else
                {
                    //On affiche un r�sum� des donn�es saisies
                    DialogResult resultat = messagesUI.MessageConfirmation_Projet(projet, "Confirmez-vous les nouvelles valeurs de ces donn�es ?", "Donn�es modifi�es");

                    //Sauvegarde des donn�es
                    if (resultat == DialogResult.Yes)
                    {
                        listeModifications.Add(projet);
                    }
                    else
                        return;
                }
            }
        }


        private void ModifierEmployeProjet()
        {
            bool idExistant = false;

            int idEmploye = utilitaires.SaisieEntier("Veuillez saisir l'ID Employ� de la donn�e � modifier", false);
            int idProjet = utilitaires.SaisieEntier("Veuillez saisir l'ID Projet de la donn�e � modifier", false);

            idExistant = utilitaires.VerificationID_EmployesProjets(idEmploye, idProjet);

            if (!idExistant)
            {
                messagesUI.MessageErreur("Cette paire d'IDs n'existe pas dans la base de donn�es.");
                throw new Exception();
            }

            if (utilitaires.VerificationListeManipulation_EmployesProjets(idEmploye,idProjet, listeSuppressions)
                || utilitaires.VerificationListeManipulation_EmployesProjets(idEmploye, idProjet, listeModifications))
            {
                messagesUI.ErreurManipulationBloquee();
                throw new Exception();
            }

            EmployeProjet employeProjet = utilitaires.GetLigneDonnees_EmployesProjet(idEmploye,idProjet);
            EmployeProjet originalEmployeProjet = employeProjet;


            //Saisie du r�le
            string roleProjet = utilitaires.SaisieChaine("Veuillez saisir le r�le de l'employ�", employeProjet.role ?? "");


            using (var context = new AppDbContext())
            {

                employeProjet = new EmployeProjet
                {
                    employee_id = idEmploye,
                    project_id = idProjet,
                    role = roleProjet,
                };

                if (employeProjet.ValeursIdentiques(originalEmployeProjet))
                {
                    messagesUI.AlerteAucuneModification();
                }
                else
                {

                    //On affiche un r�sum� des donn�es saisies
                    DialogResult resultat = messagesUI.MessageConfirmation_EmployeProjet(employeProjet, "Confirmez-vous les nouvelles valeurs de ces donn�es ?", "Donn�es modifi�es");

                    //Sauvegarde des donn�es
                    if (resultat == DialogResult.Yes)
                    {
                        listeModifications.Add(employeProjet);
                    }
                    else
                        return;
                }
            }
        }

        #endregion


        #region M�thodes d'affichage des relations

        public void AfficherRelation_DepartementsEmployes()
        {
            //Utilise une connexion temporaire � la BD
            using (var context = new AppDbContext())
            {
                //R�cup�re 10 000 lignes � partir du nombre voulu
                var departementsEmployes = context.Departements
                    .Include(d => d.employes) // Charger les employ�s li�s
                    .Where(d => d.employes.Any()) // Exclure les d�partements sans employ�s
                    .OrderBy(d => d.department_id) // Trier par ID de d�partement
                    .SelectMany(d => d.employes
                        .OrderBy(e => e.employee_id) // Trier les employ�s par ID
                        .Select(e => new DepartementEmployeAffichage
                        {
                            ID_departement = d.department_id,
                            Nom_departement = d.department_name,
                            ID_employe = e.employee_id,
                            Prenom_employe = e.first_name,
                            Nom_employe = e.last_name,
                            Date_embauche = e.hire_date.ToShortDateString(),
                            Salaire = e.salary
                        })
                    )
                    .Skip(nbElementsMinimum)
                    .Take(NB_ELEM_AFFICHAGE)
                    .ToList();

                //R�cup�re le nombre d'�l�ments affich�s � l'�cran (10 000 ou moins)
                nbElementsTotal = departementsEmployes.Count();


                //R�cup�re le nombre de lignes total de la table
                nbElementsMaximumTable = context.Departements.Join(context.Employes,
                    d => d.department_id, // Cl� �trang�re vers le d�partement
                    e => e.department_id, // Cl� �trang�re dans l'employ�
                    (d, e) => new { d, e }) // Cr�e une nouvelle paire contenant d�partement et employ�
                    .Count(); // Compte le nombre total de jointures
            


                //Met � jour les donn�es � afficher
                GrilleDonnees.DataSource = departementsEmployes.OrderBy(x => x.ID_departement).ThenBy(x => x.ID_employe).ToList();

                GrilleDonnees.Columns["ID_departement"].DisplayIndex = 0;
                GrilleDonnees.Columns["Nom_departement"].DisplayIndex = 1;
                GrilleDonnees.Columns["ID_employe"].DisplayIndex = 2;
                GrilleDonnees.Columns["Prenom_employe"].DisplayIndex = 3;
                GrilleDonnees.Columns["Nom_employe"].DisplayIndex = 4;
                GrilleDonnees.Columns["Date_embauche"].DisplayIndex = 5;
                GrilleDonnees.Columns["Salaire"].DisplayIndex = 6;


            }
        }

        public void AfficherRelation_ProjetsEmployes()
        {
            //Utilise une connexion temporaire � la BD
            using (var context = new AppDbContext())
            {
                //R�cup�re 10 000 lignes � partir du nombre voulu
                var projetsEmployes = context.Projets
                    .Join(context.EmployeProjets,
                        p => p.project_id, // cl� �trang�re vers la table EmployeProjet
                        ep => ep.project_id, // cl� �trang�re vers la table Projet
                        (p, ep) => new { p, ep }) // Jointure entre Projet et EmployeProjet
                    .Join(context.Employes,
                        pe => pe.ep.employee_id, // cl� �trang�re vers la table Employe
                        e => e.employee_id, // cl� primaire de la table Employe
                        (pe, e) => new ProjetsEmployesAffichage // Cr�ation de l'objet de sortie
                        {
                            ID_Projet = pe.p.project_id,
                            Nom_Projet = pe.p.project_name,
                            ID_employe = e.employee_id,
                            Prenom_employe = e.first_name,
                            Nom_employe = e.last_name,
                            Role = pe.ep.role,
                            Date_embauche = e.hire_date.ToShortDateString(),
                            Salaire = e.salary
                        })
                    .OrderBy(pe => pe.ID_Projet)  // Tri par ID de projet
                    .ThenBy(pe => pe.ID_employe) // Tri secondaire par ID d'employ�
                    .Skip(nbElementsMinimum)
                    .Take(NB_ELEM_AFFICHAGE)
                    .ToList();

                //R�cup�re le nombre d'�l�ments affich�s � l'�cran (10 000 ou moins)
                nbElementsTotal = projetsEmployes.Count();


                //R�cup�re le nombre de lignes total de la table
                nbElementsMaximumTable = context.Departements.Join(context.Employes,
                    d => d.department_id, // Cl� �trang�re vers le d�partement
                    e => e.department_id, // Cl� �trang�re dans l'employ�
                    (d, e) => new { d, e }) // Cr�e une nouvelle paire contenant d�partement et employ�
                    .Count(); // Compte le nombre total de jointures



                //Met � jour les donn�es � afficher
                GrilleDonnees.DataSource = projetsEmployes;//.OrderBy(x => x.ID_departement).ThenBy(x => x.ID_employe).ToList();

                GrilleDonnees.Columns["ID_Projet"].DisplayIndex = 0;
                GrilleDonnees.Columns["Nom_Projet"].DisplayIndex = 1;
                GrilleDonnees.Columns["ID_employe"].DisplayIndex = 2;
                GrilleDonnees.Columns["Prenom_employe"].DisplayIndex = 3;
                GrilleDonnees.Columns["Nom_employe"].DisplayIndex = 4;
                GrilleDonnees.Columns["Role"].DisplayIndex = 5;
                GrilleDonnees.Columns["Date_embauche"].DisplayIndex = 6;
                GrilleDonnees.Columns["Salaire"].DisplayIndex = 7;


            }
        }




        #endregion


        #region M�thodes de contr�le
        //M�thode charg�e d'indiquer la s�rie d'�l�ment affich�s ainsi que de contr�ler l'activation des boutons
        private void ControleNavigation()
        {
            //Indique la s�rie de donn�es actuellement affich�e

            int limiteBasse = nbElementsMinimum + 1;
            int limiteHaute = nbElementsMinimum + nbElementsTotal;

            bool maximumAtteint = nbElementsMinimum + nbElementsTotal >= (nbElementsMaximumTable + listeAjouts.Count - listeSuppressions.Count);

            //Si on a atteint la limite basse, on d�sactive les boutons pour reculer
            if (nbElementsMinimum == 0)
            {
                btnPremiersElems.Enabled = false;
                btnMoinsElem.Enabled = false;
                btnMoinsUnElem.Enabled = false;

                if (maximumAtteint)
                {
                    btnDerniersElems.Enabled = false;
                    btnPlusElem.Enabled = false;
                    btnPlusUnElem.Enabled = false;
                }
                else
                {
                    btnDerniersElems.Enabled = true;
                    btnPlusElem.Enabled = true;
                    btnPlusUnElem.Enabled = true;
                }



            }
            //Si on a atteint le maximum, on d�sactive les boutons pour avancer
            else if (maximumAtteint)
            {
                btnPremiersElems.Enabled = true;
                btnMoinsElem.Enabled = true;
                btnMoinsUnElem.Enabled = true;

                btnDerniersElems.Enabled = false;
                btnPlusElem.Enabled = false;
                btnPlusUnElem.Enabled = false;

                limiteHaute = (nbElementsMaximumTable + listeAjouts.Count);
            }
            else
            {
                btnPremiersElems.Enabled = true;
                btnMoinsElem.Enabled = true;
                btnMoinsUnElem.Enabled = true;

                btnDerniersElems.Enabled = true;
                btnPlusElem.Enabled = true;
                btnPlusUnElem.Enabled = true;
            }

            labelNbDonnees.Text = $"{limiteBasse} - {limiteHaute}";

            //Si la liste des tables n'est pas s�lectionn�, on empeche la manipulation
            if (listeTables.SelectedIndex == -1)
            {
                btnAjouterDonnee.Enabled = false;
                btnSupprimerDonnee.Enabled = false;
                btnModifierDonnee.Enabled = false;
            }
            else
            {
                btnAjouterDonnee.Enabled = true;
                btnSupprimerDonnee.Enabled = true;
                btnModifierDonnee.Enabled = true;
            }
        }

        //V�rifie si il ya des choses � sauvegarder
        private void ControleSauvegarde()
        {
            nbModifications = listeAjouts.Count + listeSuppressions.Count + listeModifications.Count;

            using (var context = new AppDbContext())
            {

                if (nbModifications > 0)
                {
                    labelModifications.Text = $"Modification(s) en attente : {nbModifications}";
                    btnSauvegarder.Enabled = true;
                }
                else
                {
                    labelModifications.Text = "Modification(s) en attente : 0";
                    btnSauvegarder.Enabled = false;
                }
            }

        }

        //Sauvegarde dans la BDD
        private void SauvegarderModifications()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    foreach (var element in listeAjouts)
                    {
                        switch ((TableBD)idDerniereTableChoisie)
                        {
                            case TableBD.Employes:
                                context.Add((Employe)element);
                                break;
                            case TableBD.Departements:
                                context.Add((Departement)element);
                                break;
                            case TableBD.Projets:
                                context.Add((Projet)element);
                                break;
                            case TableBD.EmployesProjets:
                                context.Add((EmployeProjet)element);
                                break;
                        }

                    }

                    foreach (var element in listeSuppressions)
                    {
                        switch ((TableBD)idDerniereTableChoisie)
                        {
                            case TableBD.Employes:
                                context.Remove((Employe)element);
                                break;
                            case TableBD.Departements:
                                context.Remove((Departement)element);
                                break;
                            case TableBD.Projets:
                                context.Remove((Projet)element);
                                break;
                            case TableBD.EmployesProjets:
                                context.Remove((EmployeProjet)element);
                                break;
                        }

                    }

                    foreach (var element in listeModifications)
                    {
                        switch ((TableBD)idDerniereTableChoisie)
                        {
                            case TableBD.Employes:
                                context.Update((Employe)element);
                                break;
                            case TableBD.Departements:
                                context.Update((Departement)element);
                                break;
                            case TableBD.Projets:
                                context.Update((Projet)element);
                                break;
                            case TableBD.EmployesProjets:
                                context.Update((EmployeProjet)element);
                                break;
                        }

                    }

                    //Envoi dans la base de donn�es
                    context.SaveChanges();

                    messagesUI.MessageInformation("Modifications sauvegard�es.", "Sauvegarde r�ussie");

                }
            }
            catch (Exception)
            {
                messagesUI.MessageErreur("Erreur lors de la sauvegarde des modifications.", "Erreur de sauvegarde");
                return;
            }

            ReinitialiserListesChangements();


        }

        private void AnnulerModifications()
        {
            ReinitialiserListesChangements();

            messagesUI.MessageInformation("Modifications annul�es.", "R�initialisation");
        }

        //R�initialise apr�s changement ou annulation
        private void ReinitialiserListesChangements()
        {
            listeAjouts.Clear();
            listeModifications.Clear();
            listeSuppressions.Clear();

            using (AppDbContext context = new AppDbContext())
            {
                prochaineValeurID[0] = context.Employes.Count() + 1;
                prochaineValeurID[1] = context.Departements.Count() + 1;
                prochaineValeurID[2] = context.Projets.Count() + 1;
            }

            RafrachirAffichage_Table((TableBD)idDerniereTableChoisie);
        }

        #endregion

    }

}
