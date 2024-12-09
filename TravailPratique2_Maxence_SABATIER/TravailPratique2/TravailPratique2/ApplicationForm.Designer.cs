namespace TravailPratique2
{
    partial class ApplicationForm
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
            components = new System.ComponentModel.Container();
            listeRelations = new ListBox();
            labelRelations = new Label();
            listeTables = new ListBox();
            labelTables = new Label();
            GrilleDonnees = new DataGridView();
            btnPremiersElems = new Button();
            btnMoinsElem = new Button();
            btnMoinsUnElem = new Button();
            labelNbDonnees = new Label();
            btnPlusUnElem = new Button();
            btnPlusElem = new Button();
            btnDerniersElems = new Button();
            btnSauvegarder = new Button();
            labelModifications = new Label();
            GestionnaireAide = new ToolTip(components);
            btnAjouterDonnee = new Button();
            btnSupprimerDonnee = new Button();
            btnAnnulerSauvegarde = new Button();
            btnModifierDonnee = new Button();
            ((System.ComponentModel.ISupportInitialize)GrilleDonnees).BeginInit();
            SuspendLayout();
            // 
            // listeRelations
            // 
            listeRelations.FormattingEnabled = true;
            listeRelations.ItemHeight = 15;
            listeRelations.Location = new Point(12, 254);
            listeRelations.Name = "listeRelations";
            listeRelations.Size = new Size(120, 184);
            listeRelations.TabIndex = 1;
            listeRelations.SelectedIndexChanged += listeRelations_SelectedIndexChanged;
            // 
            // labelRelations
            // 
            labelRelations.AutoSize = true;
            labelRelations.Location = new Point(12, 236);
            labelRelations.Name = "labelRelations";
            labelRelations.Size = new Size(55, 15);
            labelRelations.TabIndex = 2;
            labelRelations.Text = "Relations";
            // 
            // listeTables
            // 
            listeTables.FormattingEnabled = true;
            listeTables.ItemHeight = 15;
            listeTables.Location = new Point(12, 27);
            listeTables.Name = "listeTables";
            listeTables.Size = new Size(120, 184);
            listeTables.TabIndex = 3;
            listeTables.SelectedIndexChanged += listeTables_SelectedIndexChanged;
            // 
            // labelTables
            // 
            labelTables.AutoSize = true;
            labelTables.Location = new Point(12, 9);
            labelTables.Name = "labelTables";
            labelTables.Size = new Size(39, 15);
            labelTables.TabIndex = 4;
            labelTables.Text = "Tables";
            // 
            // GrilleDonnees
            // 
            GrilleDonnees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GrilleDonnees.Location = new Point(155, 27);
            GrilleDonnees.Name = "GrilleDonnees";
            GrilleDonnees.Size = new Size(892, 371);
            GrilleDonnees.TabIndex = 5;
            // 
            // btnPremiersElems
            // 
            btnPremiersElems.Location = new Point(155, 404);
            btnPremiersElems.Name = "btnPremiersElems";
            btnPremiersElems.Size = new Size(34, 23);
            btnPremiersElems.TabIndex = 6;
            btnPremiersElems.Text = "<<";
            btnPremiersElems.UseVisualStyleBackColor = true;
            btnPremiersElems.Click += btnPremiersElems_Click;
            // 
            // btnMoinsElem
            // 
            btnMoinsElem.Location = new Point(195, 404);
            btnMoinsElem.Name = "btnMoinsElem";
            btnMoinsElem.Size = new Size(34, 23);
            btnMoinsElem.TabIndex = 7;
            btnMoinsElem.Text = "<";
            btnMoinsElem.UseVisualStyleBackColor = true;
            btnMoinsElem.Click += btnMoinsElem_Click;
            // 
            // btnMoinsUnElem
            // 
            btnMoinsUnElem.Location = new Point(235, 404);
            btnMoinsUnElem.Name = "btnMoinsUnElem";
            btnMoinsUnElem.Size = new Size(34, 23);
            btnMoinsUnElem.TabIndex = 8;
            btnMoinsUnElem.Text = "<1";
            btnMoinsUnElem.UseVisualStyleBackColor = true;
            btnMoinsUnElem.Click += btnMoinsUnElem_Click;
            // 
            // labelNbDonnees
            // 
            labelNbDonnees.AutoSize = true;
            labelNbDonnees.Location = new Point(275, 408);
            labelNbDonnees.Name = "labelNbDonnees";
            labelNbDonnees.RightToLeft = RightToLeft.No;
            labelNbDonnees.Size = new Size(54, 15);
            labelNbDonnees.TabIndex = 9;
            labelNbDonnees.Text = "0 - 10000";
            // 
            // btnPlusUnElem
            // 
            btnPlusUnElem.Location = new Point(383, 404);
            btnPlusUnElem.Name = "btnPlusUnElem";
            btnPlusUnElem.Size = new Size(34, 23);
            btnPlusUnElem.TabIndex = 10;
            btnPlusUnElem.Text = "1>";
            btnPlusUnElem.UseVisualStyleBackColor = true;
            btnPlusUnElem.Click += btnPlusUnElem_Click;
            // 
            // btnPlusElem
            // 
            btnPlusElem.Location = new Point(423, 404);
            btnPlusElem.Name = "btnPlusElem";
            btnPlusElem.Size = new Size(34, 23);
            btnPlusElem.TabIndex = 11;
            btnPlusElem.Text = ">";
            btnPlusElem.UseVisualStyleBackColor = true;
            btnPlusElem.Click += btnPlusElem_Click;
            // 
            // btnDerniersElems
            // 
            btnDerniersElems.Location = new Point(463, 404);
            btnDerniersElems.Name = "btnDerniersElems";
            btnDerniersElems.Size = new Size(34, 23);
            btnDerniersElems.TabIndex = 12;
            btnDerniersElems.Text = ">>";
            btnDerniersElems.UseVisualStyleBackColor = true;
            btnDerniersElems.Click += btnDerniersElems_Click;
            // 
            // btnSauvegarder
            // 
            btnSauvegarder.BackColor = SystemColors.Control;
            btnSauvegarder.Location = new Point(935, 435);
            btnSauvegarder.Name = "btnSauvegarder";
            btnSauvegarder.Size = new Size(112, 34);
            btnSauvegarder.TabIndex = 16;
            btnSauvegarder.Text = "Sauvegarder";
            btnSauvegarder.UseVisualStyleBackColor = false;
            btnSauvegarder.Click += btnSauvegarder_Click;
            // 
            // labelModifications
            // 
            labelModifications.AutoSize = true;
            labelModifications.Location = new Point(852, 406);
            labelModifications.Name = "labelModifications";
            labelModifications.Size = new Size(151, 15);
            labelModifications.TabIndex = 17;
            labelModifications.Text = "Modifications en attente : 0";
            // 
            // btnAjouterDonnee
            // 
            btnAjouterDonnee.Location = new Point(540, 404);
            btnAjouterDonnee.Name = "btnAjouterDonnee";
            btnAjouterDonnee.Size = new Size(74, 37);
            btnAjouterDonnee.TabIndex = 18;
            btnAjouterDonnee.Text = "Ajouter";
            btnAjouterDonnee.UseVisualStyleBackColor = true;
            btnAjouterDonnee.Click += btnAjouterDonnee_Click;
            // 
            // btnSupprimerDonnee
            // 
            btnSupprimerDonnee.Location = new Point(700, 404);
            btnSupprimerDonnee.Name = "btnSupprimerDonnee";
            btnSupprimerDonnee.Size = new Size(74, 37);
            btnSupprimerDonnee.TabIndex = 19;
            btnSupprimerDonnee.Text = "Supprimer";
            btnSupprimerDonnee.UseVisualStyleBackColor = true;
            btnSupprimerDonnee.Click += btnSupprimerDonnee_Click;
            // 
            // btnAnnulerSauvegarde
            // 
            btnAnnulerSauvegarde.BackColor = SystemColors.Control;
            btnAnnulerSauvegarde.Location = new Point(817, 435);
            btnAnnulerSauvegarde.Name = "btnAnnulerSauvegarde";
            btnAnnulerSauvegarde.Size = new Size(112, 34);
            btnAnnulerSauvegarde.TabIndex = 20;
            btnAnnulerSauvegarde.Text = "Annuler";
            btnAnnulerSauvegarde.UseVisualStyleBackColor = false;
            btnAnnulerSauvegarde.Click += btnAnnulerSauvegarde_Click;
            // 
            // btnModifierDonnee
            // 
            btnModifierDonnee.Location = new Point(620, 404);
            btnModifierDonnee.Name = "btnModifierDonnee";
            btnModifierDonnee.Size = new Size(74, 37);
            btnModifierDonnee.TabIndex = 21;
            btnModifierDonnee.Text = "Modifier";
            btnModifierDonnee.UseVisualStyleBackColor = true;
            btnModifierDonnee.Click += btnModifierDonnee_Click;
            // 
            // ApplicationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AliceBlue;
            ClientSize = new Size(1059, 475);
            Controls.Add(btnModifierDonnee);
            Controls.Add(btnAnnulerSauvegarde);
            Controls.Add(btnSupprimerDonnee);
            Controls.Add(btnAjouterDonnee);
            Controls.Add(labelModifications);
            Controls.Add(btnSauvegarder);
            Controls.Add(btnDerniersElems);
            Controls.Add(btnPlusElem);
            Controls.Add(btnPlusUnElem);
            Controls.Add(labelNbDonnees);
            Controls.Add(btnMoinsUnElem);
            Controls.Add(btnMoinsElem);
            Controls.Add(btnPremiersElems);
            Controls.Add(GrilleDonnees);
            Controls.Add(labelTables);
            Controls.Add(listeTables);
            Controls.Add(labelRelations);
            Controls.Add(listeRelations);
            Name = "ApplicationForm";
            Text = "Form1";
            FormClosing += ApplicationForm_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)GrilleDonnees).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listeRelations;
        private Label labelRelations;
        private ListBox listeTables;
        private Label labelTables;
        private DataGridView GrilleDonnees;
        private Button btnPremiersElems;
        private Button btnMoinsElem;
        private Button btnMoinsUnElem;
        private Label labelNbDonnees;
        private Button btnPlusUnElem;
        private Button btnPlusElem;
        private Button btnDerniersElems;
        private Button btnSauvegarder;
        private Label labelModifications;
        private ToolTip GestionnaireAide;
        private Button btnAjouterDonnee;
        private Button btnSupprimerDonnee;
        private Button btnAnnulerSauvegarde;
        private Button btnModifierDonnee;
    }
}
