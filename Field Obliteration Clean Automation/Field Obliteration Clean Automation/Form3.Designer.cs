namespace Field_Obliteration_Clean_Automation
{
    partial class PreviewForm
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
            this.PreviewTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // PreviewTextBox
            // 
            this.PreviewTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.PreviewTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviewTextBox.Location = new System.Drawing.Point(0, 0);
            this.PreviewTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PreviewTextBox.Name = "PreviewTextBox";
            this.PreviewTextBox.ReadOnly = true;
            this.PreviewTextBox.Size = new System.Drawing.Size(1015, 539);
            this.PreviewTextBox.TabIndex = 1;
            this.PreviewTextBox.Text = "";
            // 
            // PreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 539);
            this.Controls.Add(this.PreviewTextBox);
            this.Font = new System.Drawing.Font("Trebuchet MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreviewForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preview";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox PreviewTextBox;
    }
}