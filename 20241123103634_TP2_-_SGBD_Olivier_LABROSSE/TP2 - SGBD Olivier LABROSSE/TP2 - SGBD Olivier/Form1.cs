using Microsoft.EntityFrameworkCore;
using System.Data;
using static TP2___SGBD_Olivier.ApplicationDBContext;


/*!
 * \file  "Form1.cs"
 *
 * \brief 
 *      Fichier contenant le code permettant de g�rer l'affichage des tables de la base de donn�es dans une fen�tre
 *      Il g�re �galement les actions sur les tables (ajout, modification, suppression)
 *      Un syst�me de pagination est �galement mis en place ainsi qu'un syst�me de sauvagarde des actions pour un commit/rollback sur la base de donn�es
 *        
 * \author Olivier LABROSSE
 * \date 4/11/2024
 * \last update 23/11/2024
 *
 */


namespace TP2___SGBD_Olivier
{
    public partial class Affichagetable : Form
    {
        // Champs de la classe Affichagetable
        int m_nIndiceMaximumActuel = 1; // Valeur maximale actuelle
        int m_nIndiceMinimumActuel = 1; // Valeur minimale actuelle
        int m_nIndiceMaximum = 1; // indice maximal
        const int m_nNbElementsParPage = 10000; // Nombre d'�l�ments par page
        List<CTableAction> m_listeActions = new List<CTableAction>(); // Liste des actions effectu�es. Utile pour le commit/rollback


        Table TableActuelle = Table.Aucune; // Table actuelle
        public enum Table
        {
            Departments = 0, // D�partements
            Employee_projects, // Projets des employ�s
            Employees, // Employ�s
            Projects, // Projets
            AffichageUnDep, // Affichage d'un d�partement
            Aucune // Aucune table
        }
        public Affichagetable() // constructeur
        {
            InitializeComponent(); // Fonction par d�faut pour cr�er la fen�tre
            dataGridViewTableSGBD.ClearSelection(); // Effacer la s�lection du DataGridView
            MAJBoutons(); // Mettre � jour les boutons
            MAJDropDownDepartments(); // Mettre � jour le menu d�roulant des d�partements
            Console.SetOut(new TextBoxWriter(textBox1)); // Rediriger la sortie console vers un TextBox (pour les logs)
            // Par d�faut, les logs ne sont pas visibles mais peuvent �tre montr�s via le designer de la fen�tre
        }

        /// <summary>
        /// V�rifie s'il faut ajouter/supprimer un d�partement dans le dropdown d'affichage des employ�s pour un d�partement
        /// </summary>
        public void MAJDropDownDepartments()
        {
            using (var contexte = new ApplicationDBContext())
            {

                if (toolStripDropDownButtonAffichageUnDep.DropDownItems.Count != contexte.departments.Count())
                {
                    toolStripDropDownButtonAffichageUnDep.DropDownItems.Clear();
                    foreach (Departments d in contexte.departments.ToList())
                    {
                        toolStripDropDownButtonAffichageUnDep.DropDownItems.Add(d.department_name);
                        toolStripDropDownButtonAffichageUnDep.DropDownItems[toolStripDropDownButtonAffichageUnDep.DropDownItems.Count - 1].Click += testToolStripMenuItem_Click;
                    }
                }
            }
        }

        private void MenuTable_Load(object sender, EventArgs e) {/* RIEN */}
        #region Bouttonpagination

        /// <summary>
        /// Met � jour les boutons de paginations, les boutons d'ajout, de modification des lignes et de suppression des lignes ainsi que les boutons de navigation
        /// </summary>
        /// <param name="desactiver">Param�tre pour forcer des boutons de pagination d�sactiv�s</param>
        public void MAJBoutons(bool desactiver = false)
        {
            SetEtatBoutonsPrecedents(desactiver);
            SetEtatBoutonsSuivant(desactiver);
            bool montrer = TableActuelle != Table.Aucune && TableActuelle != Table.AffichageUnDep;
            buttonAjouterLigne.Enabled = montrer;
            buttonSupprimerLigne.Enabled = montrer;
            buttonModifierLigne.Enabled = montrer;
        }
        /// <summary>
        /// Change l'�tat des boutons de pagination pr�c�dents
        /// </summary>
        /// <param name="desactiver">bool pour savoir si les boutons doivent �tre d�sacti�s par d�faut</param>
        public void SetEtatBoutonsPrecedents(bool desactiver = false)
        {
            if (desactiver)
            {
                buttonMoinsUn.Enabled = false;
                buttonPagePrecedente.Enabled = false;
                buttonPremierePage.Enabled = false;
            }
            else
            {
                buttonMoinsUn.Enabled = m_nIndiceMaximumActuel > 1;
                buttonPagePrecedente.Enabled = m_nIndiceMaximumActuel > m_nNbElementsParPage + 1;
                buttonPremierePage.Enabled = m_nIndiceMinimumActuel > 1;
            }
        }
        /// <summary>
        /// Change l'�tat des boutons de pagination suivants
        /// </summary>
        /// <param name="desactiver">bool pour savoir si les boutons doivent �tre d�sacti�s par d�faut</param>
        public void SetEtatBoutonsSuivant(bool desactiver = false)
        {
            if (desactiver)
            {
                buttonPlusUn.Enabled = false;
                buttonProchainePage.Enabled = false;
                buttonDernierePage.Enabled = false;
            }
            buttonPlusUn.Enabled = m_nIndiceMaximumActuel < m_nIndiceMaximum;
            buttonProchainePage.Enabled = m_nIndiceMaximumActuel < m_nIndiceMaximum - m_nNbElementsParPage;
            buttonDernierePage.Enabled = m_nIndiceMaximumActuel < m_nIndiceMaximum;
        }
        // Precedents
        private void buttonPremierePage_Click(object sender, EventArgs e)
        {
            m_nIndiceMinimumActuel = 1;
            m_nIndiceMaximumActuel = 1;
            DemandeDeSauvegarde();
            MontreLignes(TableActuelle, m_nIndiceMinimumActuel - 1, m_nNbElementsParPage);

        }
        private void buttonPagePrecedente_Click(object sender, EventArgs e)
        {
            DemandeDeSauvegarde();
            m_nIndiceMinimumActuel = m_nIndiceMinimumActuel - m_nNbElementsParPage;
            m_nIndiceMinimumActuel = m_nIndiceMinimumActuel < 1 ? 1 : m_nIndiceMinimumActuel;
            MontreLignes(TableActuelle, m_nIndiceMinimumActuel, -m_nNbElementsParPage);
        }

