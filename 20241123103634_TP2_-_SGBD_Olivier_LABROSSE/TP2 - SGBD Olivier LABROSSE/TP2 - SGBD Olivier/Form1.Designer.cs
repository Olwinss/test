namespace TP2___SGBD_Olivier
{
    partial class Affichagetable
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonPagePrecedente = new Button();
            buttonPremierePage = new Button();
            TexteNumPage = new Label();
            buttonProchainePage = new Button();
            buttonDernierePage = new Button();
            buttonMoinsUn = new Button();
            buttonPlusUn = new Button();
            toolStripNavBar = new ToolStrip();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripButtonDepartement = new ToolStripButton();
            toolStripDropDownButtonAffichageUnDep = new ToolStripDropDownButton();
            testToolStripMenuItem = new ToolStripMenuItem();
            testToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripButtonEmployeeProject = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            toolStripButtonEmployees = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            toolStripButtonProjects = new ToolStripButton();
            buttonAjouterLigne = new Button();
            buttonSupprimerLigne = new Button();
            textBoxBarreDeRecherche = new TextBox();
            buttonRechercher = new Button();
            checkBoxLigneSelectionnee = new CheckBox();
            buttonModifierLigne = new Button();
            dataGridViewTableSGBD = new DataGridView();
            textBox1 = new TextBox();
            toolStripNavBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTableSGBD).BeginInit();
            SuspendLayout();
            // 
            // buttonPagePrecedente
            // 
            buttonPagePrecedente.Anchor = AnchorStyles.Bottom;
            buttonPagePrecedente.ForeColor = SystemColors.ControlText;
            buttonPagePrecedente.Location = new Point(787, 501);
            buttonPagePrecedente.Name = "buttonPagePrecedente";
            buttonPagePrecedente.Size = new Size(32, 23);
            buttonPagePrecedente.TabIndex = 0;
            buttonPagePrecedente.Text = "<";
            buttonPagePrecedente.UseVisualStyleBackColor = true;
            buttonPagePrecedente.Click += buttonPagePrecedente_Click;
            // 
            // buttonPremierePage
            // 
            buttonPremierePage.Anchor = AnchorStyles.Bottom;
            buttonPremierePage.ForeColor = SystemColors.ControlText;
            buttonPremierePage.Location = new Point(749, 501);
            buttonPremierePage.Name = "buttonPremierePage";
            buttonPremierePage.Size = new Size(32, 23);
            buttonPremierePage.TabIndex = 1;
            buttonPremierePage.Text = "<<";
            buttonPremierePage.UseVisualStyleBackColor = true;
            buttonPremierePage.Click += buttonPremierePage_Click;
            // 
            // TexteNumPage
            // 
            TexteNumPage.Anchor = AnchorStyles.Bottom;
            TexteNumPage.AutoSize = true;
            TexteNumPage.ForeColor = SystemColors.ControlText;
            TexteNumPage.Location = new Point(807, 527);
            TexteNumPage.Name = "TexteNumPage";
            TexteNumPage.Size = new Size(126, 15);
            TexteNumPage.TabIndex = 2;
            TexteNumPage.Text = "Données 1 à 10000 sur ";
            // 
            // buttonProchainePage
            // 
            buttonProchainePage.Anchor = AnchorStyles.Bottom;
            buttonProchainePage.ForeColor = SystemColors.ControlText;
            buttonProchainePage.Location = new Point(901, 501);
            buttonProchainePage.Name = "buttonProchainePage";
            buttonProchainePage.Size = new Size(32, 23);
            buttonProchainePage.TabIndex = 4;
            buttonProchainePage.Text = ">";
            buttonProchainePage.UseVisualStyleBackColor = true;
            buttonProchainePage.Click += buttonProchainePage_Click;
            // 
            // buttonDernierePage
            // 
            buttonDernierePage.Anchor = AnchorStyles.Bottom;
            buttonDernierePage.ForeColor = SystemColors.ControlText;
            buttonDernierePage.Location = new Point(939, 501);
            buttonDernierePage.Name = "buttonDernierePage";
            buttonDernierePage.Size = new Size(32, 23);
            buttonDernierePage.TabIndex = 3;
            buttonDernierePage.Text = ">>";
            buttonDernierePage.UseVisualStyleBackColor = true;
            buttonDernierePage.Click += buttonDernierePage_Click;
            // 
            // buttonMoinsUn
            // 
            buttonMoinsUn.Anchor = AnchorStyles.Bottom;
            buttonMoinsUn.ForeColor = SystemColors.ControlText;
            buttonMoinsUn.Location = new Point(825, 501);
            buttonMoinsUn.Name = "buttonMoinsUn";
            buttonMoinsUn.Size = new Size(32, 23);
            buttonMoinsUn.TabIndex = 6;
            buttonMoinsUn.Text = "-1";
            buttonMoinsUn.UseVisualStyleBackColor = true;
            buttonMoinsUn.Click += buttonMoinsUn_Click;
            // 
            // buttonPlusUn
            // 
            buttonPlusUn.Anchor = AnchorStyles.Bottom;
            buttonPlusUn.ForeColor = SystemColors.ControlText;
            buttonPlusUn.Location = new Point(863, 501);
            buttonPlusUn.Name = "buttonPlusUn";
            buttonPlusUn.Size = new Size(32, 23);
            buttonPlusUn.TabIndex = 7;
            buttonPlusUn.Text = "+1";
            buttonPlusUn.UseVisualStyleBackColor = true;
            buttonPlusUn.Click += buttonPlusUn_Click;
            // 
            // toolStripNavBar
            // 
            toolStripNavBar.Dock = DockStyle.Left;
            toolStripNavBar.Items.AddRange(new ToolStripItem[] { toolStripSeparator2, toolStripButtonDepartement, toolStripDropDownButtonAffichageUnDep, toolStripSeparator1, toolStripButtonEmployeeProject, toolStripSeparator3, toolStripButtonEmployees, toolStripSeparator4, toolStripButtonProjects });
            toolStripNavBar.Location = new Point(0, 0);
            toolStripNavBar.Name = "toolStripNavBar";
            toolStripNavBar.RenderMode = ToolStripRenderMode.System;
            toolStripNavBar.Size = new Size(171, 584);
            toolStripNavBar.TabIndex = 12;
            toolStripNavBar.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(168, 6);
            // 
            // toolStripButtonDepartement
            // 
            toolStripButtonDepartement.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonDepartement.ImageTransparentColor = Color.LightYellow;
            toolStripButtonDepartement.Name = "toolStripButtonDepartement";
            toolStripButtonDepartement.Size = new Size(168, 19);
            toolStripButtonDepartement.Text = "Departements";
            toolStripButtonDepartement.Click += toolStripButtonDepartement_Click;
            // 
            // toolStripDropDownButtonAffichageUnDep
            // 
            toolStripDropDownButtonAffichageUnDep.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButtonAffichageUnDep.DropDownItems.AddRange(new ToolStripItem[] { testToolStripMenuItem, testToolStripMenuItem1 });
            toolStripDropDownButtonAffichageUnDep.ImageScaling = ToolStripItemImageScaling.None;
            toolStripDropDownButtonAffichageUnDep.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButtonAffichageUnDep.Name = "toolStripDropDownButtonAffichageUnDep";
            toolStripDropDownButtonAffichageUnDep.Size = new Size(168, 19);
            toolStripDropDownButtonAffichageUnDep.Text = "Affichage d'un Département";
            toolStripDropDownButtonAffichageUnDep.Click += toolStripDropDownButtonAffichageUnDep_Click;
            // 
            // testToolStripMenuItem
            // 
            testToolStripMenuItem.Name = "testToolStripMenuItem";
            testToolStripMenuItem.Size = new Size(94, 22);
            testToolStripMenuItem.Text = "Test";
            testToolStripMenuItem.Click += testToolStripMenuItem_Click;
            // 
            // testToolStripMenuItem1
            // 
            testToolStripMenuItem1.Name = "testToolStripMenuItem1";
            testToolStripMenuItem1.Size = new Size(94, 22);
            testToolStripMenuItem1.Text = "Test";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(168, 6);
            // 
            // toolStripButtonEmployeeProject
            // 
            toolStripButtonEmployeeProject.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonEmployeeProject.ImageTransparentColor = Color.Magenta;
            toolStripButtonEmployeeProject.Name = "toolStripButtonEmployeeProject";
            toolStripButtonEmployeeProject.Size = new Size(168, 19);
            toolStripButtonEmployeeProject.Text = "Employés-projets";
            toolStripButtonEmployeeProject.Click += toolStripButtonEmployeeProject_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(168, 6);
            // 
            // toolStripButtonEmployees
            // 
            toolStripButtonEmployees.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonEmployees.ImageTransparentColor = Color.Magenta;
            toolStripButtonEmployees.Name = "toolStripButtonEmployees";
            toolStripButtonEmployees.Size = new Size(168, 19);
            toolStripButtonEmployees.Text = "Employés";
            toolStripButtonEmployees.Click += toolStripButtonEmployees_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(168, 6);
            // 
            // toolStripButtonProjects
            // 
            toolStripButtonProjects.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonProjects.ImageTransparentColor = Color.Magenta;
            toolStripButtonProjects.Name = "toolStripButtonProjects";
            toolStripButtonProjects.Size = new Size(168, 19);
            toolStripButtonProjects.Text = "Projets";
            toolStripButtonProjects.Click += toolStripButtonProjects_Click;
            // 
            // buttonAjouterLigne
            // 
            buttonAjouterLigne.Anchor = AnchorStyles.Bottom;
            buttonAjouterLigne.Location = new Point(973, 501);
            buttonAjouterLigne.Name = "buttonAjouterLigne";
            buttonAjouterLigne.Size = new Size(195, 23);
            buttonAjouterLigne.TabIndex = 13;
            buttonAjouterLigne.Text = "Ajouter une ligne";
            buttonAjouterLigne.UseVisualStyleBackColor = true;
            buttonAjouterLigne.Click += buttonAjouterLigne_Click;
            // 
            // buttonSupprimerLigne
            // 
            buttonSupprimerLigne.Anchor = AnchorStyles.Bottom;
            buttonSupprimerLigne.Location = new Point(973, 557);
            buttonSupprimerLigne.Name = "buttonSupprimerLigne";
            buttonSupprimerLigne.RightToLeft = RightToLeft.No;
            buttonSupprimerLigne.Size = new Size(195, 23);
            buttonSupprimerLigne.TabIndex = 14;
            buttonSupprimerLigne.Text = "Supprimer ligne";
            buttonSupprimerLigne.UseVisualStyleBackColor = true;
            buttonSupprimerLigne.Click += buttonSupprimerLigne_Click;
            // 
            // textBoxBarreDeRecherche
            // 
            textBoxBarreDeRecherche.Anchor = AnchorStyles.Bottom;
            textBoxBarreDeRecherche.Location = new Point(188, 501);
            textBoxBarreDeRecherche.Name = "textBoxBarreDeRecherche";
            textBoxBarreDeRecherche.PlaceholderText = "Entrer du texte pour rechercher";
            textBoxBarreDeRecherche.Size = new Size(345, 23);
            textBoxBarreDeRecherche.TabIndex = 15;
            textBoxBarreDeRecherche.TextChanged += textBoxBarreDeRecherche_TextChanged;
            // 
            // buttonRechercher
            // 
            buttonRechercher.Anchor = AnchorStyles.Bottom;
            buttonRechercher.Location = new Point(539, 501);
            buttonRechercher.Name = "buttonRechercher";
            buttonRechercher.Size = new Size(195, 23);
            buttonRechercher.TabIndex = 16;
            buttonRechercher.Text = "Rechercher";
            buttonRechercher.UseVisualStyleBackColor = true;
            buttonRechercher.Click += buttonRechercher_Click;
            // 
            // checkBoxLigneSelectionnee
            // 
            checkBoxLigneSelectionnee.Anchor = AnchorStyles.Bottom;
            checkBoxLigneSelectionnee.AutoSize = true;
            checkBoxLigneSelectionnee.Location = new Point(1185, 531);
            checkBoxLigneSelectionnee.Name = "checkBoxLigneSelectionnee";
            checkBoxLigneSelectionnee.Size = new Size(177, 19);
            checkBoxLigneSelectionnee.TabIndex = 17;
            checkBoxLigneSelectionnee.Text = "Agir sur la ligne sélectionnée";
            checkBoxLigneSelectionnee.UseVisualStyleBackColor = true;
            // 
            // buttonModifierLigne
            // 
            buttonModifierLigne.Anchor = AnchorStyles.Bottom;
            buttonModifierLigne.Location = new Point(973, 528);
            buttonModifierLigne.Name = "buttonModifierLigne";
            buttonModifierLigne.Size = new Size(195, 23);
            buttonModifierLigne.TabIndex = 18;
            buttonModifierLigne.Text = "Modifier une ligne";
            buttonModifierLigne.UseVisualStyleBackColor = true;
            buttonModifierLigne.Click += buttonModifierLigne_Click;
            // 
            // dataGridViewTableSGBD
            // 
            dataGridViewTableSGBD.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewTableSGBD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewTableSGBD.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTableSGBD.Location = new Point(188, 12);
            dataGridViewTableSGBD.Name = "dataGridViewTableSGBD";
            dataGridViewTableSGBD.ReadOnly = true;
            dataGridViewTableSGBD.Size = new Size(1174, 478);
            dataGridViewTableSGBD.TabIndex = 5;
            dataGridViewTableSGBD.CellContentClick += dataGridViewTableSGBD_CellContentClick;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(188, 12);
            textBox1.MinimumSize = new Size(300, 300);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(1174, 300);
            textBox1.TabIndex = 19;
            textBox1.Visible = false;
            // 
            // Affichagetable
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1377, 584);
            Controls.Add(textBox1);
            Controls.Add(buttonModifierLigne);
            Controls.Add(checkBoxLigneSelectionnee);
            Controls.Add(buttonRechercher);
            Controls.Add(textBoxBarreDeRecherche);
            Controls.Add(buttonSupprimerLigne);
            Controls.Add(buttonAjouterLigne);
            Controls.Add(toolStripNavBar);
            Controls.Add(buttonPlusUn);
            Controls.Add(buttonMoinsUn);
            Controls.Add(dataGridViewTableSGBD);
            Controls.Add(buttonProchainePage);
            Controls.Add(buttonDernierePage);
            Controls.Add(TexteNumPage);
            Controls.Add(buttonPremierePage);
            Controls.Add(buttonPagePrecedente);
            Name = "Affichagetable";
            Text = "Affichage table ";
            FormClosing += Affichagetable_FormClosing;
            Load += MenuTable_Load;
            toolStripNavBar.ResumeLayout(false);
            toolStripNavBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTableSGBD).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonPagePrecedente;
        private Button buttonPremierePage;
        private Label TexteNumPage;
        private Button buttonProchainePage;
        private Button buttonDernierePage;
        private Button buttonMoinsUn;
        private Button buttonPlusUn;
        private ToolStrip toolStripNavBar;
        private ToolStripButton toolStripButtonDepartement;
        private ToolStripButton toolStripButtonEmployeeProject;
        private ToolStripButton toolStripButtonEmployees;
        private ToolStripButton toolStripButtonProjects;
        private Button buttonAjouterLigne;
        private Button buttonSupprimerLigne;
        private ToolStripDropDownButton toolStripDropDownButtonAffichageUnDep;
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem testToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private TextBox textBoxBarreDeRecherche;
        private Button buttonRechercher;
        private CheckBox checkBoxLigneSelectionnee;
        private Button buttonModifierLigne;
        private DataGridView dataGridViewTableSGBD;
        private TextBox textBox1;
    }
}
