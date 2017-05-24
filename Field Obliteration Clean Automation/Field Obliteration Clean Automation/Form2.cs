using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Field_Obliteration_Clean_Automation
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:f.dandrea.lopez@accenture.com?subject=FOCA App: Improvement Report");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://p.yusukekamiyamane.com/");
        }

        private void SpecialThanksLabel_Click(object sender, EventArgs e)
        {
            Form4 easterEgg = new Form4();
            easterEgg.ShowDialog();
        }
    }
}