        private void buttonMoinsUn_Click(object sender, EventArgs e)
        {
            DemandeDeSauvegarde();
            GetLigneIndice(TableActuelle, m_nIndiceMaximumActuel - 1);
        }

        // Suivant
        private void buttonDernierePage_Click(object sender, EventArgs e)
        {
            DemandeDeSauvegarde();
            m_nIndiceMinimumActuel = m_nIndiceMaximum - m_nIndiceMaximum % m_nNbElementsParPage;
            MontreLignes(TableActuelle, m_nIndiceMaximum - (m_nIndiceMaximum % m_nNbElementsParPage));
            m_nIndiceMaximumActuel = m_nIndiceMaximum;
        }
        private void buttonProchainePage_Click(object sender, EventArgs e)
        {
            DemandeDeSauvegarde();
            int nbElements = m_nNbElementsParPage;
            if (m_nIndiceMaximumActuel + m_nNbElementsParPage > m_nIndiceMaximum)
            {
                m_nIndiceMinimumActuel = m_nIndiceMaximumActuel;
                nbElements = m_nIndiceMaximum % m_nNbElementsParPage;
            }
            m_nIndiceMinimumActuel = m_nIndiceMaximumActuel;
            MontreLignes(TableActuelle, m_nIndiceMinimumActuel, nbElements);
        }

        private void buttonPlusUn_Click(object sender, EventArgs e)
        {
            DemandeDeSauvegarde();
            GetLigneIndice(TableActuelle, m_nIndiceMaximumActuel + 1);
        }
        #endregion
        private void MontreTable(Table table)
        {
            toolStripButtonDepartement.Enabled = (table != Table.Departments);
            toolStripButtonEmployees.Enabled = (table != Table.Employees);
            toolStripButtonEmployeeProject.Enabled = (table != Table.Employee_projects);
            toolStripButtonProjects.Enabled = (table != Table.Projects);
            MontreLignes(table, 0, m_nNbElementsParPage);
        }

        #region Row(s) selection
        public void GetLigneIndice(Table table, int indice)
        {
            Thread.Sleep(100);
            m_nIndiceMinimumActuel = indice;
            m_nIndiceMaximumActuel = indice;
            indice--;
            MontreLigneAPartirDe(table, indice, 1);
            MAJPages();
            MAJBoutons();
        }

        public void MontreLigneAPartirDe(Table table, int indice, int nbValues = m_nNbElementsParPage)
        {
            using (var contexte = new ApplicationDBContext())
            {
                switch (table)
                {
                    case Table.Departments:
                        List<Departments> contenuDep = contexte.departments
                            .OrderBy(d => d.department_id)
                            .Skip(indice)
                            .Take(Math.Abs(nbValues))
                            .ToList();
                        m_listeActions.OrderBy(a => a.idAction);
                        foreach (CTableAction action in m_listeActions)
                        {
                            switch (action.GetTypeAction())
                            {
                                case CTableAction.TypeAction.Ajout:
                                    contenuDep.Add((Departments)action.GetLigne());
                                    break;
                                case CTableAction.TypeAction.Modification:
                                    CTableModification modif = (CTableModification)action;
                                    Departments dep = (Departments)modif.GetAncienneLigne();
                                    int indice2 = contenuDep.FindIndex(d => d.department_id == dep.department_id);
                                    if (indice2 != -1)
                                        contenuDep[indice2] = (Departments)action.GetLigne();
                                    break;
                                case CTableAction.TypeAction.Suppression:
                                    contenuDep.Remove(contenuDep.Find(d => d.department_id == ((Departments)action.GetLigne()).department_id));
                                    break;
                            }
                        }
                        dataGridViewTableSGBD.DataSource = contenuDep;
                        break;
                    case Table.Employees:
                        List<Employees> contenuEmp = contexte.employees
                            .Include(e => e.departement) // Joindre la table departments
                            .OrderBy(e => e.employee_id)
                            .Skip(indice)
                            .Take(Math.Abs(nbValues))
                            .ToList();
                        foreach (CTableAction action in m_listeActions)
                        {
                            switch (action.GetTypeAction())
                            {
                                case CTableAction.TypeAction.Ajout:
                                    contenuEmp.Add((Employees)action.GetLigne());
                                    break;
                                case CTableAction.TypeAction.Modification:
                                    CTableModification modif = (CTableModification)action;
                                    Employees emp = (Employees)modif.GetAncienneLigne();
                                    int indice2 = contenuEmp.FindIndex(e => e.employee_id == emp.employee_id);
                                    if (indice2 != -1)
                                        contenuEmp[indice2] = (Employees)action.GetLigne();
                                    break;
                                case CTableAction.TypeAction.Suppression:
                                    contenuEmp.Remove((Employees)action.GetLigne());
                                    break;
                            }
                        }
                        dataGridViewTableSGBD.DataSource = contenuEmp;
                        break;
                    case Table.Employee_projects:
                        var contenuEmpProj = contexte.employee_projects
                            .Include(ep => ep.employee)
                            .Include(ep => ep.project)
                            .OrderBy((ep) => ep.employee_id)
                            .Skip(indice)
                            .Take(Math.Abs(nbValues))
                            .ToList();
                        foreach (CTableAction action in m_listeActions)
                        {
                            switch (action.GetTypeAction())
                            {
                                case CTableAction.TypeAction.Ajout:
                                    contenuEmpProj.Add((Employee_projects)action.GetLigne());
                                    break;
                                case CTableAction.TypeAction.Modification:
                                    CTableModification modif = (CTableModification)action;
                                    Employee_projects emp = (Employee_projects)modif.GetAncienneLigne();
                                    int indice2 = contenuEmpProj.FindIndex(e => e.employee_id == emp.employee_id);
                                    if (indice2 != -1)
                                        contenuEmpProj[indice2] = (Employee_projects)action.GetLigne();
                                    break;
                                case CTableAction.TypeAction.Suppression:
                                    contenuEmpProj.Remove((Employee_projects)action.GetLigne());
                                    break;
                            }
                        }
                        dataGridViewTableSGBD.DataSource = contenuEmpProj;
                        break;
                    case Table.Projects:
                        var contenuProj = contexte.projects
                            .OrderBy((p) => p.project_id)
                            .Skip(indice)
                            .Take(Math.Abs(nbValues))
                            .ToList();
                        foreach (CTableAction action in m_listeActions)
                        {
                            switch (action.GetTypeAction())
                            {
                                case CTableAction.TypeAction.Ajout:
                                    contenuProj.Add((Projects)action.GetLigne());
                                    break;
                                case CTableAction.TypeAction.Modification:
                                    CTableModification modif = (CTableModification)action;
                                    Projects proj = (Projects)modif.GetAncienneLigne();
                                    int indice2 = contenuProj.FindIndex(e => e.project_id == proj.project_id);
                                    if (indice2 != -1)
                                        contenuProj[indice2] = (Projects)action.GetLigne();
                                    break;
                                case CTableAction.TypeAction.Suppression:
                                    contenuProj.Remove((Projects)action.GetLigne());
                                    break;
                            }
                        }
                        dataGridViewTableSGBD.DataSource = contenuProj;
                        break;
                    case Table.AffichageUnDep:
                        break;

                    default:
                        throw new Exception("Table inconnue");
                }
            }
        }


