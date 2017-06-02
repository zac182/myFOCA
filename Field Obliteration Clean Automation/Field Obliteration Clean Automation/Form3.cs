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
using System.Runtime.InteropServices;

namespace Field_Obliteration_Clean_Automation
{
    public partial class PreviewForm : Form
    {
        public PreviewForm()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", EntryPoint = "HideCaret")]
        public static extern long HideCaret(IntPtr hwnd);

        public void previewTextBoxLoad(object sender, EventArgs e, string match, string component, string path)
        {
            try
            {
                PreviewTextBox.Text = File.ReadAllText(path);
                PreviewTextBox.Text = PreviewTextBox.Text.Replace("\r", "");
                int matchIdx = PreviewTextBox.Text.IndexOf(match);
                PreviewTextBox.Select(matchIdx, match.Length);
                PreviewTextBox.SelectionFont = new Font(PreviewTextBox.Font, FontStyle.Bold);
                PreviewTextBox.SelectionColor = Color.Red;
                PreviewTextBox.ScrollToCaret();
                if (matchIdx >= 300)
                {
                    PreviewTextBox.Select(matchIdx - 300, 0);
                }
                HideCaret(PreviewTextBox.Handle);
            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
    }
}
 