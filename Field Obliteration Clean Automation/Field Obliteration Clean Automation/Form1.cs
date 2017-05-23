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
    public partial class MainApp : Form
    {
        public MainApp()
        {
            InitializeComponent();
        }
        public static int j = 0;

        private void enableSeachButton(object sender, EventArgs e)
        {
            if (FieldTextBox.Text.Trim() != "" && ObjectTextBox.Text.Trim() != "" && PathTextBox.Text.Trim() != "")
                SearchButton.Enabled = true;
            else
                SearchButton.Enabled = false;
        }

        private bool searchButtonValidation(object sender, EventArgs e)
        {
            if (FieldTextBox.Text.Trim().Length != 0 && FieldTextBox.Text.Trim().Substring(FieldTextBox.Text.Trim().Length - 3, 3) != "__c")
            {
                if (ObjectTextBox.Text.Trim().Length == 0)
                {
                    MessageBox.Show("The field to be deleted must be a Custom Field\r\nMust define object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show("The field to be deleted must be a Custom Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (FieldTextBox.Text.Trim().Length == 0)
            {
                if (ObjectTextBox.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Must define field to be deleted\r\nMust define object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show("Must define field to be deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (ObjectTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Must define object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void searchButtonProcess(object sender, EventArgs e, string fileline, string fieldLower, string componentLower, FileInfo file, StreamReader filestr, string type)
        {
            if (type == "reportType" || type == "report")
            {
                while ((fileline = filestr.ReadLine()) != null)
                {
                    if (fileline.ToLower().Contains("." + fieldLower + "</field>") || fileline.ToLower().Contains(">" + fieldLower + "</field>") || fileline.ToLower().Contains("$" + fieldLower + "</field>"))
                    {
                        string match = "Partial";
                        if (fileline.ToLower().Contains("<field>" + componentLower + "." + fieldLower + "</field>"))
                        {
                            match = "Full";
                        }
                        string fileName = file.Name.Replace("." + type, "");
                        MainDGV.Rows.Add(false, fileName, type, match, fileline.TrimStart(' ').TrimEnd(' '), file.FullName);
                        break;
                    }
                }
            }
            else if (type == "permissionset" || type == "profile")
            {
                while ((fileline = filestr.ReadLine()) != null)
                {

                    if (fileline.ToLower().Contains("<field>" + componentLower + "." + fieldLower + "</field>"))
                    {
                        string match = "Full";

                        string fileName = file.Name.Replace("." + type, "");
                        MainDGV.Rows.Add(false, fileName, type, match, fileline.TrimStart(' ').TrimEnd(' '), file.FullName);
                        break;
                    }
                }
            }
            else if (type == "objectTranslation")
            {
                int pathint = file.FullName.IndexOf("\\objectTranslations\\") + 20;
                if (file.FullName.Substring(pathint).ToLower() == (componentLower + "-en_US.objectTranslation").ToLower())
                {
                    while ((fileline = filestr.ReadLine()) != null)
                    {
                        if (fileline.ToLower().Contains("<name>" + fieldLower + "</name>"))
                        {
                            string match = "Full";
                            string fileName = file.Name.Replace("." + type, "");
                            MainDGV.Rows.Add(false, fileName, type, match, fileline.TrimStart(' ').TrimEnd(' '), file.FullName);
                            break;
                        }
                    }
                }
            }
        }

        private void searchButtonClick(object sender, EventArgs e)
        {
             if (searchButtonValidation(sender,e))
            {
                MainDGV.Rows.Clear();
                Cursor.Current = Cursors.WaitCursor;
                Task.Delay(2000);
                List<XmlDocument> docs = new List<XmlDocument>();
                string field = FieldTextBox.Text.Trim();
                string fieldLower = field.ToLower();
                string component = ObjectTextBox.Text.Trim();
                string componentLower = component.ToLower();
                DirectoryInfo root = new DirectoryInfo(PathTextBox.Text);
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
                                    searchButtonProcess(sender, e, fileline, fieldLower, componentLower, file, filestr, "reportType");
                                }
                                else if (fileline.Contains("<PermissionSet"))
                                {
                                    searchButtonProcess(sender, e, fileline, fieldLower, componentLower, file, filestr, "permissionset");
                                }
                                else if (fileline.Contains("<Profile"))
                                {
                                    searchButtonProcess(sender, e, fileline, fieldLower, componentLower, file, filestr, "profile");
                                }
                                else if (fileline.Contains("<CustomObjectTranslation"))
                                {
                                    searchButtonProcess(sender, e, fileline, fieldLower, componentLower, file, filestr, "objectTranslation");
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
                                            searchButtonProcess(sender, e, reportfileline, fieldLower, componentLower, reportfile, reportfilestr, "report");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (MainDGV.RowCount != 0 && FieldTextBox.TextLength != 0)
                {
                    TotalFieldsStatusLabel.Text = "Total files found: " + MainDGV.RowCount;
                    SelectedFieldsStatusLabel.Text = "Selected files: 0";
                    DGVCover.Hide();
                    DGVCoverImg.Hide();
                    CleanButton.Enabled = true;
                    SelectAllButton.Enabled = true;
                    UnselectAllButton.Enabled = true;
                    SelectMatchedButton.Enabled = true;
                    resizeColumns();
                }
                else
                {
                    TotalFieldsStatusLabel.Text = "Total files found: 0";
                    DGVCover.Show();
                    DGVCoverImg.Show();
                    CleanButton.Enabled = false;
                    SelectAllButton.Enabled = false;
                    UnselectAllButton.Enabled = false;
                    SelectMatchedButton.Enabled = false;
                    MessageBox.Show("No references were found from the desired field in the selected folder.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void cleanButtonClick(object sender, EventArgs e)
        {
            if (srcAsDestCheckBox.Checked)
            {
                DialogResult result2 = MessageBox.Show("This process will overwrite the checked files in the path selected.\r\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result2 == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (MainDGV.Rows.Count != 0)
                    {
                        bool chkboxtrue = false;
                        string field = FieldTextBox.Text.Trim();
                        string component = ObjectTextBox.Text.Trim();
                        foreach (DataGridViewRow row in MainDGV.Rows)
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
                                string matchLine = row.Cells[4].Value.ToString();
                                doc.Load(pathCell);
                                if (pathCell.Contains("\\reportTypes\\"))
                                {
                                    string fieldMatched = matchLine.Replace("<field>", "");
                                    fieldMatched = fieldMatched.Replace("</field>", "");
                                    XmlNodeList NodeLista = doc.GetElementsByTagName("ReportType");
                                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("sections");
                                    for (int lis = 0; lis < NodeLista2.Count && bkr == false; lis++)
                                    {
                                        XmlNodeList NodeLista3 = null;
                                        NodeLista3 = ((XmlElement)NodeLista2[i]).GetElementsByTagName("columns");
                                        foreach (XmlElement nodo in NodeLista3)
                                        {
                                            XmlNodeList nField = nodo.GetElementsByTagName("field");
                                            if (nField[0].InnerText.Contains(fieldMatched))
                                            {
                                                nodo.ParentNode.RemoveChild(nodo);
                                                int pFrom = pathCell.IndexOf("\\src\\");
                                                String pathCellfinal = PathTextBox.Text + pathCell.Substring(pFrom + 4);
                                                System.IO.Directory.CreateDirectory(PathTextBox.Text + "\\reportTypes");
                                                XmlWriterSettings settings = new XmlWriterSettings
                                                {
                                                    Encoding = Encoding.UTF8,
                                                    Indent = true,
                                                    IndentChars = "    ",
                                                    NewLineChars = "\r\n",
                                                    NewLineHandling = NewLineHandling.Replace,
                                                    CloseOutput = true
                                                };
                                                writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                doc.Save(writer);
                                                writer.Close();
                                                string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                                string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                string xmlpart2 = xmlString.Substring(38);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                {
                                                    sw.Write(xmlpart1 + xmlpart2);
                                                    sw.WriteLine("");
                                                }

                                                StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                StreamReader sr2 = File.OpenText(pathCellfinal);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                {
                                                    string fileline1;
                                                    string fileline2;
                                                    while ((fileline1 = sr1.ReadLine()) != null)
                                                    {
                                                        while ((fileline2 = sr2.ReadLine()) != null)
                                                        {
                                                            if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t") || fileline1.Trim().Replace(" ", "") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("'", "&apos;") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("\"", "&quot;") == fileline2.Trim().Replace(" ", ""))
                                                            {
                                                                sw.WriteLine(fileline2);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                sr1.Close();
                                                sr2.Close();
                                                string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                {
                                                    sw.Write(replacementXML);
                                                }
                                                File.Delete(pathCellfinal + ".tmp");
                                                File.Delete(pathCellfinal + ".tmp2");
                                                bkr = true;
                                                break;
                                            }
                                        }
                                        i++;
                                    }
                                }
                                else if (pathCell.Contains("\\permissionsets\\"))
                                {
                                    string fieldMatched = matchLine.Replace("<field>", "");
                                    fieldMatched = fieldMatched.Replace("</field>", "");
                                    XmlNodeList NodeLista = doc.GetElementsByTagName("PermissionSet");
                                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                                    foreach (XmlElement nodo in NodeLista2)
                                    {
                                        XmlNodeList nField = nodo.GetElementsByTagName("field");
                                        if (nField[0].InnerText.Contains(fieldMatched))
                                        {
                                            nodo.ParentNode.RemoveChild(nodo);
                                            int pFrom = pathCell.IndexOf("\\src\\");
                                            String pathCellfinal = PathTextBox.Text + pathCell.Substring(pFrom + 4);
                                            System.IO.Directory.CreateDirectory(PathTextBox.Text + "\\permissionsets");
                                            XmlWriterSettings settings = new XmlWriterSettings
                                            {
                                                Encoding = Encoding.UTF8,
                                                Indent = true,
                                                IndentChars = "    ",
                                                NewLineChars = "\r\n",
                                                NewLineHandling = NewLineHandling.Replace,
                                                CloseOutput = true
                                            };
                                            writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                            doc.Save(writer);
                                            writer.Close();
                                            string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                            string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                            string xmlpart2 = xmlString.Substring(38);
                                            using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                            {
                                                sw.Write(xmlpart1 + xmlpart2);
                                                sw.WriteLine("");
                                            }

                                            StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                            StreamReader sr2 = File.OpenText(pathCellfinal);
                                            using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                            {
                                                string fileline1;
                                                string fileline2;
                                                while ((fileline1 = sr1.ReadLine()) != null)
                                                {
                                                    while ((fileline2 = sr2.ReadLine()) != null)
                                                    {
                                                        if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t") || fileline1.Trim().Replace(" ", "") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("'", "&apos;") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("\"", "&quot;") == fileline2.Trim().Replace(" ", ""))
                                                        {
                                                            sw.WriteLine(fileline2);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            sr1.Close();
                                            sr2.Close();
                                            string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                            using (StreamWriter sw = File.CreateText(pathCellfinal))
                                            {
                                                sw.Write(replacementXML);
                                            }
                                            File.Delete(pathCellfinal + ".tmp");
                                            File.Delete(pathCellfinal + ".tmp2");
                                            bkr = true;
                                            break;
                                        }
                                        i++;
                                    }
                                }
                                else if (pathCell.Contains("\\reports\\"))
                                {
                                    string fieldMatched = matchLine.Replace("<field>", "");
                                    fieldMatched = fieldMatched.Replace("</field>", "");
                                    XmlNodeList NodeLista = doc.GetElementsByTagName("Report");
                                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("columns");
                                    foreach (XmlElement nodo in NodeLista2)
                                    {
                                        XmlNodeList nField = nodo.GetElementsByTagName("field");
                                        if (nField[0].InnerText.Contains(fieldMatched))
                                        {
                                            nodo.ParentNode.RemoveChild(nodo);
                                            int pFrom = pathCell.IndexOf("\\src\\");
                                            String pathCellfinal = PathTextBox.Text + pathCell.Substring(pFrom + 4);
                                            String reportFolder = pathCell.Substring(pFrom+4).Replace(row.Cells[1].Value.ToString() + "." + row.Cells[2].Value.ToString(), "");
                                            System.IO.Directory.CreateDirectory(PathTextBox.Text + reportFolder);
                                            XmlWriterSettings settings = new XmlWriterSettings
                                            {
                                                Encoding = Encoding.UTF8,
                                                Indent = true,
                                                IndentChars = "    ",
                                                NewLineChars = "\r\n",
                                                NewLineHandling = NewLineHandling.Replace,
                                                CloseOutput = true
                                            };
                                            writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                            doc.Save(writer);
                                            writer.Close();
                                            string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                            string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                            string xmlpart2 = xmlString.Substring(38);
                                            using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                            {
                                                sw.Write(xmlpart1 + xmlpart2);
                                                sw.WriteLine("");
                                            }

                                            StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                            StreamReader sr2 = File.OpenText(pathCellfinal);
                                            using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                            {
                                                string fileline1;
                                                string fileline2;
                                                while ((fileline1 = sr1.ReadLine()) != null)
                                                {
                                                    while ((fileline2 = sr2.ReadLine()) != null)
                                                    {
                                                        if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t") || fileline1.Trim().Replace(" ", "") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("'", "&apos;") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("\"", "&quot;") == fileline2.Trim().Replace(" ", ""))
                                                        {
                                                            sw.WriteLine(fileline2);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            sr1.Close();
                                            sr2.Close();
                                            string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                            using (StreamWriter sw = File.CreateText(pathCellfinal))
                                            {
                                                sw.Write(replacementXML);
                                            }
                                            File.Delete(pathCellfinal + ".tmp");
                                            File.Delete(pathCellfinal + ".tmp2");
                                            bkr = true;
                                            break;
                                        }
                                        i++;
                                    }
                                }
                                else if (pathCell.Contains("\\profiles\\"))
                                {
                                    string fieldMatched = matchLine.Replace("<field>", "");
                                    fieldMatched = fieldMatched.Replace("</field>", "");
                                    XmlNodeList NodeLista = doc.GetElementsByTagName("Profile");
                                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                                    foreach (XmlElement nodo in NodeLista2)
                                    {
                                        XmlNodeList nField = nodo.GetElementsByTagName("field");
                                        if (nField[0].InnerText.Contains(fieldMatched))
                                        {
                                            nodo.ParentNode.RemoveChild(nodo);
                                            int pFrom = pathCell.IndexOf("\\src\\");
                                            String pathCellfinal = PathTextBox.Text + pathCell.Substring(pFrom + 4);
                                            System.IO.Directory.CreateDirectory(PathTextBox.Text + "\\profiles");
                                            XmlWriterSettings settings = new XmlWriterSettings
                                            {
                                                Encoding = Encoding.UTF8,
                                                Indent = true,
                                                IndentChars = "    ",
                                                NewLineChars = "\r\n",
                                                NewLineHandling = NewLineHandling.Replace,
                                                CloseOutput = true
                                            };
                                            writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                            doc.Save(writer);
                                            writer.Close();
                                            string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                            string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                            string xmlpart2 = xmlString.Substring(38);
                                            using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                            {
                                                sw.Write(xmlpart1 + xmlpart2);
                                                sw.WriteLine("");
                                            }

                                            StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                            StreamReader sr2 = File.OpenText(pathCellfinal);
                                            using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                            {
                                                string fileline1;
                                                string fileline2;
                                                while ((fileline1 = sr1.ReadLine()) != null)
                                                {
                                                    while ((fileline2 = sr2.ReadLine()) != null)
                                                    {
                                                        if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t") || fileline1.Trim().Replace(" ", "") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("'", "&apos;") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("\"", "&quot;") == fileline2.Trim().Replace(" ", ""))
                                                        {
                                                            sw.WriteLine(fileline2);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            sr1.Close();
                                            sr2.Close();
                                            string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                            using (StreamWriter sw = File.CreateText(pathCellfinal))
                                            {
                                                sw.Write(replacementXML);
                                            }
                                            File.Delete(pathCellfinal + ".tmp");
                                            File.Delete(pathCellfinal + ".tmp2");
                                            bkr = true;
                                            break;
                                        }
                                        i++;
                                    }
                                }
                                else if (pathCell.Contains("\\objectTranslations\\"))
                                {
                                    string fieldMatched = matchLine.Replace("<name>", "");
                                    fieldMatched = fieldMatched.Replace("</name>", "");
                                    XmlNodeList NodeLista = doc.GetElementsByTagName("CustomObjectTranslation");
                                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fields");
                                    foreach (XmlElement nodo in NodeLista2)
                                    {
                                        XmlNodeList nField = nodo.GetElementsByTagName("name");
                                        if (nField[0].InnerText.Contains(fieldMatched))
                                        {
                                            nodo.ParentNode.RemoveChild(nodo);
                                            int pFrom = pathCell.IndexOf("\\src\\");
                                            String pathCellfinal = PathTextBox.Text + pathCell.Substring(pFrom + 4);
                                            System.IO.Directory.CreateDirectory(PathTextBox.Text + "\\objectTranslations");
                                            XmlWriterSettings settings = new XmlWriterSettings
                                            {
                                                Encoding = Encoding.UTF8,
                                                Indent = true,
                                                IndentChars = "    ",
                                                NewLineChars = "\r\n",
                                                NewLineHandling = NewLineHandling.Replace,
                                                CloseOutput = true
                                            };
                                            writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                            doc.Save(writer);
                                            writer.Close();
                                            string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
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
                                            using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                            {
                                                sw.Write(xmlpart1 + xmlpart2);
                                                sw.WriteLine("");
                                            }
                                            StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                            StreamReader sr2 = File.OpenText(pathCellfinal);
                                            using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                            {
                                                string fileline1;
                                                string fileline2;
                                                while ((fileline1 = sr1.ReadLine()) != null)
                                                {
                                                    while ((fileline2 = sr2.ReadLine()) != null)
                                                    {
                                                        if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t") || fileline1.Trim().Replace(" ", "") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("'", "&apos;") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("\"", "&quot;") == fileline2.Trim().Replace(" ", ""))
                                                        {
                                                            sw.WriteLine(fileline2);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            sr1.Close();
                                            sr2.Close();
                                            string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                            using (StreamWriter sw = File.CreateText(pathCellfinal))
                                            {
                                                sw.Write(replacementXML);
                                            }
                                            File.Delete(pathCellfinal + ".tmp");
                                            File.Delete(pathCellfinal + ".tmp2");
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
                DialogResult result = SaveFolderDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (Directory.Exists(SaveFolderDialog.SelectedPath + "\\src"))
                    {
                        DialogResult result2 = MessageBox.Show("This process will overwrite the checked files in the path selected.\r\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result2 == DialogResult.Yes)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            if (MainDGV.Rows.Count != 0)
                            {
                                bool chkboxtrue = false;
                                string field = FieldTextBox.Text.Trim();
                                string component = ObjectTextBox.Text.Trim();
                                foreach (DataGridViewRow row in MainDGV.Rows)
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
                                        string matchLine = row.Cells[4].Value.ToString();
                                        doc.Load(pathCell);
                                        if (pathCell.Contains("\\reportTypes\\"))
                                        {
                                            string fieldMatched = matchLine.Replace("<field>", "");
                                            fieldMatched = fieldMatched.Replace("</field>", "");
                                            XmlNodeList NodeLista = doc.GetElementsByTagName("ReportType");
                                            XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("sections");
                                            for (int lis = 0; lis < NodeLista2.Count && bkr == false; lis++)
                                            {
                                                XmlNodeList NodeLista3 = null;
                                                NodeLista3 = ((XmlElement)NodeLista2[i]).GetElementsByTagName("columns");
                                                foreach (XmlElement nodo in NodeLista3)
                                                {
                                                    XmlNodeList nField = nodo.GetElementsByTagName("field");
                                                    if (nField[0].InnerText.ToLower().Contains(fieldMatched))
                                                    {
                                                        nodo.ParentNode.RemoveChild(nodo);
                                                        int pFrom = pathCell.IndexOf("\\src\\");
                                                        String pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                                                        System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + "\\src\\reportTypes");
                                                        XmlWriterSettings settings = new XmlWriterSettings
                                                        {
                                                            Encoding = Encoding.UTF8,
                                                            Indent = true,
                                                            IndentChars = "    ",
                                                            NewLineChars = "\r\n",
                                                            NewLineHandling = NewLineHandling.Replace,
                                                            CloseOutput = true
                                                        };
                                                        writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                        doc.Save(writer);
                                                        writer.Close();
                                                        string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                                        string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                        string xmlpart2 = xmlString.Substring(38);
                                                        using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                        {
                                                            sw.Write(xmlpart1 + xmlpart2);
                                                            sw.WriteLine("");
                                                        }
                                                        StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                        StreamReader sr2 = File.OpenText(pathCell);
                                                        using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                        {
                                                            string fileline1;
                                                            string fileline2;
                                                            while ((fileline1 = sr1.ReadLine()) != null)
                                                            {
                                                                while ((fileline2 = sr2.ReadLine()) != null)
                                                                {
                                                                    if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t") || fileline1.Trim().Replace(" ", "") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("'", "&apos;") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("\"", "&quot;") == fileline2.Trim().Replace(" ", ""))
                                                                    {
                                                                        sw.WriteLine(fileline2);
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        sr1.Close();
                                                        sr2.Close();
                                                        string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                        using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                        {
                                                            sw.Write(replacementXML);
                                                        }
                                                        File.Delete(pathCellfinal + ".tmp");
                                                        File.Delete(pathCellfinal + ".tmp2");
                                                        bkr = true;
                                                        break;
                                                    }
                                                }
                                                i++;
                                            }
                                        }
                                        else if (pathCell.Contains("\\permissionsets\\"))
                                        {
                                            string fieldMatched = matchLine.Replace("<field>", "");
                                            fieldMatched = fieldMatched.Replace("</field>", "");
                                            XmlNodeList NodeLista = doc.GetElementsByTagName("PermissionSet");
                                            XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                                            foreach (XmlElement nodo in NodeLista2)
                                            {
                                                XmlNodeList nField = nodo.GetElementsByTagName("field");
                                                if (nField[0].InnerText.Contains(fieldMatched))
                                                {
                                                    nodo.ParentNode.RemoveChild(nodo);
                                                    int pFrom = pathCell.IndexOf("\\src\\");
                                                    String pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                                                    System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + "\\src\\permissionsets");
                                                    XmlWriterSettings settings = new XmlWriterSettings
                                                    {
                                                        Encoding = Encoding.UTF8,
                                                        Indent = true,
                                                        IndentChars = "    ",
                                                        NewLineChars = "\r\n",
                                                        NewLineHandling = NewLineHandling.Replace,
                                                        CloseOutput = true
                                                    };
                                                    writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                    doc.Save(writer);
                                                    writer.Close();
                                                    string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                                    string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                    string xmlpart2 = xmlString.Substring(38);
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                    {
                                                        sw.Write(xmlpart1 + xmlpart2);
                                                        sw.WriteLine("");
                                                    }
                                                    StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                    StreamReader sr2 = File.OpenText(pathCell);
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                    {
                                                        string fileline1;
                                                        string fileline2;
                                                        while ((fileline1 = sr1.ReadLine()) != null)
                                                        {
                                                            while ((fileline2 = sr2.ReadLine()) != null)
                                                            {
                                                                if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t") || fileline1.Trim().Replace(" ", "") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("'", "&apos;") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("\"", "&quot;") == fileline2.Trim().Replace(" ", ""))
                                                                {
                                                                    sw.WriteLine(fileline2);
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    sr1.Close();
                                                    sr2.Close();
                                                    string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                    {
                                                        sw.Write(replacementXML);
                                                    }
                                                    File.Delete(pathCellfinal + ".tmp");
                                                    File.Delete(pathCellfinal + ".tmp2");
                                                    bkr = true;
                                                    break;
                                                }
                                                i++;
                                            }
                                        }
                                        else if (pathCell.Contains("\\reports\\"))
                                        {
                                            string fieldMatched = matchLine.Replace("<field>", "");
                                            fieldMatched = fieldMatched.Replace("</field>", "");
                                            XmlNodeList NodeLista = doc.GetElementsByTagName("Report");
                                            XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("columns");
                                            foreach (XmlElement nodo in NodeLista2)
                                            {
                                                XmlNodeList nField = nodo.GetElementsByTagName("field");
                                                if (nField[0].InnerText.Contains(fieldMatched))
                                                {
                                                    nodo.ParentNode.RemoveChild(nodo);
                                                    int pFrom = pathCell.IndexOf("\\src\\");
                                                    String pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                                                    String reportFolder = pathCell.Substring(pFrom).Replace(row.Cells[1].Value.ToString() + "." + row.Cells[2].Value.ToString(), "");
                                                    System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + reportFolder);
                                                    XmlWriterSettings settings = new XmlWriterSettings
                                                    {
                                                        Encoding = Encoding.UTF8,
                                                        Indent = true,
                                                        IndentChars = "    ",
                                                        NewLineChars = "\r\n",
                                                        NewLineHandling = NewLineHandling.Replace,
                                                        CloseOutput = true
                                                    };
                                                    writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                    doc.Save(writer);
                                                    writer.Close();
                                                    string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                                    string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                    string xmlpart2 = xmlString.Substring(38);
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                    {
                                                        sw.Write(xmlpart1 + xmlpart2);
                                                        sw.WriteLine("");
                                                    }
                                                    StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                    StreamReader sr2 = File.OpenText(pathCell);
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                    {
                                                        string fileline1;
                                                        string fileline2;
                                                        while ((fileline1 = sr1.ReadLine()) != null)
                                                        {
                                                            while ((fileline2 = sr2.ReadLine()) != null)
                                                            {
                                                                if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t") || fileline1.Trim().Replace(" ", "") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("'", "&apos;") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("\"", "&quot;") == fileline2.Trim().Replace(" ", ""))
                                                                {
                                                                    sw.WriteLine(fileline2);
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    sr1.Close();
                                                    sr2.Close();
                                                    string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                    {
                                                        sw.Write(replacementXML);
                                                    }
                                                    File.Delete(pathCellfinal + ".tmp");
                                                    File.Delete(pathCellfinal + ".tmp2");
                                                    bkr = true;
                                                    break;
                                                }
                                                i++;
                                            }
                                        }
                                        else if (pathCell.Contains("\\profiles\\"))
                                        {
                                            string fieldMatched = matchLine.Replace("<field>", "");
                                            fieldMatched = fieldMatched.Replace("</field>", "");
                                            XmlNodeList NodeLista = doc.GetElementsByTagName("Profile");
                                            XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                                            foreach (XmlElement nodo in NodeLista2)
                                            {
                                                XmlNodeList nField = nodo.GetElementsByTagName("field");
                                                if (nField[0].InnerText.Contains(fieldMatched))
                                                {
                                                    nodo.ParentNode.RemoveChild(nodo);
                                                    int pFrom = pathCell.IndexOf("\\src\\");
                                                    String pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                                                    System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + "\\src\\profiles");
                                                    XmlWriterSettings settings = new XmlWriterSettings
                                                    {
                                                        Encoding = Encoding.UTF8,
                                                        Indent = true,
                                                        IndentChars = "    ",
                                                        NewLineChars = "\r\n",
                                                        NewLineHandling = NewLineHandling.Replace,
                                                        CloseOutput = true
                                                    };
                                                    writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                    doc.Save(writer);
                                                    writer.Close();
                                                    string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                                    string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                    string xmlpart2 = xmlString.Substring(38);
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                    {
                                                        sw.Write(xmlpart1 + xmlpart2);
                                                        sw.WriteLine("");
                                                    }
                                                    StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                    StreamReader sr2 = File.OpenText(pathCell);
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                    {
                                                        string fileline1;
                                                        string fileline2;
                                                        while ((fileline1 = sr1.ReadLine()) != null)
                                                        {
                                                            while ((fileline2 = sr2.ReadLine()) != null)
                                                            {
                                                                if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t") || fileline1.Trim().Replace(" ", "") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("'", "&apos;") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("\"", "&quot;") == fileline2.Trim().Replace(" ", ""))
                                                                {
                                                                    sw.WriteLine(fileline2);
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    sr1.Close();
                                                    sr2.Close();
                                                    string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                    {
                                                        sw.Write(replacementXML);
                                                    }
                                                    File.Delete(pathCellfinal + ".tmp");
                                                    File.Delete(pathCellfinal + ".tmp2");
                                                    bkr = true;
                                                    break;
                                                }
                                                i++;
                                            }
                                        }
                                        else if (pathCell.Contains("\\objectTranslations\\"))
                                        {
                                            string fieldMatched = matchLine.Replace("<name>", "");
                                            fieldMatched = fieldMatched.Replace("</name>", "");
                                            XmlNodeList NodeLista = doc.GetElementsByTagName("CustomObjectTranslation");
                                            XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fields");
                                            foreach (XmlElement nodo in NodeLista2)
                                            {
                                                XmlNodeList nField = nodo.GetElementsByTagName("name");
                                                if (nField[0].InnerText.Contains(fieldMatched))
                                                {
                                                    nodo.ParentNode.RemoveChild(nodo);
                                                    int pFrom = pathCell.IndexOf("\\src\\");
                                                    String pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                                                    System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + "\\src\\objectTranslations");
                                                    XmlWriterSettings settings = new XmlWriterSettings
                                                    {
                                                        Encoding = Encoding.UTF8,
                                                        Indent = true,
                                                        IndentChars = "    ",
                                                        NewLineChars = "\r\n",
                                                        NewLineHandling = NewLineHandling.Replace,
                                                        CloseOutput = true
                                                    };
                                                    writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                    doc.Save(writer);
                                                    writer.Close();
                                                    string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
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
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                    {
                                                        sw.Write(xmlpart1 + xmlpart2);
                                                        sw.WriteLine("");
                                                    }
                                                    StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                    StreamReader sr2 = File.OpenText(pathCell);
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                    {
                                                        string fileline1;
                                                        string fileline2;
                                                        while ((fileline1 = sr1.ReadLine()) != null)
                                                        {
                                                            while ((fileline2 = sr2.ReadLine()) != null)
                                                            {
                                                                if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t") || fileline1.Trim().Replace(" ", "") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("'", "&apos;") == fileline2.Trim().Replace(" ", "") || fileline1.Trim().Replace(" ", "").Replace("\"", "&quot;") == fileline2.Trim().Replace(" ", ""))
                                                                {
                                                                    sw.WriteLine(fileline2);
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    sr1.Close();
                                                    sr2.Close();
                                                    string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                    {
                                                        sw.Write(replacementXML);
                                                    }
                                                    File.Delete(pathCellfinal + ".tmp");
                                                    File.Delete(pathCellfinal + ".tmp2");
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
                        if (MainDGV.Rows.Count != 0)
                        {
                            bool chkboxtrue = false;
                            string field = FieldTextBox.Text.Trim();
                            string component = ObjectTextBox.Text.Trim();
                            foreach (DataGridViewRow row in MainDGV.Rows)
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
                                    string matchLine = row.Cells[4].Value.ToString();
                                    doc.Load(pathCell);
                                    if (pathCell.Contains("\\reportTypes\\"))
                                    {
                                        string fieldMatched = matchLine.Replace("<field>", "");
                                        fieldMatched = fieldMatched.Replace("</field>", "");
                                        XmlNodeList NodeLista = doc.GetElementsByTagName("ReportType");
                                        XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("sections");
                                        for (int lis = 0; lis < NodeLista2.Count && bkr == false; lis++)
                                        {
                                            XmlNodeList NodeLista3 = null;
                                            NodeLista3 = ((XmlElement)NodeLista2[i]).GetElementsByTagName("columns");
                                            foreach (XmlElement nodo in NodeLista3)
                                            {
                                                XmlNodeList nField = nodo.GetElementsByTagName("field");
                                                if (nField[0].InnerText.ToLower().Contains(fieldMatched))
                                                {
                                                    nodo.ParentNode.RemoveChild(nodo);
                                                    int pFrom = pathCell.IndexOf("\\src\\");
                                                    String pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                                                    System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + "\\src\\reportTypes");
                                                    XmlWriterSettings settings = new XmlWriterSettings
                                                    {
                                                        Encoding = Encoding.UTF8,
                                                        Indent = true,
                                                        IndentChars = "    ",
                                                        NewLineChars = "\r\n",
                                                        NewLineHandling = NewLineHandling.Replace,
                                                        CloseOutput = true
                                                    };
                                                    writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                    doc.Save(writer);
                                                    writer.Close();
                                                    string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                                    string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                    string xmlpart2 = xmlString.Substring(38);
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                    {
                                                        sw.Write(xmlpart1 + xmlpart2);
                                                        sw.WriteLine("");
                                                    }
                                                    StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                    StreamReader sr2 = File.OpenText(pathCell);
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                    {
                                                        string fileline1;
                                                        string fileline2;
                                                        while ((fileline1 = sr1.ReadLine()) != null)
                                                        {
                                                            while ((fileline2 = sr2.ReadLine()) != null)
                                                            {
                                                                if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t"))
                                                                {
                                                                    sw.WriteLine(fileline2);
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    sr1.Close();
                                                    sr2.Close();
                                                    string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                    using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                    {
                                                        sw.Write(replacementXML);
                                                    }
                                                    File.Delete(pathCellfinal + ".tmp");
                                                    File.Delete(pathCellfinal + ".tmp2");
                                                    bkr = true;
                                                    break;
                                                }
                                            }
                                            i++;
                                        }
                                    }
                                    else if (pathCell.Contains("\\permissionsets\\"))
                                    {
                                        string fieldMatched = matchLine.Replace("<field>", "");
                                        fieldMatched = fieldMatched.Replace("</field>", "");
                                        XmlNodeList NodeLista = doc.GetElementsByTagName("PermissionSet");
                                        XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                                        foreach (XmlElement nodo in NodeLista2)
                                        {
                                            XmlNodeList nField = nodo.GetElementsByTagName("field");
                                            if (nField[0].InnerText.Contains(fieldMatched))
                                            {
                                                nodo.ParentNode.RemoveChild(nodo);
                                                int pFrom = pathCell.IndexOf("\\src\\");
                                                String pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                                                System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + "\\src\\permissionsets");
                                                XmlWriterSettings settings = new XmlWriterSettings
                                                {
                                                    Encoding = Encoding.UTF8,
                                                    Indent = true,
                                                    IndentChars = "    ",
                                                    NewLineChars = "\r\n",
                                                    NewLineHandling = NewLineHandling.Replace,
                                                    CloseOutput = true
                                                };
                                                writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                doc.Save(writer);
                                                writer.Close();
                                                string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                                string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                string xmlpart2 = xmlString.Substring(38);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                {
                                                    sw.Write(xmlpart1 + xmlpart2);
                                                    sw.WriteLine("");
                                                }
                                                StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                StreamReader sr2 = File.OpenText(pathCell);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                {
                                                    string fileline1;
                                                    string fileline2;
                                                    while ((fileline1 = sr1.ReadLine()) != null)
                                                    {
                                                        while ((fileline2 = sr2.ReadLine()) != null)
                                                        {
                                                            if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t"))
                                                            {
                                                                sw.WriteLine(fileline2);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                sr1.Close();
                                                sr2.Close();
                                                string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                {
                                                    sw.Write(replacementXML);
                                                }
                                                File.Delete(pathCellfinal + ".tmp");
                                                File.Delete(pathCellfinal + ".tmp2");
                                                bkr = true;
                                                break;
                                            }
                                            i++;
                                        }
                                    }
                                    else if (pathCell.Contains("\\reports\\"))
                                    {
                                        string fieldMatched = matchLine.Replace("<field>", "");
                                        fieldMatched = fieldMatched.Replace("</field>", "");
                                        XmlNodeList NodeLista = doc.GetElementsByTagName("Report");
                                        XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("columns");
                                        foreach (XmlElement nodo in NodeLista2)
                                        {
                                            XmlNodeList nField = nodo.GetElementsByTagName("field");
                                            if (nField[0].InnerText.Contains(fieldMatched))
                                            {
                                                nodo.ParentNode.RemoveChild(nodo);
                                                int pFrom = pathCell.IndexOf("\\src\\");
                                                String pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                                                String reportFolder = pathCell.Substring(pFrom).Replace(row.Cells[1].Value.ToString() + "." + row.Cells[2].Value.ToString(), "");
                                                System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + reportFolder);
                                                XmlWriterSettings settings = new XmlWriterSettings
                                                {
                                                    Encoding = Encoding.UTF8,
                                                    Indent = true,
                                                    IndentChars = "    ",
                                                    NewLineChars = "\r\n",
                                                    NewLineHandling = NewLineHandling.Replace,
                                                    CloseOutput = true
                                                };
                                                writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                doc.Save(writer);
                                                writer.Close();
                                                string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                                string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                string xmlpart2 = xmlString.Substring(38);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                {
                                                    sw.Write(xmlpart1 + xmlpart2);
                                                    sw.WriteLine("");
                                                }
                                                StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                StreamReader sr2 = File.OpenText(pathCell);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                {
                                                    string fileline1;
                                                    string fileline2;
                                                    while ((fileline1 = sr1.ReadLine()) != null)
                                                    {
                                                        while ((fileline2 = sr2.ReadLine()) != null)
                                                        {
                                                            if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t"))
                                                            {
                                                                sw.WriteLine(fileline2);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                sr1.Close();
                                                sr2.Close();
                                                string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                {
                                                    sw.Write(replacementXML);
                                                }
                                                File.Delete(pathCellfinal + ".tmp");
                                                File.Delete(pathCellfinal + ".tmp2");
                                                bkr = true;
                                                break;
                                            }
                                            i++;
                                        }
                                    }
                                    else if (pathCell.Contains("\\profiles\\"))
                                    {
                                        string fieldMatched = matchLine.Replace("<field>", "");
                                        fieldMatched = fieldMatched.Replace("</field>", "");
                                        XmlNodeList NodeLista = doc.GetElementsByTagName("Profile");
                                        XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                                        foreach (XmlElement nodo in NodeLista2)
                                        {
                                            XmlNodeList nField = nodo.GetElementsByTagName("field");
                                            if (nField[0].InnerText.Contains(fieldMatched))
                                            {
                                                nodo.ParentNode.RemoveChild(nodo);
                                                int pFrom = pathCell.IndexOf("\\src\\");
                                                String pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                                                System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + "\\src\\profiles");
                                                XmlWriterSettings settings = new XmlWriterSettings
                                                {
                                                    Encoding = Encoding.UTF8,
                                                    Indent = true,
                                                    IndentChars = "    ",
                                                    NewLineChars = "\r\n",
                                                    NewLineHandling = NewLineHandling.Replace,
                                                    CloseOutput = true
                                                };
                                                writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                doc.Save(writer);
                                                writer.Close();
                                                string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
                                                string xmlpart1 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                                                string xmlpart2 = xmlString.Substring(38);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                {
                                                    sw.Write(xmlpart1 + xmlpart2);
                                                    sw.WriteLine("");
                                                }
                                                StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                StreamReader sr2 = File.OpenText(pathCell);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                {
                                                    string fileline1;
                                                    string fileline2;
                                                    while ((fileline1 = sr1.ReadLine()) != null)
                                                    {
                                                        while ((fileline2 = sr2.ReadLine()) != null)
                                                        {
                                                            if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t"))
                                                            {
                                                                sw.WriteLine(fileline2);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                sr1.Close();
                                                sr2.Close();
                                                string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                {
                                                    sw.Write(replacementXML);
                                                }
                                                File.Delete(pathCellfinal + ".tmp");
                                                File.Delete(pathCellfinal + ".tmp2");
                                                bkr = true;
                                                break;
                                            }
                                            i++;
                                        }
                                    }
                                    else if (pathCell.Contains("\\objectTranslations\\"))
                                    {
                                        string fieldMatched = matchLine.Replace("<name>", "");
                                        fieldMatched = fieldMatched.Replace("</name>", "");
                                        XmlNodeList NodeLista = doc.GetElementsByTagName("CustomObjectTranslation");
                                        XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fields");
                                        foreach (XmlElement nodo in NodeLista2)
                                        {
                                            XmlNodeList nField = nodo.GetElementsByTagName("name");
                                            if (nField[0].InnerText.Contains(fieldMatched))
                                            {
                                                nodo.ParentNode.RemoveChild(nodo);
                                                int pFrom = pathCell.IndexOf("\\src\\");
                                                String pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                                                System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + "\\src\\objectTranslations");
                                                XmlWriterSettings settings = new XmlWriterSettings
                                                {
                                                    Encoding = Encoding.UTF8,
                                                    Indent = true,
                                                    IndentChars = "    ",
                                                    NewLineChars = "\r\n",
                                                    NewLineHandling = NewLineHandling.Replace,
                                                    CloseOutput = true
                                                };
                                                writer = XmlWriter.Create(pathCellfinal + ".tmp", settings);
                                                doc.Save(writer);
                                                writer.Close();
                                                string xmlString = System.IO.File.ReadAllText(pathCellfinal + ".tmp");
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
                                                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                                                {
                                                    sw.Write(xmlpart1 + xmlpart2);
                                                    sw.WriteLine("");
                                                }
                                                StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                                                StreamReader sr2 = File.OpenText(pathCell);
                                                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp2"))
                                                {
                                                    string fileline1;
                                                    string fileline2;
                                                    while ((fileline1 = sr1.ReadLine()) != null)
                                                    {
                                                        while ((fileline2 = sr2.ReadLine()) != null)
                                                        {
                                                            if (fileline1.Trim() == fileline2.Trim() || fileline2.Contains("\t"))
                                                            {
                                                                sw.WriteLine(fileline2);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                sr1.Close();
                                                sr2.Close();
                                                string replacementXML = File.ReadAllText(pathCellfinal + ".tmp2");
                                                using (StreamWriter sw = File.CreateText(pathCellfinal))
                                                {
                                                    sw.Write(replacementXML);
                                                }
                                                File.Delete(pathCellfinal + ".tmp");
                                                File.Delete(pathCellfinal + ".tmp2");
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
        }

        private void selectAllButtonClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow dgv in MainDGV.Rows)
            {
                dgv.Cells[0].Value = true;
            }
            Cursor.Current = Cursors.Default;
        }

        private void unselectAllButtonClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow dgv in MainDGV.Rows)
            {
                dgv.Cells[0].Value = false;
            }
            Cursor.Current = Cursors.Default;
        }

        private void loadPathButtonClick(object sender, EventArgs e)
        {
            DialogResult result = LoadFolderDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK && LoadFolderDialog.SelectedPath.Substring(LoadFolderDialog.SelectedPath.Length - 3, 3) == "src") // Test result.
            {
                MainDGV.Rows.Clear();
                PathTextBox.Text = LoadFolderDialog.SelectedPath;
                SearchButton.Enabled = true;
                Properties.Settings.Default.PathReminder = PathTextBox.Text;
                Properties.Settings.Default.Save();
            }
            else if (result == DialogResult.OK && LoadFolderDialog.SelectedPath.Substring(LoadFolderDialog.SelectedPath.Length - 3, 3) != "src")
            {
                MessageBox.Show("An \"src\" folder must be selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loadPathButtonClick(sender, e);
            }       
        }

        private void selectMatchedButtonClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow dgv in MainDGV.Rows)
            {
                if (dgv.Cells[3].Value.ToString() == "Full")
                {
                    dgv.Cells[0].Value = true;
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        private void previewDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewSelectedRowCollection rowPrev = MainDGV.SelectedRows;
                if (rowPrev.Count != 1)
                {
                    MessageBox.Show("Please select only one row", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string matchline = rowPrev[0].Cells[4].Value.ToString();
                    string path = rowPrev[0].Cells[5].Value.ToString();
                    string component = rowPrev[0].Cells[1].Value.ToString();
                    string type = rowPrev[0].Cells[2].Value.ToString();
                    PreviewForm preview = new PreviewForm();
                    preview.Text = component + "." + type;
                    string prev = rowPrev.ToString();
                    preview.previewTextBoxLoad(sender, e, matchline, component, path);
                    preview.ShowDialog();
                }
            }
        }

        private void mainApp_Load(object sender, EventArgs e)
        {
            Text = "F.O.C.A. v1.0";
            PathTextBox.Text = Properties.Settings.Default.PathReminder;
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 10; // in miliseconds
            timer1.Start();
        }

        private void dataGridView1_Click_1(object sender, EventArgs e)
        {
            MainDGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private Timer timer1;

        private void timer1_Tick(object sender, EventArgs e)
        {
            MainDGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (MainDGV.RowCount != 0)
            {
                int alt = 0;
                foreach (DataGridViewRow row in MainDGV.Rows)
                {
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        alt++;
                    }
                }
                SelectedFieldsStatusLabel.Text = "Selected files: " + alt;
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
        
}
