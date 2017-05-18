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
    //RC 1.0.20170314
    public partial class Form1 : Form
    {
        public static int i = 0;
        public class eachrow
        {
            public bool CheckB = false;
            public string Name = null;
            public string Dir = null;
            public XmlDocument doc = null;
        };
        public Form1()
        {
            InitializeComponent();
        }
        public static int j = 0;
        private void enableSeachButton(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "" && textBox2.Text.Trim() != "" && textBox3.Text.Trim() != "")
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Length != 0 && textBox1.Text.Trim().Substring(textBox1.Text.Trim().Length - 3, 3) != "__c")
            {
                if (textBox2.Text.Trim().Length == 0)
                {
                    MessageBox.Show("The field to be deleted must be a Custom Field\r\nMust define object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("The field to be deleted must be a Custom Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (textBox1.Text.Trim().Length == 0)
            {
                if (textBox2.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Must define field to be deleted\r\nMust define object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Must define field to be deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (textBox2.Text.Trim().Length == 0)
            {
                MessageBox.Show("Must define object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                i = 0;
                dataGridView1.Rows.Clear();
                Cursor.Current = Cursors.WaitCursor;
                Task.Delay(2000);
                List<XmlDocument> docs = new List<XmlDocument>();
                string field = textBox1.Text.Trim();
                string component = textBox2.Text.Trim();
                DirectoryInfo root = new DirectoryInfo(textBox3.Text);
                DirectoryInfo[] dirs = root.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    if (dir.Name != "reports")
                    {
                        FileInfo[] files = dir.GetFiles();
                        foreach (FileInfo file in files)
                        {
                            StreamReader filestr = file.OpenText();
                            string fileline;
                            while ((fileline = filestr.ReadLine()) != null)
                            {
                                if (fileline.Contains("<ReportType"))
                                {
                                    while ((fileline = filestr.ReadLine()) != null)
                                    {
                                        if (fileline.Contains("." + field + "</field>") || fileline.Contains(">" + field + "</field>") || fileline.Contains("$" + field + "</field>"))
                                        {
                                            string match = "Partial";
                                            if (fileline.Contains("<field>" + component + "." + field + "</field>"))
                                            {
                                                match = "Full";
                                            }
                                            string fileName = file.Name.Replace(".reportType", "");
                                            dataGridView1.Rows.Add(false, fileName, "reportType", match, fileline.TrimStart(' ').TrimEnd(' '), file.FullName);
                                            break;
                                        }
                                    }
                                }
                                else if (fileline.Contains("<PermissionSet"))
                                {
                                    while ((fileline = filestr.ReadLine()) != null)
                                    {

                                        if (fileline.Contains("<field>" + component + "." + field + "</field>"))
                                        {
                                            string match = "Full";

                                            string fileName = file.Name.Replace(".permissionset", "");
                                            dataGridView1.Rows.Add(false, fileName, "permissionset", match, fileline.TrimStart(' ').TrimEnd(' '), file.FullName);
                                            break;
                                        }
                                    }
                                }
                                else if (fileline.Contains("<Profile"))
                                {
                                    while ((fileline = filestr.ReadLine()) != null)
                                    {

                                        if (fileline.Contains("<field>" + component + "." + field + "</field>"))
                                        {
                                            string match;
                                            match = "Full";
                                            string fileName = file.Name.Replace(".profile", "");
                                            dataGridView1.Rows.Add(false, fileName, "profile", match, fileline.TrimStart(' ').TrimEnd(' '), file.FullName);
                                            break;


                                        }
                                    }
                                }
                                else if (fileline.Contains("<CustomObjectTranslation"))
                                {
                                    int pathint = file.FullName.IndexOf("\\objectTranslations\\") + 20;
                                    if (file.FullName.Substring(pathint) == component + "-en_US.objectTranslation")
                                        {
                                        while ((fileline = filestr.ReadLine()) != null)
                                        {
                                            if (fileline.Contains("<name>" + field + "</name>"))
                                            {
                                                string match = "Full";
                                                string fileName = file.Name.Replace(".objectTranslation", "");
                                                dataGridView1.Rows.Add(false, fileName, "objectTranslation", match, fileline.TrimStart(' ').TrimEnd(' '), file.FullName);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        DirectoryInfo reportroot = new DirectoryInfo(dir.FullName);
                        DirectoryInfo[] reportdirs = reportroot.GetDirectories();
                        foreach (DirectoryInfo reportdir in reportdirs)
                        {
                            if (reportdir.Name != "unfiled$public")
                            {
                                FileInfo[] reportfiles = reportdir.GetFiles();
                                foreach (FileInfo reportfile in reportfiles)
                                {
                                    StreamReader reportfilestr = reportfile.OpenText();
                                    string reportfileline;
                                    while ((reportfileline = reportfilestr.ReadLine()) != null)
                                    {
                                        if (reportfileline.Contains("<Report"))
                                        {
                                            while ((reportfileline = reportfilestr.ReadLine()) != null)
                                            {
                                                if (reportfileline.Contains("." + field + "</field>") || reportfileline.Contains(">" + field + "</field>") || reportfileline.Contains("$" + field + "</field>"))
                                                    {
                                                    string match = "Partial";
                                                    if (reportfileline.Contains("<field>" + component + "." + field + "</field>"))
                                                    {
                                                        match = "Full";
                                                    }
                                                    string reportfileName = reportfile.Name.Replace(".report", "");
                                                    dataGridView1.Rows.Add(false, reportfileName, "report", match, reportfileline.TrimStart(' ').TrimEnd(' '), reportfile.FullName);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (dataGridView1.RowCount != 0 && textBox1.TextLength != 0)
                {
                    toolStripStatusLabel1.Text = "Total files found: " + dataGridView1.RowCount;
                    toolStripStatusLabel2.Text = "Selected files: 0";
                    pictureBox1.Hide();
                    pictureBox2.Hide();
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button6.Enabled = true;
                    resizeColumns();
                }
                else
                {
                    toolStripStatusLabel1.Text = "Total files found: 0";
                    pictureBox1.Show();
                    pictureBox2.Show();
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    button6.Enabled = false;
                    MessageBox.Show("No references were found from the desired field in the selected folder.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (Directory.Exists(folderBrowserDialog2.SelectedPath))
                {
                    DialogResult result2 = MessageBox.Show("This process will overwrite the checked files in the path selected.\r\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result2 == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        if (dataGridView1.Rows.Count != 0)
                        {
                            bool chkboxtrue = false;
                            string field = textBox1.Text.Trim();
                            string component = textBox2.Text.Trim();
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                DataGridViewCheckBoxCell chkboxCell = row.Cells[0] as DataGridViewCheckBoxCell;
                                if (Convert.ToBoolean(chkboxCell.Value))
                                {
                                    chkboxtrue = true;
                                    int i = 0;
                                    bool bkr = false;
                                    XmlDocument doc = new XmlDocument();
                                    XmlWriter writer = null;
                                    string pathCell = row.Cells[5].Value.ToString();
                                    doc.Load(pathCell);
                                    if (pathCell.Contains("\\reportTypes\\"))
                                    {
                                        XmlNodeList NodeLista = doc.GetElementsByTagName("ReportType");
                                        XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("sections");
                                        for (int lis = 0; lis < NodeLista2.Count && bkr == false; lis++)
                                        {
                                            XmlNodeList NodeLista3 = null;
                                            NodeLista3 = ((XmlElement)NodeLista2[i]).GetElementsByTagName("columns");
                                            foreach (XmlElement nodo in NodeLista3)
                                            {
                                                XmlNodeList nField = nodo.GetElementsByTagName("field");
                                                if (nField[0].InnerText.Contains(field))
                                                {
                                                    nodo.ParentNode.RemoveChild(nodo);
                                                    int pFrom = pathCell.IndexOf("\\src\\");
                                                    String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                                    System.IO.Directory.CreateDirectory(folderBrowserDialog2.SelectedPath + "\\src\\reportTypes");
                                                    XmlWriterSettings settings = new XmlWriterSettings
                                                    {
                                                        Encoding = Encoding.UTF8,
                                                        Indent = true,
                                                        IndentChars = "    ",
                                                        NewLineChars = "\r\n",
                                                        NewLineHandling = NewLineHandling.Replace,
                                                        CloseOutput = true
                                                    };
                                                    writer = XmlWriter.Create(pathCellfinal, settings);
                                                    doc.Save(writer);
                                                    writer.Close();
                                                    string xmlString = System.IO.File.ReadAllText(pathCellfinal);
                                                    string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                    string xmlpart2 = xmlString.Substring(38);
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                    {
                                                        sw.Write(xmlpart1 + xmlpart2);
                                                        sw.WriteLine("");
                                                    }
                                                    bkr = true;
                                                    break;
                                                }
                                            }
                                            i++;
                                        }
                                    }
                                    else if (pathCell.Contains("\\permissionsets\\"))
                                    {
                                        XmlNodeList NodeLista = doc.GetElementsByTagName("PermissionSet");
                                        XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                                        foreach (XmlElement nodo in NodeLista2)
                                        {
                                            XmlNodeList nField = nodo.GetElementsByTagName("field");
                                            if (nField[0].InnerText.Contains(field))
                                            {
                                                nodo.ParentNode.RemoveChild(nodo);
                                                int pFrom = pathCell.IndexOf("\\src\\");
                                                String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                                System.IO.Directory.CreateDirectory(folderBrowserDialog2.SelectedPath + "\\src\\permissionsets");
                                                XmlWriterSettings settings = new XmlWriterSettings
                                                {
                                                    Encoding = Encoding.UTF8,
                                                    Indent = true,
                                                    IndentChars = "    ",
                                                    NewLineChars = "\r\n",
                                                    NewLineHandling = NewLineHandling.Replace,
                                                    CloseOutput = true
                                                };
                                                writer = XmlWriter.Create(pathCellfinal, settings);
                                                doc.Save(writer);
                                                writer.Close();
                                                string xmlString = System.IO.File.ReadAllText(pathCellfinal);
                                                string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                string xmlpart2 = xmlString.Substring(38);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                {
                                                    sw.Write(xmlpart1 + xmlpart2);
                                                    sw.WriteLine("");
                                                }
                                                bkr = true;
                                                break;
                                            }
                                            i++;
                                        }
                                    }
                                    else if (pathCell.Contains("\\reports\\"))
                                    {
                                        XmlNodeList NodeLista = doc.GetElementsByTagName("Report");
                                        XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("columns");
                                        foreach (XmlElement nodo in NodeLista2)
                                        {
                                            XmlNodeList nField = nodo.GetElementsByTagName("field");
                                            if (nField[0].InnerText.Contains(field))
                                            {
                                                nodo.ParentNode.RemoveChild(nodo);
                                                int pFrom = pathCell.IndexOf("\\src\\");
                                                String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                                String reportFolder = pathCell.Substring(pFrom).Replace(row.Cells[1].Value.ToString() + "." + row.Cells[2].Value.ToString(), "");
                                                System.IO.Directory.CreateDirectory(folderBrowserDialog2.SelectedPath + reportFolder);
                                                XmlWriterSettings settings = new XmlWriterSettings
                                                {
                                                    Encoding = Encoding.UTF8,
                                                    Indent = true,
                                                    IndentChars = "    ",
                                                    NewLineChars = "\r\n",
                                                    NewLineHandling = NewLineHandling.Replace,
                                                    CloseOutput = true
                                                };
                                                writer = XmlWriter.Create(pathCellfinal, settings);
                                                doc.Save(writer);
                                                writer.Close();
                                                string xmlString = System.IO.File.ReadAllText(pathCellfinal);
                                                string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                string xmlpart2 = xmlString.Substring(38);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                {
                                                    sw.Write(xmlpart1 + xmlpart2);
                                                    sw.WriteLine("");
                                                }
                                                bkr = true;
                                                break;
                                            }
                                            i++;
                                        }
                                    }
                                    else if (pathCell.Contains("\\profiles\\"))
                                    {
                                        XmlNodeList NodeLista = doc.GetElementsByTagName("Profile");
                                        XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                                        foreach (XmlElement nodo in NodeLista2)
                                        {
                                            XmlNodeList nField = nodo.GetElementsByTagName("field");
                                            if (nField[0].InnerText.Contains(field))
                                            {
                                                nodo.ParentNode.RemoveChild(nodo);
                                                int pFrom = pathCell.IndexOf("\\src\\");
                                                String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                                System.IO.Directory.CreateDirectory(folderBrowserDialog2.SelectedPath + "\\src\\profiles");
                                                XmlWriterSettings settings = new XmlWriterSettings
                                                {
                                                    Encoding = Encoding.UTF8,
                                                    Indent = true,
                                                    IndentChars = "    ",
                                                    NewLineChars = "\r\n",
                                                    NewLineHandling = NewLineHandling.Replace,
                                                    CloseOutput = true
                                                };
                                                writer = XmlWriter.Create(pathCellfinal, settings);
                                                doc.Save(writer);
                                                writer.Close();
                                                string xmlString = System.IO.File.ReadAllText(pathCellfinal);
                                                string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                string xmlpart2 = xmlString.Substring(38);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                {
                                                    sw.Write(xmlpart1 + xmlpart2);
                                                    sw.WriteLine("");
                                                }
                                                bkr = true;
                                                break;
                                            }
                                            i++;
                                        }
                                    }
                                    else if (pathCell.Contains("\\objectTranslations\\"))
                                    {
                                        XmlNodeList NodeLista = doc.GetElementsByTagName("CustomObjectTranslation");
                                        XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fields");
                                        foreach (XmlElement nodo in NodeLista2)
                                        {
                                            XmlNodeList nField = nodo.GetElementsByTagName("name");
                                            if (nField[0].InnerText.Contains(field))
                                            {
                                                nodo.ParentNode.RemoveChild(nodo);
                                                int pFrom = pathCell.IndexOf("\\src\\");
                                                String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                                System.IO.Directory.CreateDirectory(folderBrowserDialog2.SelectedPath + "\\src\\objectTranslations");
                                                XmlWriterSettings settings = new XmlWriterSettings
                                                {
                                                    Encoding = Encoding.UTF8,
                                                    Indent = true,
                                                    IndentChars = "    ",
                                                    NewLineChars = "\r\n",
                                                    NewLineHandling = NewLineHandling.Replace,
                                                    CloseOutput = true
                                                };
                                                writer = XmlWriter.Create(pathCellfinal, settings);
                                                doc.Save(writer);
                                                writer.Close();
                                                string xmlString = System.IO.File.ReadAllText(pathCellfinal);
                                                string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                string xmlpart2 = xmlString.Substring(38);
                                                string removable1 = ">\r\n            <!--";
                                                string removable2 = "-->\r\n        </";
                                                string removable3 = ">\r\n                <!--";
                                                string removable4 = "-->\r\n            </";
                                                string removable5 = "\'";
                                                while (xmlpart2.Contains(removable1))
                                                {
                                                    xmlpart2 = xmlpart2.Replace(removable1, "><!--");
                                                }
                                                while (xmlpart2.Contains(removable2))
                                                {
                                                    xmlpart2 = xmlpart2.Replace(removable2, "--></");
                                                }
                                                while (xmlpart2.Contains(removable3))
                                                {
                                                    xmlpart2 = xmlpart2.Replace(removable3, "><!--");
                                                }
                                                while (xmlpart2.Contains(removable4))
                                                {
                                                    xmlpart2 = xmlpart2.Replace(removable4, "--></");
                                                }
                                                while (xmlpart2.Contains(removable5))
                                                {
                                                    xmlpart2 = xmlpart2.Replace(removable5, "&apos;");
                                                }
                                                using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                {
                                                    sw.Write(xmlpart1 + xmlpart2);
                                                    sw.WriteLine("");
                                                }
                                                bkr = true;
                                                break;
                                            }
                                            i++;
                                        }
                                    }
                                }
                            }
                            Cursor.Current = Cursors.Default;
                            if (chkboxtrue)
                            {
                                MessageBox.Show("Task Completed!");
                            }
                            else
                            {
                                MessageBox.Show("There was no row checked", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                else
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (dataGridView1.Rows.Count != 0)
                    {
                        bool chkboxtrue = false;
                        string field = textBox1.Text.Trim();
                        string component = textBox2.Text.Trim();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            DataGridViewCheckBoxCell chkboxCell = row.Cells[0] as DataGridViewCheckBoxCell;
                            if (Convert.ToBoolean(chkboxCell.Value))
                            {
                                chkboxtrue = true;
                                int i = 0;
                                bool bkr = false;
                                XmlDocument doc = new XmlDocument();
                                XmlWriter writer = null;
                                string pathCell = row.Cells[5].Value.ToString();
                                doc.Load(pathCell);
                                if (pathCell.Contains("\\reportTypes\\"))
                                {
                                    XmlNodeList NodeLista = doc.GetElementsByTagName("ReportType");
                                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("sections");
                                    for (int lis = 0; lis < NodeLista2.Count && bkr == false; lis++)
                                    {
                                        XmlNodeList NodeLista3 = null;
                                        NodeLista3 = ((XmlElement)NodeLista2[i]).GetElementsByTagName("columns");
                                        foreach (XmlElement nodo in NodeLista3)
                                        {
                                            XmlNodeList nField = nodo.GetElementsByTagName("field");
                                            if (nField[0].InnerText.Contains(field))
                                            {
                                                nodo.ParentNode.RemoveChild(nodo);
                                                int pFrom = pathCell.IndexOf("\\src\\");
                                                String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                                System.IO.Directory.CreateDirectory(folderBrowserDialog2.SelectedPath + "\\src\\reportTypes");
                                                XmlWriterSettings settings = new XmlWriterSettings
                                                {
                                                    Encoding = Encoding.UTF8,
                                                    Indent = true,
                                                    IndentChars = "    ",
                                                    NewLineChars = "\r\n",
                                                    NewLineHandling = NewLineHandling.Replace,
                                                    CloseOutput = true
                                                };
                                                writer = XmlWriter.Create(pathCellfinal, settings);
                                                doc.Save(writer);
                                                writer.Close();
                                                string xmlString = System.IO.File.ReadAllText(pathCellfinal);
                                                string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                string xmlpart2 = xmlString.Substring(38);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                {
                                                    sw.Write(xmlpart1 + xmlpart2);
                                                    sw.WriteLine("");
                                                }
                                                bkr = true;
                                                break;
                                            }
                                        }
                                        i++;
                                    }
                                }
                                else if (pathCell.Contains("\\permissionsets\\"))
                                {
                                    XmlNodeList NodeLista = doc.GetElementsByTagName("PermissionSet");
                                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                                    foreach (XmlElement nodo in NodeLista2)
                                    {
                                        XmlNodeList nField = nodo.GetElementsByTagName("field");
                                        if (nField[0].InnerText.Contains(field))
                                        {
                                            nodo.ParentNode.RemoveChild(nodo);
                                            int pFrom = pathCell.IndexOf("\\src\\");
                                            String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                            System.IO.Directory.CreateDirectory(folderBrowserDialog2.SelectedPath + "\\src\\permissionsets");
                                            XmlWriterSettings settings = new XmlWriterSettings
                                            {
                                                Encoding = Encoding.UTF8,
                                                Indent = true,
                                                IndentChars = "    ",
                                                NewLineChars = "\r\n",
                                                NewLineHandling = NewLineHandling.Replace,
                                                CloseOutput = true
                                            };
                                            writer = XmlWriter.Create(pathCellfinal, settings);
                                            doc.Save(writer);
                                            writer.Close();
                                            string xmlString = System.IO.File.ReadAllText(pathCellfinal);
                                            string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                            string xmlpart2 = xmlString.Substring(38);
                                            using (StreamWriter sw = File.CreateText(pathCellfinal))
                                            {
                                                sw.Write(xmlpart1 + xmlpart2);
                                                sw.WriteLine("");
                                            }
                                            bkr = true;
                                            break;
                                        }
                                        i++;
                                    }
                                }
                                else if (pathCell.Contains("\\reports\\"))
                                {
                                    XmlNodeList NodeLista = doc.GetElementsByTagName("Report");
                                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("columns");
                                    foreach (XmlElement nodo in NodeLista2)
                                    {
                                        XmlNodeList nField = nodo.GetElementsByTagName("field");
                                        if (nField[0].InnerText.Contains(field))
                                        {
                                            nodo.ParentNode.RemoveChild(nodo);
                                            int pFrom = pathCell.IndexOf("\\src\\");
                                            String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                            String reportFolder = pathCell.Substring(pFrom).Replace(row.Cells[1].Value.ToString() + "." + row.Cells[2].Value.ToString(), "");
                                            System.IO.Directory.CreateDirectory(folderBrowserDialog2.SelectedPath + reportFolder);
                                            XmlWriterSettings settings = new XmlWriterSettings
                                            {
                                                Encoding = Encoding.UTF8,
                                                Indent = true,
                                                IndentChars = "    ",
                                                NewLineChars = "\r\n",
                                                NewLineHandling = NewLineHandling.Replace,
                                                CloseOutput = true
                                            };
                                            writer = XmlWriter.Create(pathCellfinal, settings);
                                            doc.Save(writer);
                                            writer.Close();
                                            string xmlString = System.IO.File.ReadAllText(pathCellfinal);
                                            string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                            string xmlpart2 = xmlString.Substring(38);
                                            using (StreamWriter sw = File.CreateText(pathCellfinal))
                                            {
                                                sw.Write(xmlpart1 + xmlpart2);
                                                sw.WriteLine("");
                                            }
                                            bkr = true;
                                            break;
                                        }
                                        i++;
                                    }
                                }
                                else if (pathCell.Contains("\\profiles\\"))
                                {
                                    XmlNodeList NodeLista = doc.GetElementsByTagName("Profile");
                                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                                    foreach (XmlElement nodo in NodeLista2)
                                    {
                                        XmlNodeList nField = nodo.GetElementsByTagName("field");
                                        if (nField[0].InnerText.Contains(field))
                                        {
                                            nodo.ParentNode.RemoveChild(nodo);
                                            int pFrom = pathCell.IndexOf("\\src\\");
                                            String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                            System.IO.Directory.CreateDirectory(folderBrowserDialog2.SelectedPath + "\\src\\profiles");
                                            XmlWriterSettings settings = new XmlWriterSettings
                                            {
                                                Encoding = Encoding.UTF8,
                                                Indent = true,
                                                IndentChars = "    ",
                                                NewLineChars = "\r\n",
                                                NewLineHandling = NewLineHandling.Replace,
                                                CloseOutput = true
                                            };
                                            writer = XmlWriter.Create(pathCellfinal, settings);
                                            doc.Save(writer);
                                            writer.Close();
                                            string xmlString = System.IO.File.ReadAllText(pathCellfinal);
                                            string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                            string xmlpart2 = xmlString.Substring(38);
                                            using (StreamWriter sw = File.CreateText(pathCellfinal))
                                            {
                                                sw.Write(xmlpart1 + xmlpart2);
                                                sw.WriteLine("");
                                            }
                                            bkr = true;
                                            break;
                                        }
                                        i++;
                                    }
                                }
                                else if (pathCell.Contains("\\objectTranslations\\"))
                                {
                                    XmlNodeList NodeLista = doc.GetElementsByTagName("CustomObjectTranslation");
                                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fields");
                                    foreach (XmlElement nodo in NodeLista2)
                                    {
                                        XmlNodeList nField = nodo.GetElementsByTagName("name");
                                        if (nField[0].InnerText.Contains(field))
                                        {
                                            nodo.ParentNode.RemoveChild(nodo);
                                            int pFrom = pathCell.IndexOf("\\src\\");
                                            String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                            System.IO.Directory.CreateDirectory(folderBrowserDialog2.SelectedPath + "\\src\\objectTranslations");
                                            XmlWriterSettings settings = new XmlWriterSettings
                                            {
                                                Encoding = Encoding.UTF8,
                                                Indent = true,
                                                IndentChars = "    ",
                                                NewLineChars = "\r\n",
                                                NewLineHandling = NewLineHandling.Replace,
                                                CloseOutput = true
                                            };
                                            writer = XmlWriter.Create(pathCellfinal, settings);
                                            doc.Save(writer);
                                            writer.Close();
                                            string xmlString = System.IO.File.ReadAllText(pathCellfinal);
                                            string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                            string xmlpart2 = xmlString.Substring(38);
                                            string removable1 = ">\r\n            <!--";
                                            string removable2 = "-->\r\n        </";
                                            string removable3 = ">\r\n                <!--";
                                            string removable4 = "-->\r\n            </";
                                            string removable5 = "\'";
                                            while (xmlpart2.Contains(removable1))
                                            {
                                                xmlpart2 = xmlpart2.Replace(removable1, "><!--");
                                            }
                                            while (xmlpart2.Contains(removable2))
                                            {
                                                xmlpart2 = xmlpart2.Replace(removable2, "--></");
                                            }
                                            while (xmlpart2.Contains(removable3))
                                            {
                                                xmlpart2 = xmlpart2.Replace(removable3, "><!--");
                                            }
                                            while (xmlpart2.Contains(removable4))
                                            {
                                                xmlpart2 = xmlpart2.Replace(removable4, "--></");
                                            }
                                            while (xmlpart2.Contains(removable5))
                                            {
                                                xmlpart2 = xmlpart2.Replace(removable5, "&apos;");
                                            }
                                            using (StreamWriter sw = File.CreateText(pathCellfinal))
                                            {
                                                sw.Write(xmlpart1 + xmlpart2);
                                                sw.WriteLine("");
                                            }
                                            bkr = true;
                                            break;
                                        }
                                        i++;
                                    }
                                }
                            }
                        }
                        Cursor.Current = Cursors.Default;
                        if (chkboxtrue)
                        {
                            MessageBox.Show("Task Completed!");
                        }
                        else
                        {
                            MessageBox.Show("There was no row checked", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow dgv in dataGridView1.Rows)
            {
                dgv.Cells[0].Value = true;
            }
            Cursor.Current = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow dgv in dataGridView1.Rows)
            {
                dgv.Cells[0].Value = false;
            }
            Cursor.Current = Cursors.Default;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK && folderBrowserDialog1.SelectedPath.Substring(folderBrowserDialog1.SelectedPath.Length - 3, 3) == "src") // Test result.
            {
                dataGridView1.Rows.Clear();
                textBox3.Text = folderBrowserDialog1.SelectedPath;
                button1.Enabled = true;
                Properties.Settings.Default.PathReminder = textBox3.Text;
                Properties.Settings.Default.Save();
            }
            else if (result == DialogResult.OK && folderBrowserDialog1.SelectedPath.Substring(folderBrowserDialog1.SelectedPath.Length - 3, 3) != "src")
            {
                MessageBox.Show("An \"src\" folder must be selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button5_Click(sender, e);
            }       
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 about = new Form2();
            about.ShowDialog();
            //MessageBox.Show("\t\tVersion 1.0\r\n\n\tDeveloped by D'Andrea López Fabrizio Carlo\r\n\n\tSpecial Thanks to MMS Ops Dev ADC Team\r\n\n\tFor bugs or improvements send an email to:\n\r\tf.dandrea.lopez@accenture.com");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow dgv in dataGridView1.Rows)
            {
                if (dgv.Cells[3].Value.ToString() == "Full")
                {
                    dgv.Cells[0].Value = true;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Text = Properties.Settings.Default.PathReminder;
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 10; // in miliseconds
            timer1.Start();
        }

        private void dataGridView1_Click_1(object sender, EventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private Timer timer1;

        private void timer1_Tick(object sender, EventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount != 0)
            {
                int alt = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        alt++;
                    }
                }
                toolStripStatusLabel2.Text = "Selected files: " + alt;
            }
        }
        private void resizeColumns()
        {
            Component.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            int widthcmp = Component.Width;
            Component.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            Component.Width = widthcmp;
            MatchedLine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            int widthml = MatchedLine.Width;
            MatchedLine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            MatchedLine.Width = widthml;
            Path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            int widthph = Path.Width;
            Path.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            Path.Width = widthph;
        }
    }

        /*private void findFiles()
{
while ((fileline = filestr.ReadLine()) != null)
{
if (fileline.Contains(field + "</field>"))
{
string match = "Partial";
if (fileline.Contains(component + "." + field + "</field>"))
{
match = "Full";
}
string fileName = file.Name.Replace(".reportType", "");
dataGridView1.Rows.Add(false, fileName, "reportType", match, fileline.TrimStart(' ').TrimEnd(' '), file.FullName);
break;
}
}
}*/
    
}