        public void MontreLignes(Table table, int indice, int nbValues = m_nNbElementsParPage)
        {
            if (indice > 0)
                indice--;
            MontreLigneAPartirDe(table, indice, nbValues);
            m_nIndiceMaximumActuel = m_nIndiceMinimumActuel + Math.Abs(nbValues);
            m_nIndiceMaximumActuel = m_nIndiceMaximumActuel < m_nIndiceMaximum ? m_nIndiceMaximumActuel : m_nIndiceMaximum;
            MAJPages();
            MAJBoutons();
        }

        #endregion

        #region AjoutEtModification

        #region Departements
        private void AjouterDepartement()
        {
            using (var contexte = new ApplicationDBContext())
            {
                string nom = GetTexteEntre("Quel est le nom du d�partement ?", "Cr�ation d'un d�partement", "Nouveau departement", 255, out bool quitter);
                if (quitter) return;
                Departments dep = new Departments();
                dep.department_name = nom;
                int nbAjouts = 0;
                foreach (CTableAction action in m_listeActions)
                {
                    if (action.GetTypeAction() == CTableAction.TypeAction.Ajout)
                    {
                        nbAjouts++;
                    }
                }
                dep.department_id = contexte.departments.Count() + nbAjouts + 1; // id temporaire en local
                m_listeActions.Add(new CTableAjout(dep));
                MessageBox.Show($"D�partement {dep.department_name} ajout�");
                MAJDropDownDepartments();
                MontreLignes(TableActuelle, m_nIndiceMinimumActuel, m_nNbElementsParPage);
                m_nIndiceMaximum++;
            }
        }

