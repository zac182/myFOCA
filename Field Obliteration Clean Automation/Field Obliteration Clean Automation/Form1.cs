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

        private void button1_Click(object sender, EventArgs e)
        {
            i = 0;
            DialogResult result = folderBrowserDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK && folderBrowserDialog1.SelectedPath.Substring(folderBrowserDialog1.SelectedPath.Length - 3, 3) == "src") // Test result.
            {
                Cursor.Current = Cursors.WaitCursor;
                Task.Delay(2000);
                dataGridView1.Rows.Clear();
                //Create the XmlDocument.
                List<XmlDocument> docs = new List<XmlDocument>();
                string field = textBox1.Text;
                string component = textBox2.Text;
                DirectoryInfo root = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
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
                                        if (fileline.Contains(field + "</field>"))
                                        {
                                            string match = "Parcial Match";
                                            if (fileline.Contains(component + "." + field + "</field>"))
                                            {
                                                match = "Full Match";
                                            }
                                            dataGridView1.Rows.Add(false, file.Name, match, fileline, file.FullName);
                                            break;
                                        }
                                    }
                                }
                                else if (fileline.Contains("<Profile") || fileline.Contains("<PermissionSet"))
                                {
                                    while ((fileline = filestr.ReadLine()) != null)
                                    {
                                        if (fileline.Contains(field + "</field>"))
                                        {
                                            string match = "Parcial Match";
                                            if (fileline.Contains(component + "." + field + "</field>"))
                                            {
                                                match = "Full Match";
                                            }
                                            dataGridView1.Rows.Add(false, file.Name, match, fileline, file.FullName);
                                            break;
                                        }
                                    }
                                }
                                else if (fileline.Contains("<CustomObjectTranslation"))
                                {
                                    while ((fileline = filestr.ReadLine()) != null)
                                    {
                                        if (fileline.Contains(field + "</name>"))
                                        {
                                            string match = "Parcial Match";
                                            if (fileline.Contains(component + "." + field + "</name>"))
                                            {
                                                match = "Full Match";
                                            }
                                            dataGridView1.Rows.Add(false, file.Name, match, fileline, file.FullName);
                                            break;
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
                                                if (reportfileline.Contains(field + "</field>"))
                                                {
                                                    string match = "Parcial Match";
                                                    if (reportfileline.Contains(component + "." + field + "</field>"))
                                                    {
                                                        match = "Full Match";
                                                    }
                                                    dataGridView1.Rows.Add(false, reportfile.Name, match, reportfileline, reportfile.FullName);
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
                Cursor.Current = Cursors.Default;
            }
            else if (result == DialogResult.OK && folderBrowserDialog1.SelectedPath.Substring(folderBrowserDialog1.SelectedPath.Length - 3, 3) != "src")
            {
                MessageBox.Show("Debe seleccionar una carpeta src");
                button1_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog2.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                Cursor.Current = Cursors.WaitCursor;
                if (dataGridView1.Rows.Count != 0)
                {
                    string field = textBox1.Text;
                    string component = textBox2.Text;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        DataGridViewCheckBoxCell chkboxCell = row.Cells[0] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(chkboxCell.Value))
                        {
                            int i = 0;
                            bool bkr = false;
                            XmlDocument doc = new XmlDocument();
                            XmlWriter writer = null;
                            string pathCell = row.Cells[2].Value.ToString();
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
                                            XmlWriterSettings settings = new XmlWriterSettings {
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
                                    if (nField[0].InnerText.Contains("." +  field))
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
                                    if (nField[0].InnerText.Contains("." + field))
                                    {
                                        nodo.ParentNode.RemoveChild(nodo);
                                        int pFrom = pathCell.IndexOf("\\src\\");
                                        String pathCellfinal = folderBrowserDialog2.SelectedPath + pathCell.Substring(pFrom);
                                        String reportFolder = pathCell.Substring(pFrom).Replace(row.Cells[1].Value.ToString(), "");
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
                                    if (nField[0].InnerText.Contains("." + field))
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
                                    if (nField[0].InnerText == field)
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
                    MessageBox.Show("Task Completed!");
                }
            }

            /*static void WalkDirectoryTree(System.IO.DirectoryInfo root, List<eachrow> lista, string field,DataGridView dataGridView1)
            {
                System.IO.FileInfo[] files = null;
                System.IO.DirectoryInfo[] subDirs = null;

                try
                {
                    files = root.GetFiles("*.*");
                }

                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }

                if (files != null)
                {

                    foreach (System.IO.FileInfo fi in files)
                    {                    


                        // Now find all the subdirectories under this directory.
                        subDirs = root.GetDirectories();

                        foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                        {
                            // Resursive call for each subdirectory.
                            WalkDirectoryTree(dirInfo, lista, field, dataGridView1);
                        }
                    }
                }
            }*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgv in dataGridView1.Rows)
            {
                dgv.Cells[0].Value = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgv in dataGridView1.Rows)
            {
                dgv.Cells[0].Value = false;
            }
        }
    }
}