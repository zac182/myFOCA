namespace Field_Obliteration_Clean_Automation
{
    partial class MainApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainApp));
            this.SearchButton = new System.Windows.Forms.Button();
            this.FieldTextBox = new System.Windows.Forms.TextBox();
            this.MainDGV = new System.Windows.Forms.DataGridView();
            this.Checkbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Component = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Match = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MatchedLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SaveFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.ObjectTextBox = new System.Windows.Forms.TextBox();
            this.FieldLabel = new System.Windows.Forms.Label();
            this.ObjectLabel = new System.Windows.Forms.Label();
            this.SelectAllButton = new System.Windows.Forms.Button();
            this.UnselectAllButton = new System.Windows.Forms.Button();
            this.PathGroupBox = new System.Windows.Forms.GroupBox();
            this.srcAsDestCheckBox = new System.Windows.Forms.CheckBox();
            this.LoadPathButton = new System.Windows.Forms.Button();
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.DGVCover = new System.Windows.Forms.PictureBox();
            this.DGVCoverImg = new System.Windows.Forms.PictureBox();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.TotalFieldsStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SelectedFieldsStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectMatchedButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MainDGV)).BeginInit();
            this.PathGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVCover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVCoverImg)).BeginInit();
            this.StatusBar.SuspendLayout();
            this.MenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.Enabled = false;
            this.SearchButton.Image = ((System.Drawing.Image)(resources.GetObject("SearchButton.Image")));
            this.SearchButton.Location = new System.Drawing.Point(720, 585);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(117, 54);
            this.SearchButton.TabIndex = 9;
            this.SearchButton.Text = "   Search     Field";
            this.SearchButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.searchButtonClick);
            // 
            // FieldTextBox
            // 
            this.FieldTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FieldTextBox.Location = new System.Drawing.Point(542, 34);
            this.FieldTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.FieldTextBox.Name = "FieldTextBox";
            this.FieldTextBox.Size = new System.Drawing.Size(356, 21);
            this.FieldTextBox.TabIndex = 2;
            this.FieldTextBox.TextChanged += new System.EventHandler(this.enableSeachButton);
            // 
            // MainDGV
            // 
            this.MainDGV.AllowUserToAddRows = false;
            this.MainDGV.AllowUserToDeleteRows = false;
            this.MainDGV.AllowUserToResizeColumns = false;
            this.MainDGV.AllowUserToResizeRows = false;
            this.MainDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.MainDGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.MainDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MainDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Checkbox,
            this.Component,
            this.Type,
            this.Match,
            this.MatchedLine,
            this.Path});
            this.MainDGV.Location = new System.Drawing.Point(14, 158);
            this.MainDGV.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MainDGV.Name = "MainDGV";
            this.MainDGV.RowHeadersVisible = false;
            this.MainDGV.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.MainDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MainDGV.Size = new System.Drawing.Size(948, 420);
            this.MainDGV.TabIndex = 7;
            this.MainDGV.TabStop = false;
            //this.MainDGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.previewDoubleClick);
            this.MainDGV.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.MainDGV.Click += new System.EventHandler(this.dataGridView1_Click_1);
            // 
            // Checkbox
            // 
            this.Checkbox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Checkbox.HeaderText = "Checkbox";
            this.Checkbox.Name = "Checkbox";
            this.Checkbox.Width = 68;
            // 
            // Component
            // 
            this.Component.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Component.HeaderText = "Component";
            this.Component.Name = "Component";
            this.Component.ReadOnly = true;
            this.Component.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Type.Width = 59;
            // 
            // Match
            // 
            this.Match.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Match.HeaderText = "Match";
            this.Match.Name = "Match";
            this.Match.ReadOnly = true;
            this.Match.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Match.Width = 68;
            // 
            // MatchedLine
            // 
            this.MatchedLine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.MatchedLine.HeaderText = "MatchedLine";
            this.MatchedLine.Name = "MatchedLine";
            this.MatchedLine.ReadOnly = true;
            this.MatchedLine.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Path
            // 
            this.Path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Path.HeaderText = "Path";
            this.Path.Name = "Path";
            this.Path.ReadOnly = true;
            this.Path.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // LoadFolderDialog
            // 
            this.LoadFolderDialog.Description = "Select the src folder to be used as source.";
            // 
            // SaveFolderDialog
            // 
            this.SaveFolderDialog.Description = "Select the root folder in which \n the src folder will be created or modified.";
            // 
            // ObjectTextBox
            // 
            this.ObjectTextBox.Location = new System.Drawing.Point(78, 34);
            this.ObjectTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ObjectTextBox.Name = "ObjectTextBox";
            this.ObjectTextBox.Size = new System.Drawing.Size(357, 21);
            this.ObjectTextBox.TabIndex = 1;
            this.ObjectTextBox.TextChanged += new System.EventHandler(this.enableSeachButton);
            // 
            // FieldLabel
            // 
            this.FieldLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FieldLabel.AutoSize = true;
            this.FieldLabel.Location = new System.Drawing.Point(498, 37);
            this.FieldLabel.Name = "FieldLabel";
            this.FieldLabel.Size = new System.Drawing.Size(38, 18);
            this.FieldLabel.TabIndex = 5;
            this.FieldLabel.Text = "Field:";
            // 
            // ObjectLabel
            // 
            this.ObjectLabel.AutoSize = true;
            this.ObjectLabel.Location = new System.Drawing.Point(21, 37);
            this.ObjectLabel.Name = "ObjectLabel";
            this.ObjectLabel.Size = new System.Drawing.Size(51, 18);
            this.ObjectLabel.TabIndex = 6;
            this.ObjectLabel.Text = "Object:";
            // 
            // SelectAllButton
            // 
            this.SelectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectAllButton.Enabled = false;
            this.SelectAllButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectAllButton.Image")));
            this.SelectAllButton.Location = new System.Drawing.Point(12, 585);
            this.SelectAllButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SelectAllButton.Name = "SelectAllButton";
            this.SelectAllButton.Size = new System.Drawing.Size(117, 54);
            this.SelectAllButton.TabIndex = 6;
            this.SelectAllButton.Text = " Select All";
            this.SelectAllButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.SelectAllButton.UseVisualStyleBackColor = true;
            this.SelectAllButton.Click += new System.EventHandler(this.selectAllButtonClick);
            // 
            // UnselectAllButton
            // 
            this.UnselectAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UnselectAllButton.Enabled = false;
            this.UnselectAllButton.Image = ((System.Drawing.Image)(resources.GetObject("UnselectAllButton.Image")));
            this.UnselectAllButton.Location = new System.Drawing.Point(258, 586);
            this.UnselectAllButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.UnselectAllButton.Name = "UnselectAllButton";
            this.UnselectAllButton.Size = new System.Drawing.Size(117, 54);
            this.UnselectAllButton.TabIndex = 8;
            this.UnselectAllButton.Text = " Unselect All";
            this.UnselectAllButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.UnselectAllButton.UseVisualStyleBackColor = true;
            this.UnselectAllButton.Click += new System.EventHandler(this.unselectAllButtonClick);
            // 
            // PathGroupBox
            // 
            this.PathGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PathGroupBox.Controls.Add(this.srcAsDestCheckBox);
            this.PathGroupBox.Controls.Add(this.LoadPathButton);
            this.PathGroupBox.Controls.Add(this.PathTextBox);
            this.PathGroupBox.Location = new System.Drawing.Point(17, 73);
            this.PathGroupBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PathGroupBox.Name = "PathGroupBox";
            this.PathGroupBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PathGroupBox.Size = new System.Drawing.Size(944, 76);
            this.PathGroupBox.TabIndex = 8;
            this.PathGroupBox.TabStop = false;
            this.PathGroupBox.Text = "Path";
            // 
            // srcAsDestCheckBox
            // 
            this.srcAsDestCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.srcAsDestCheckBox.AutoSize = true;
            this.srcAsDestCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.srcAsDestCheckBox.Location = new System.Drawing.Point(743, 30);
            this.srcAsDestCheckBox.Name = "srcAsDestCheckBox";
            this.srcAsDestCheckBox.Size = new System.Drawing.Size(195, 22);
            this.srcAsDestCheckBox.TabIndex = 5;
            this.srcAsDestCheckBox.Text = "Use this as Destination Folder?";
            this.srcAsDestCheckBox.UseVisualStyleBackColor = true;
            // 
            // LoadPathButton
            // 
            this.LoadPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadPathButton.Image = ((System.Drawing.Image)(resources.GetObject("LoadPathButton.Image")));
            this.LoadPathButton.Location = new System.Drawing.Point(708, 22);
            this.LoadPathButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LoadPathButton.Name = "LoadPathButton";
            this.LoadPathButton.Size = new System.Drawing.Size(29, 35);
            this.LoadPathButton.TabIndex = 4;
            this.LoadPathButton.UseVisualStyleBackColor = true;
            this.LoadPathButton.Click += new System.EventHandler(this.loadPathButtonClick);
            // 
            // PathTextBox
            // 
            this.PathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PathTextBox.Location = new System.Drawing.Point(7, 28);
            this.PathTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.ReadOnly = true;
            this.PathTextBox.Size = new System.Drawing.Size(695, 21);
            this.PathTextBox.TabIndex = 3;
            this.PathTextBox.TextChanged += new System.EventHandler(this.enableSeachButton);
            // 
            // DGVCover
            // 
            this.DGVCover.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVCover.ErrorImage = null;
            this.DGVCover.InitialImage = null;
            this.DGVCover.Location = new System.Drawing.Point(14, 158);
            this.DGVCover.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DGVCover.Name = "DGVCover";
            this.DGVCover.Size = new System.Drawing.Size(948, 420);
            this.DGVCover.TabIndex = 9;
            this.DGVCover.TabStop = false;
            // 
            // DGVCoverImg
            // 
            this.DGVCoverImg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVCoverImg.Image = ((System.Drawing.Image)(resources.GetObject("DGVCoverImg.Image")));
            this.DGVCoverImg.InitialImage = ((System.Drawing.Image)(resources.GetObject("DGVCoverImg.InitialImage")));
            this.DGVCoverImg.Location = new System.Drawing.Point(14, 158);
            this.DGVCoverImg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DGVCoverImg.Name = "DGVCoverImg";
            this.DGVCoverImg.Size = new System.Drawing.Size(948, 419);
            this.DGVCoverImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.DGVCoverImg.TabIndex = 10;
            this.DGVCoverImg.TabStop = false;
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TotalFieldsStatusLabel,
            this.SelectedFieldsStatusLabel});
            this.StatusBar.Location = new System.Drawing.Point(0, 643);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.StatusBar.Size = new System.Drawing.Size(978, 22);
            this.StatusBar.TabIndex = 12;
            this.StatusBar.Text = "statusStrip1";
            // 
            // TotalFieldsStatusLabel
            // 
            this.TotalFieldsStatusLabel.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalFieldsStatusLabel.Name = "TotalFieldsStatusLabel";
            this.TotalFieldsStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // SelectedFieldsStatusLabel
            // 
            this.SelectedFieldsStatusLabel.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectedFieldsStatusLabel.Name = "SelectedFieldsStatusLabel";
            this.SelectedFieldsStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.MenuStrip.Size = new System.Drawing.Size(978, 25);
            this.MenuStrip.TabIndex = 13;
            this.MenuStrip.TabStop = true;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 19);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // SelectMatchedButton
            // 
            this.SelectMatchedButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SelectMatchedButton.Enabled = false;
            this.SelectMatchedButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectMatchedButton.Image")));
            this.SelectMatchedButton.Location = new System.Drawing.Point(135, 586);
            this.SelectMatchedButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SelectMatchedButton.Name = "SelectMatchedButton";
            this.SelectMatchedButton.Size = new System.Drawing.Size(117, 54);
            this.SelectMatchedButton.TabIndex = 7;
            this.SelectMatchedButton.Text = " Select Full Matched";
            this.SelectMatchedButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.SelectMatchedButton.UseVisualStyleBackColor = true;
            this.SelectMatchedButton.Click += new System.EventHandler(this.selectMatchedButtonClick);
            // 
            // MainApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 665);
            this.Controls.Add(this.SelectMatchedButton);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.MenuStrip);
            this.Controls.Add(this.DGVCoverImg);
            this.Controls.Add(this.PathGroupBox);
            this.Controls.Add(this.UnselectAllButton);
            this.Controls.Add(this.SelectAllButton);
            this.Controls.Add(this.ObjectLabel);
            this.Controls.Add(this.FieldLabel);
            this.Controls.Add(this.ObjectTextBox);
            this.Controls.Add(this.FieldTextBox);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.DGVCover);
            this.Controls.Add(this.MainDGV);
            this.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainApp";
            this.Text = "F.O.C.A.";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.mainApp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MainDGV)).EndInit();
            this.PathGroupBox.ResumeLayout(false);
            this.PathGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVCover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DGVCoverImg)).EndInit();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox FieldTextBox;
        private System.Windows.Forms.DataGridView MainDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Component;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Match;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatchedLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn Path;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Checkbox;
        private System.Windows.Forms.FolderBrowserDialog LoadFolderDialog;
        private System.Windows.Forms.FolderBrowserDialog SaveFolderDialog;
        private System.Windows.Forms.TextBox ObjectTextBox;
        private System.Windows.Forms.Label FieldLabel;
        private System.Windows.Forms.Label ObjectLabel;
        private System.Windows.Forms.Button SelectAllButton;
        private System.Windows.Forms.Button UnselectAllButton;
        private System.Windows.Forms.GroupBox PathGroupBox;
        private System.Windows.Forms.Button LoadPathButton;
        private System.Windows.Forms.TextBox PathTextBox;
        private System.Windows.Forms.PictureBox DGVCover;
        private System.Windows.Forms.PictureBox DGVCoverImg;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripStatusLabel TotalFieldsStatusLabel;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button SelectMatchedButton;
        private System.Windows.Forms.ToolStripStatusLabel SelectedFieldsStatusLabel;
        private System.Windows.Forms.CheckBox srcAsDestCheckBox;
    }
}