        public void ModifierDepartement(bool LigneSelectionne = false)
        {
            bool quitter = false;
            Departments? dep;
            using (var contexte = new ApplicationDBContext())
            {
                contexte.Database.OpenConnection();
                if (!LigneSelectionne)
                {
                    int id = GetEntierEntree("Quel est l'id du d�partement � modifier ?", "Modification d'un d�partement", "0", 10, out quitter);
                    if (quitter) return;
                    dep = contexte.departments.FirstOrDefault((d) => d.department_id == id);
                    var resultat = RegardeSiExisteDansListeActions(Table.Departments, id);
                    if (resultat != null)
                    {
                        if (resultat.GetTypeAction() == CTableAction.TypeAction.Suppression)
                        {
                            MessageBox.Show("Le d�partement a �t� supprim�. Impossible de le modifier", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            dep = (Departments)resultat.GetLigne();
                        }
                    }
                }
                else
                    dep = (Departments)dataGridViewTableSGBD.CurrentRow.DataBoundItem;
                if (dep != null)
                {
                    string nom = GetTexteEntre("Quel est le nouveau nom du d�partement ?", "Modification d'un d�partement", dep.department_name, 255, out quitter);
                    if (quitter) return;
                    m_listeActions.Add(new CTableModification(dep, new Departments(nom, dep.department_id)));
                }
                else
                {
                    MessageBox.Show("D�partement non trouv�", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Employees
        private void AjouterEmployee()
        {
            using (var contexte = new ApplicationDBContext())
            {
                contexte.Database.OpenConnection();
                bool quitter = false;
                Employees emp = new Employees();
                emp.first_name = GetTexteEntre("Quel est le pr�nom de l'employ� ?", "Cr�ation d'un employ�", "Nouveau employ�", 255, out quitter);
                if (quitter) return;
                emp.last_name = GetTexteEntre("Quel est le nom de l'employ� ?", "Cr�ation d'un employ�", "Nouveau employ�", 255, out quitter);
                if (quitter) return;
                emp.hire_date = GetDateEntree("Veuillez entrer la date d'embauche de l'employ� (yyyy-mm-dd)", "Cr�ation d'un employ�", "yyyy-mm-dd", out quitter);
                if (quitter) return;
                emp.salary = GetFloatEntree("Quel est le salaire de l'employ� ?", "Cr�ation d'un employ�", "0", 10, out quitter);
                if (quitter) return;
                Departments? dep = null;
                if (MessageBox.Show("Voulez vous assigner le nouvel employ� � un d�partement", "Cr�ation d'un employ�", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    emp.department_id = GetEntierEntree("Quel est l'id du d�partement de l'employ� ?", "Cr�ation d'un employ�", "0", 10, out quitter);
                    dep = contexte.departments.FirstOrDefault((d) => d.department_id == emp.department_id);
                    if (dep == null)
                    {
                        MessageBox.Show("Le d�partement n'a pas �t� trouv�", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        emp.department_id = null;
                        quitter = true;
                    }
                }
                if (quitter) return;
                emp.employee_id = contexte.employees.Count() + 1; // id temporaire en local
                m_listeActions.Add(new CTableAjout(emp));
                if (dep != null)
                {
                    MessageBox.Show($"Employ� {emp.first_name} {emp.last_name} ajout� au departement {dep.department_name} ayant l'id {dep.department_id}");
                }
                else
                {
                    MessageBox.Show($"Employ� {emp.first_name} {emp.last_name} ajout�");
                }
                MontreLignes(TableActuelle, m_nIndiceMinimumActuel, m_nNbElementsParPage);
                m_nIndiceMaximum++;
            }
        }

        private void ModifierEmployee(bool LigneSelectionne)
        {
            using (var contexte = new ApplicationDBContext())
            {
                contexte.Database.OpenConnection();
                bool quitter = false;
                Employees? emp;
                if (!LigneSelectionne)
                {
                    int id = GetEntierEntree("Quel est l'id de l'employ� � modifier ?", "Modification d'un employ�", "0", 10, out quitter);
                    emp = contexte.employees.FirstOrDefault((e) => e.employee_id == id);
                    var resultat = RegardeSiExisteDansListeActions(Table.Employees, id);
                    if (resultat != null)
                    {
                        if (resultat.GetTypeAction() == CTableAction.TypeAction.Suppression)
                        {
                            MessageBox.Show("L'employ� a �t� supprim�. Impossible de le modifier", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            emp = (Employees)resultat.GetLigne();
                        }
                    }
                    if (quitter) return;
                }
                else
                    emp = (Employees)dataGridViewTableSGBD.CurrentRow.DataBoundItem;
                if (emp == null)
                {
                    MessageBox.Show("Employ� non trouv�", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string first_name = GetTexteEntre("Quel est le pr�nom de l'employ� ?", "Modification d'un employ�", emp.first_name, 255, out quitter);
                if (quitter) return;
                string last_name = GetTexteEntre("Quel est le nom de l'employ� ?", "Modification d'un employ�", emp.last_name, 255, out quitter);
                if (quitter) return;
                DateTime hire_date = GetDateEntree("Veuillez entrer la date d'embauche de l'employ� (yyyy-mm-dd)", "Modification d'un employ�", emp.hire_date.ToString(), out quitter);
                if (quitter) return;
                float salary = GetFloatEntree("Quel est le salaire de l'employ� ?", "Modification d'un employ�", emp.salary.ToString(), 10, out quitter);
                if (quitter) return;
                int? department_id = emp.department_id; Departments? dep = emp.departement;
                if (MessageBox.Show("Voulez vous assigner le nouvel employ� � un d�partement", "Modification d'un employ�", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    department_id = GetEntierEntree("Quel est l'id du d�partement de l'employ� ?", "Modification d'un employ�", emp.department_id.ToString(), 10, out quitter);
                    dep = contexte.departments.FirstOrDefault((d) => d.department_id == emp.department_id);
                    if (dep == null)
                    {
                        MessageBox.Show("Le d�partement n'a pas �t� trouv�. L'employ�e garde le pr�c�dent d�partement.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dep = emp.departement;
                    }
                }
                m_listeActions.Add(new CTableModification(emp, new Employees(first_name, last_name, hire_date, department_id, salary, dep, emp.employee_projects, emp.employee_id)));
            }
        }

        #endregion

        #region EmployeeProjects
        private void AjouterEmployee_projects()
        {
            using (var contexte = new ApplicationDBContext())
            {
                contexte.Database.OpenConnection();
                bool quitter = false;
                bool found = false;
                int employee_id = -1;
                Employees? emp = null;
                while (!found)
                {
                    employee_id = GetEntierEntree("Quel est l'id de l'employ� ?", "Ajout d'un employ� sur un projet", "0", 10, out quitter);
                    if (quitter) return;
                    emp = contexte.employees.Where((e) => e.employee_id == employee_id).FirstOrDefault();
                    if (emp != null)
                        found = true;
                    else
                        MessageBox.Show("Employ� non trouv�. Veuillez r�essayer.");
                }
                found = false;
                int project_id = -1;
                Projects? proj = null;
                while (!found)
                {
                    project_id = GetEntierEntree("Quel est l'id du projet ?", "Ajout d'un employ� sur un projet�", "0", 10, out quitter);
                    if (quitter) return;
                    proj = contexte.projects.Where((p) => p.project_id == project_id).FirstOrDefault();
                    if (proj != null)
                        found = true;
                    else
                        MessageBox.Show("Projet non trouv�. Veuillez r�essayer.");
                }

                CTableAction searched = RegardeSiExisteDansListeActions(Table.Employee_projects, employee_id, project_id);
                if (contexte.employee_projects.FirstOrDefault((ep) => ep.employee_id == employee_id && ep.project_id == project_id) != null || (searched != null && searched.GetTypeAction() != CTableAction.TypeAction.Suppression))
                {
                    MessageBox.Show("L'employ� est d�j� dans ce projet", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string role = GetTexteEntre("Quel est le r�le de l'employ� dans le projet ?", "Ajout d'un employ� sur un projet", "Nouveau employ�", 255, out quitter);
                if (quitter) return;
                Employee_projects ep = new Employee_projects(employee_id, project_id, role);
                m_listeActions.Add(new CTableAjout(ep));
                MessageBox.Show($"Employ� n�{employee_id} {emp?.first_name} {emp?.last_name} ajout� au projet n�{project_id} {proj?.project_id} avec le r�le {role}");
                m_nIndiceMaximum++;
            }
        }

        private void ModifierEmployee_projects(bool LigneSelectionne)
        {
            Employee_projects? AncienEmpProj = null;
            using (var contexte = new ApplicationDBContext())
            {
                contexte.Database.OpenConnection();
                bool quitter = false;
                int employee_id = -1;
                int project_id = -1;
                if (!LigneSelectionne)
                {
                    bool found = false;
                    while (!found)
                    {
                        employee_id = GetEntierEntree("Quel est l'id de l'employ� ?", "Ajout d'un employ� sur un projet", "0", 10, out quitter);
                        if (quitter) return;
                        project_id = GetEntierEntree("Quel est l'id du projet ?", "Ajout d'un employ� sur un projet�", "0", 10, out quitter);
                        if (quitter) return;
                        AncienEmpProj = contexte.employee_projects.Where((ep) => ep.employee_id == employee_id && ep.project_id == project_id).FirstOrDefault();
                        if (AncienEmpProj != null)
                            found = true;
                        else
                            MessageBox.Show("Impossible de trouver l'entr�e souhait�e. Veuillez r�essayer.");
                    }
                }
                else
                {
                    AncienEmpProj = (Employee_projects)dataGridViewTableSGBD.CurrentRow.DataBoundItem;
                }
                string role = GetTexteEntre("Quel est le r�le de l'employ� dans le projet ?", "Modification d'un employ� sur un projet", ((Employee_projects)dataGridViewTableSGBD.CurrentRow.DataBoundItem).role, 255, out quitter);
                if (quitter) return;
                Employee_projects ep = new Employee_projects(employee_id, project_id, role);
                if (AncienEmpProj == null)
                {
                    MessageBox.Show("Erreur dans la r�cup�ration de la ligne", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m_listeActions.Add(new CTableModification(AncienEmpProj, ep));
                m_nIndiceMaximum++;
            }
        }

        #endregion

        #region Projets
        private void AjouterProjet()
        {
            using (var contexte = new ApplicationDBContext())
            {
                contexte.Database.OpenConnection();
                bool quitter = false;
                Projects proj = new Projects();
                proj.project_name = GetTexteEntre("Quel est le nom du projet ?", "Cr�ation d'un projet", "Nouveau projet", 255, out quitter);
                if (quitter) return;
                proj.start_date = GetDateEntree("Veuillez entrer la date de d�but du projet (yyyy-mm-dd)", "Cr�ation d'un projet", "yyyy-mm-dd", out quitter);
                if (quitter) return;
                proj.end_date = GetDateEntree("Veuillez entrer la date de fin du projet (yyyy-mm-dd)", "Cr�ation d'un projet", "yyyy-mm-dd", out quitter);
                if (quitter) return;
                proj.project_id = contexte.projects.Count() + 1; // id temporaire en local
                m_listeActions.Add(new CTableAjout(proj));
                m_nIndiceMaximum++;
            }
        }

        private void ModifierProjet(bool LigneSelectionne)
        {
            using (var contexte = new ApplicationDBContext())
            {
                contexte.Database.OpenConnection();
                bool quitter = false;
                int id = GetEntierEntree("Quel est l'id du projet � modifier ?", "Modification d'un projet", "0", 10, out quitter);
                if (quitter) return;
                Projects? proj = contexte.projects.FirstOrDefault((p) => p.project_id == id);
                string project_name = GetTexteEntre("Quel est le nom du projet ?", "Modification d'un projet", proj.project_name, 255, out quitter);
                if (quitter) return;
                DateTime start_date = GetDateEntree("Veuillez entrer la date de d�but du projet (yyyy-mm-dd)", "Modification d'un projet", proj.start_date.ToString(), out quitter);
                if (quitter) return;
                DateTime end_date = GetDateEntree("Veuillez entrer la date de fin du projet (yyyy-mm-dd)", "Modification d'un projet", proj.end_date.ToString(), out quitter);
                if (quitter) return;
                m_listeActions.Add(new CTableModification(proj, new Projects(project_name, start_date, end_date)));
            }
        }

        #endregion

        #endregion

        #region Events

        #region MenuTables
        private void toolStripButtonDepartement_Click(object sender, EventArgs e)
        {
            DemandeDeSauvegarde();
            TableActuelle = Table.Departments;
            m_nIndiceMinimumActuel = 1;
            m_nIndiceMaximumActuel = 1;
            using (var contexte = new ApplicationDBContext())
            {
                m_nIndiceMaximum = contexte.departments.Count();
            }
            ActiverDropdownDepartements();
            MontreTable(TableActuelle);
        }
        private void toolStripButtonEmployeeProject_Click(object sender, EventArgs e)
        {
            DemandeDeSauvegarde();
            TableActuelle = Table.Employee_projects;
            m_nIndiceMinimumActuel = 1;
            m_nIndiceMaximumActuel = 1;
            using (var contexte = new ApplicationDBContext())
            {
                m_nIndiceMaximum = contexte.employee_projects.Count();
            }
            ActiverDropdownDepartements();
            MontreTable(TableActuelle);
        }

        private void toolStripButtonEmployees_Click(object sender, EventArgs e)
        {
            DemandeDeSauvegarde();
            TableActuelle = Table.Employees;
            m_nIndiceMinimumActuel = 1;
            m_nIndiceMaximumActuel = 1;
            using (var contexte = new ApplicationDBContext())
            {
                m_nIndiceMaximum = contexte.employees.Count();
            }
            ActiverDropdownDepartements();
            MontreTable(TableActuelle);
        }

        private void toolStripButtonProjects_Click(object sender, EventArgs e)
        {
            DemandeDeSauvegarde();
            TableActuelle = Table.Projects;
            m_nIndiceMinimumActuel = 1;
            m_nIndiceMaximumActuel = 1;
            using (var contexte = new ApplicationDBContext())
            {
                m_nIndiceMaximum = contexte.projects.Count();
            }
            ActiverDropdownDepartements();
            MontreTable(TableActuelle);
        }

        private void ActiverDropdownDepartements()
        {
            foreach (var item in toolStripDropDownButtonAffichageUnDep.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                    ((ToolStripMenuItem)item).Enabled = true;
            }
        }
        private void toolStripDropDownButtonAffichageUnDep_Click(object sender, EventArgs e) { /* RIEN */}

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DemandeDeSauvegarde();
            foreach (var item in toolStripDropDownButtonAffichageUnDep.DropDownItems) // on active les du dropdown des d�partemnts
            {
                if (item is ToolStripMenuItem)
                    ((ToolStripMenuItem)item).Enabled = true;
            }
            Show();
            // selon la valeur de l'item cliqu�, on affiche les lignes correspondantes
            string nom = ((ToolStripMenuItem)sender).Text; // on r�cup�re le nom de d�partement
            ((ToolStripMenuItem)sender).Enabled = false; // on d�sactive l'item cliqu�
            using (var contexte = new ApplicationDBContext())
            {
                Departments? dep = contexte.departments.Include((d) => d.employees).Where((d) => d.department_name == nom).FirstOrDefault(); // on r�cup�re le d�partement
                if (dep != null) // si le d�partement existe
                {
                    MAJBoutons(true); // on d�sactive les boutons de pagination
                    ICollection<Employees>? employees = dep.employees.OrderBy((e) => e.employee_id).ToList(); // on les r�cup�res dans une collection
                    dataGridViewTableSGBD.DataSource = employees; // on les affiche
                    m_nIndiceMaximum = m_nIndiceMaximumActuel = employees.Count;
                    m_nIndiceMinimumActuel = 1;
                    MAJPages();
                    TableActuelle = Table.AffichageUnDep; // on met � jour la table courante
                }
            }
            MontreTable(Table.AffichageUnDep); // r�active toutes les options de la navbar
        }
        #endregion

        private void dataGridViewTableSGBD_CellContentClick(object sender, DataGridViewCellEventArgs e) { return; }
        private void TexteNumPage_Click(object sender, EventArgs e) {/* RIEN */ }
        private void buttonAjouterLigne_Click(object sender, EventArgs e)
        {
            switch (TableActuelle)
            {
                case Table.Departments:
                    AjouterDepartement();
                    break;
                case Table.Employees:
                    AjouterEmployee();
                    break;
                case Table.Employee_projects:
                    AjouterEmployee_projects();
                    break;
                case Table.Projects:
                    AjouterProjet();
                    break;
                default:
                    throw new Exception("Table inconnue");
            }
            MontreLignes(TableActuelle, m_nIndiceMinimumActuel, m_nNbElementsParPage);
        }
        private void buttonModifierLigne_Click(object sender, EventArgs e)
        {
            switch (TableActuelle)
            {
                case Table.Departments:
                    ModifierDepartement(checkBoxLigneSelectionnee.Checked);
                    break;
                case Table.Employees:
                    ModifierEmployee(checkBoxLigneSelectionnee.Checked);
                    break;
                case Table.Employee_projects:
                    ModifierEmployee_projects(checkBoxLigneSelectionnee.Checked);
                    break;
                case Table.Projects:
                    ModifierProjet(checkBoxLigneSelectionnee.Checked);
                    break;
                default:
                    throw new Exception("Table inconnue");
            }
            MontreLignes(TableActuelle, m_nIndiceMinimumActuel, m_nNbElementsParPage);
        }
        private void buttonSupprimerLigne_Click(object sender, EventArgs e)
        {
            SupprimerCellule(checkBoxLigneSelectionnee.Checked);
        }

        /// <summary>
        /// Permet de demander � l'utilisateur s'il souhaite sauvegarder, ne pas sauvegarder ou annuler l'action (tentative de changement de page / fermeture de l'application)
        /// </summary>
        /// <returns>true : action r�alis�e | false : annulation de l'action</returns>
        private bool DemandeDeSauvegarde()
        {
            if (m_listeActions.Count < 1)
                return true;
            switch (EssaiQuitterApp($"{m_listeActions.Count} modifications en attente.\nVoulez vous sauvegarder les modifications ?"))
            {
                case optionSortie.Save:
                    using (var contexte = new ApplicationDBContext())
                    {
                        foreach (CTableAction action in m_listeActions)
                        {
                            switch (action.GetTypeAction())
                            {
                                case CTableAction.TypeAction.Ajout:
                                    if (action.GetLigne() is Departments dep)    
                                    { 
                                        // Permet d'utiliser l'incr�mentation automatique
                                        Departments nouveaudep = new Departments();
                                        nouveaudep.department_name = dep.department_name;
                                        contexte.departments.Add(nouveaudep);
                                    }
                                    else if (action.GetLigne() is Employees emp)
                                    {
                                        // Permet d'utiliser l'incr�mentation automatique
                                        Employees nouveauEmp = new Employees();
                                        nouveauEmp.first_name = emp.first_name;
                                        nouveauEmp.last_name = emp.last_name;
                                        nouveauEmp.department_id = emp.department_id;
                                        nouveauEmp.hire_date = emp.hire_date;
                                        nouveauEmp.salary = emp.salary;
                                        contexte.employees.Add(nouveauEmp);
                                    }
                                    else if (action.GetLigne() is Projects proj)
                                    {
                                        // Permet d'utiliser l'incr�mentation automatique
                                        Projects nouveauProj = new Projects();
                                        nouveauProj.project_name = proj.project_name;
                                        nouveauProj.start_date = proj.start_date;
                                        nouveauProj.end_date = proj.end_date;
                                        contexte.projects.Add(proj);
                                    }
                                    else if (action.GetLigne() is Employee_projects)
                                    {
                                        contexte.Add(action.GetLigne());
                                    }
                                    else
                                    {
                                        throw new Exception("Erreur lors de la sauvegarde : Type inconnu");
                                    }
                                    break;
                                case CTableAction.TypeAction.Modification:
                                    contexte.Update(action.GetLigne());
                                    break;
                                case CTableAction.TypeAction.Suppression:
                                    contexte.Remove(action.GetLigne());
                                    break;
                            }
                        }
                        contexte.SaveChanges();
                        m_listeActions.Clear();
                        return true;
                    }
                case optionSortie.NoSave:
                    if (MessageBox.Show("Les modifications ne seront pas sauvegard�es, �tes-vous s�r de quitter ?", "V�rification sauvegarde", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        m_listeActions.Clear();
                        return true;
                    }
                    else
                        return false;
                case optionSortie.Cancel:
                    return false;
                case optionSortie.Error:
                    MessageBox.Show("Erreur lors de la sauvegarde");
                    break;
            }
            return false;
        }
  
        private void Affichagetable_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DemandeDeSauvegarde() == false)
                e.Cancel = true;
            return;
        }
        private void buttonRechercher_Click(object sender, EventArgs e)
        {
            string searched = textBoxBarreDeRecherche.Text.ToLower(); // stockage dans une variable pour simplifier la lecture du code (moins cons�quent)
            DemandeDeSauvegarde();
            using (var contexte = new ApplicationDBContext())
            {
                // Pour une raison obscure, le contains sur les dates contenant les caract�res '/' ou '-' ne fonctionnent pas
                switch (TableActuelle)
                {
                    case Table.Departments:
                        ICollection<Departments> mylistDep = new List<Departments>();
                        var depContains = contexte.departments.Where((d) => d.department_name.ToLower().Contains(searched) || d.department_id.ToString().Contains(searched));
                        m_nIndiceMaximum = depContains.Count();
                        dataGridViewTableSGBD.DataSource = mylistDep = depContains.ToList();
                        break;
                    case Table.AffichageUnDep:
                        ICollection<Employees>? mylistEmpDep = new List<Employees>();
                        string depname = "";
                        foreach (var item in toolStripDropDownButtonAffichageUnDep.DropDownItems)
                        {
                            if (item is ToolStripMenuItem)
                            {
                                if (!((ToolStripMenuItem)item).Enabled)
                                {
                                    depname = ((ToolStripMenuItem)item).Text;
                                    break;
                                }
                            }
                        }
                        if (depname == "")
                            return;
                        mylistEmpDep = contexte.departments
                            .Include(d => d.employees)
                            .First((d) => d.department_name == depname).employees;
                        mylistEmpDep = mylistEmpDep
                            .Where((ep) => ep.first_name.ToLower().Contains(searched) || ep.last_name.ToLower().Contains(searched) || ep.salary.ToString().Contains(searched))
                            .ToList();
                        dataGridViewTableSGBD.DataSource = mylistEmpDep;
                        break;
                    case Table.Employees:
                        ICollection<Employees> mylistEmp = new List<Employees>();
                        mylistEmp = contexte.employees
                            .Include(e => e.departement)
                            .Where((e) => e.first_name.ToLower().Contains(searched) || e.last_name.ToLower().Contains(searched) || e.employee_id.ToString().Contains(searched) || e.salary.ToString().Contains(searched)
                        || e.departement.department_name.Contains(searched) || e.hire_date.ToString().Contains(searched))
                            .ToList();
                        dataGridViewTableSGBD.DataSource = mylistEmp;
                        break;
                    case Table.Employee_projects:
                        ICollection<Employee_projects> mylistEmpProj = contexte.employee_projects
                            .Include(ep => ep.employee)
                            .Include(ep => ep.project)
                            .Where((ep) => ep.role.ToLower().Contains(searched) || ep.project.project_name.ToLower().Contains(searched) || 
                                ep.employee.first_name.ToLower().Contains(searched) || ep.employee.last_name.ToLower().Contains(searched))
                            .ToList() ;
                        dataGridViewTableSGBD.DataSource = mylistEmpProj;
                        break;
                    case Table.Projects:
                        ICollection<Projects> mylistProj = contexte.projects.Where((p) => p.project_name.ToLower().Contains(searched) || p.project_id.ToString().ToLower().Contains(searched) || (p.start_date.ToString().Contains(searched) || p.end_date.ToString().Contains(searched))).ToList();
                        dataGridViewTableSGBD.DataSource = mylistProj;
                        break;
                }
                TexteNumPage.Text = $"{m_nIndiceMinimumActuel}-{m_nIndiceMaximumActuel - m_listeActions.Count} sur {m_nIndiceMaximum}";
            }
        }
        #endregion

        #region SupprimerCellule
        public void SupprimerCelluleByInput()
        {
            using (var contexte = new ApplicationDBContext())
            {
                switch (TableActuelle)
                {
                    case Table.Projects:
                        int id = GetEntierEntree("Quel est l'id du projet � supprimer ?", "Suppression d'un projet", "0", 10, out bool quitter);
                        if (quitter) return;
                        Projects? proj = contexte.projects.FirstOrDefault((p) => p.project_id == id);
                        if (proj != null)
                        {
                            m_listeActions.Add(new CTableSuppression(proj));
                            MessageBox.Show("Projet supprim�");
                        }
                        else
                        {
                            var resultat = RegardeSiExisteDansListeActions(TableActuelle, id);
                            if (resultat != null && resultat.GetTypeAction() != CTableAction.TypeAction.Suppression)
                                m_listeActions.Add(new CTableSuppression(resultat.GetLigne()));
                            else
                                MessageBox.Show("Projet non trouv�", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    case Table.Departments:
                        int idDep = GetEntierEntree("Quel est l'id du d�partement � supprimer ?", "Suppression d'un d�partement", "0", 10, out bool quitDep);
                        if (quitDep) return;
                        Departments? dep = contexte.departments.FirstOrDefault((d) => d.department_id == idDep);
                        if (dep != null)
                        {
                            m_listeActions.Add(new CTableSuppression(dep));
                            MessageBox.Show("D�partement supprim�");
                        }
                        else
                        {
                            var resultat = RegardeSiExisteDansListeActions(TableActuelle, idDep);
                            if (resultat != null && resultat.GetTypeAction() != CTableAction.TypeAction.Suppression)
                                m_listeActions.Add(new CTableSuppression(resultat.GetLigne()));
                            else
                                MessageBox.Show("D�partement non trouv�", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    case Table.Employees:
                        int idEmp = GetEntierEntree("Quel est l'id de l'employ� � supprimer ?", "Suppression d'un employ�", "0", 10, out bool quitEmp);
                        if (quitEmp) return;
                        Employees? emp = contexte.employees.FirstOrDefault((e) => e.employee_id == idEmp);
                        if (emp != null)
                        {
                            m_listeActions.Add(new CTableSuppression(emp));
                            MessageBox.Show("Employ� supprim�");
                        }
                        else
                        {
                            var resultat = RegardeSiExisteDansListeActions(TableActuelle, idEmp);
                            if (resultat != null && resultat.GetTypeAction() != CTableAction.TypeAction.Suppression)
                                m_listeActions.Add(new CTableSuppression(resultat.GetLigne()));
                            else
                                MessageBox.Show("Employ� non trouv�", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    case Table.Employee_projects:
                        int idEmploye = GetEntierEntree("Quel est l'id de l'employ� de la relation � supprimer ?", "Suppression d'un employ� sur un projet", "0", 10, out bool quitEmpProj);
                        if (quitEmpProj) return;
                        int idProj = GetEntierEntree("Quel est l'id du projet de la relation � supprimer ?", "Suppression d'un employ� sur un projet", "0", 10, out quitEmpProj);
                        if (quitEmpProj) return;
                        Employee_projects? empProj = contexte.employee_projects.FirstOrDefault((ep) => ep.employee_id == idEmploye && ep.project_id == idProj);
                        if (empProj != null)
                        {
                            m_listeActions.Add(new CTableSuppression(empProj));
                            MessageBox.Show("Employ� sur un projet supprim�");
                        }
                        else
                        {
                            var resultat = RegardeSiExisteDansListeActions(TableActuelle, idEmploye,idProj);
                            if (resultat != null && resultat.GetTypeAction() != CTableAction.TypeAction.Suppression)
                                m_listeActions.Add(new CTableSuppression(resultat.GetLigne()));
                            else
                                MessageBox.Show("Employ� sur un projet non trouv�", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                }
            }
        }
        private void SupprimerCelluleSupprimee()
        {
            using (var contexte = new ApplicationDBContext())

            {
                switch (TableActuelle)
                {
                    case Table.Departments:
                        m_listeActions.Add(new CTableSuppression((Departments)dataGridViewTableSGBD.CurrentRow.DataBoundItem));
                        break;
                    case Table.Employees:
                        m_listeActions.Add(new CTableSuppression((Employees)dataGridViewTableSGBD.CurrentRow.DataBoundItem));
                        break;
                    case Table.Employee_projects:
                        m_listeActions.Add(new CTableSuppression((Employee_projects)dataGridViewTableSGBD.CurrentRow.DataBoundItem));
                        break;
                    case Table.Projects:
                        m_listeActions.Add(new CTableSuppression((Projects)dataGridViewTableSGBD.CurrentRow.DataBoundItem));
                        break;
                    default:
                        throw new Exception("Table inconnue");
                }
            }
        }
        public void SupprimerCellule(bool LigneSelectionnee = false)
        {
            if (!LigneSelectionnee)
            {
                SupprimerCelluleByInput();
            }
            else
            {
                SupprimerCelluleSupprimee();
            }
            MontreLignes(TableActuelle, m_nIndiceMinimumActuel, m_nNbElementsParPage);
        }
        #endregion

        #region InputGetters
        private string GetTexteEntre(string message, string title, string defaultValue, int stringsize, out bool quitter)
        {
            string result = "";
            bool correctInput = false;
            while (!correctInput)
            {
                result = Microsoft.VisualBasic.Interaction.InputBox(message, title, defaultValue);
                quitter = result.Length == 0;
                if (quitter)
                    return "";
                correctInput = result != null && result.Length <= stringsize && result.Length > 1;
                if (result == null || result.Length <= 1)
                    MessageBox.Show("Champ nul", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (result.Length > stringsize)
                    MessageBox.Show($"Champ trop long (max {stringsize} caract�res)", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            quitter = false;
            return result;
        }
        private DateTime GetDateEntree(string message, string title, string defaultValue, out bool quitter)
        {
            string date = Microsoft.VisualBasic.Interaction.InputBox(message, title, defaultValue);
            DateTime dateTime;
            while (DateTime.TryParse(date, out dateTime) == false)
            {
                date = Microsoft.VisualBasic.Interaction.InputBox(message, title, defaultValue);
                quitter = date.Length == 0;
                if (quitter)
                    return DateTime.Now;
            }
            quitter = false;
            return dateTime;
        }
        private int GetEntierEntree(string message, string title, string defaultValue, int MaxLength, out bool quitter)
        {
            string ToParse;
            int result = -1;
            bool correctInput = false;
            while (!correctInput)
            {
                ToParse = Microsoft.VisualBasic.Interaction.InputBox(message, title, defaultValue);
                quitter = ToParse.Length == 0;
                if (quitter)
                    return -1;
                correctInput = ToParse != null && ToParse.Length <= MaxLength && ToParse[0] != '-' && int.TryParse(ToParse, out result) == true;

                if (!correctInput)
                    MessageBox.Show("Veuillez entrer un nombre correct", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            quitter = false;
            return result;
        }

        private float GetFloatEntree(string message, string title, string defaultValue, int MaxLength, out bool quitter)
        {
            string ToParse;
            float result = -1;
            bool correctInput = false;
            while (!correctInput)
            {
                ToParse = Microsoft.VisualBasic.Interaction.InputBox(message, title, defaultValue);
                quitter = ToParse.Length == 0;
                if (quitter)
                    return -1;
                correctInput = ToParse != null && ToParse.Length <= MaxLength && ToParse[0] != '-' && float.TryParse(ToParse, out result) == true;

                if (!correctInput)
                    MessageBox.Show("Veuillez entrer un nombre correct", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            quitter = false;
            return result;
        }

        private optionSortie EssaiQuitterApp(string message)
        {

            switch (MessageBox.Show(message, "Sauvegarde", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    return optionSortie.Save;
                case DialogResult.No:
                    return optionSortie.NoSave;

                case DialogResult.Cancel:
                case DialogResult.Abort:
                    return optionSortie.Cancel;
                default:
                    return optionSortie.Error;

            }
        }
        #endregion
        enum optionSortie
        {
            Save,
            NoSave,
            Cancel,
            Error
        }
        public void MAJPages()
        {
            TexteNumPage.Text = $"{m_nIndiceMinimumActuel}-{m_nIndiceMaximumActuel} sur {m_nIndiceMaximum}";
        }

        private void textBoxBarreDeRecherche_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Permet de regarder dans la liste d'actions si la ligne recherch�e existe d�j� 
        /// </summary>
        /// <param name="table">Table actuelle</param>
        /// <param name="id1">Project id ou departement id ou employee_id</param>
        /// <param name="id2">Project id pour la table Employee_projects</param>
        /// <returns></returns>
        public CTableAction? RegardeSiExisteDansListeActions(Table table, int id1, int id2 = -1)
        {
            CTableAction? ret = null;
            List<CTableAction> resultat = new List<CTableAction>();
            switch (table)
            {
                case Table.Projects:
                    resultat = m_listeActions.FindAll((a) => (a.GetLigne() as Projects)?.project_id == id1);
                    break;
                case Table.Departments:
                    resultat = m_listeActions.FindAll((a) => (a.GetLigne() as Departments)?.department_id == id1);
                    break;
                case Table.Employees:
                    resultat = m_listeActions.FindAll((a) => (a.GetLigne() as Employees)?.employee_id == id1);
                    break;
                case Table.Employee_projects:
                    resultat = m_listeActions.FindAll((a) => (a.GetLigne() as Employee_projects)?.employee_id == id1 && (a.GetLigne() as Employee_projects)?.project_id == id2);
                    break;

            }
            if (resultat.Count == 0)
            {
                return null;
            }
            else
            {
                if (resultat.Count == 1)
                    ret = resultat[0];
                else
                {
                    foreach (CTableAction action in resultat)
                    {
                        if (ret == null || action.idAction > ret.idAction)
                        {
                            ret = action;
                        }
                    }
                }

            }
            return ret;
        }
    }

    /// <summary>
    /// Classe abstraite permettant de faire du polymorphisme.
    /// Les classes filles sont les tables de la base de donn�es
    /// </summary>
    public abstract class CTable()
    {

    }
}