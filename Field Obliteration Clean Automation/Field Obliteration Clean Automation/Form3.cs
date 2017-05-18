using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Configuration;

namespace Field_Obliteration_Clean_Automation
{
    public partial class PreviewForm : Form
    {
        public PreviewForm()
        {
            InitializeComponent();
        }

        public void previewTextBoxLoad(object sender, EventArgs e, string match, string component, string path)
        {   
            string[] previewBody = System.IO.File.ReadAllLines(path);
            foreach(string line in previewBody)
            {
               PreviewTextBox.AppendText(line + "\n");
            }
            PreviewGroupBox.Text = component;
            PreviewTextBox.Select(PreviewTextBox.Text.IndexOf(match), match.Length);
            PreviewTextBox.SelectionFont = new Font(PreviewTextBox.Font, FontStyle.Bold);
            PreviewTextBox.SelectionColor = Color.Red;
            PreviewTextBox.ScrollToCaret();
            PreviewTextBox.Select(PreviewTextBox.Text.IndexOf(match) - 300, 0);
        }
    }
}
 