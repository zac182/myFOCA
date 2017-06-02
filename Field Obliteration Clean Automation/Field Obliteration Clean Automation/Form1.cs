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
using System.Net;

namespace Field_Obliteration_Clean_Automation
{
    //RC 1.0.20170314
    public partial class MainApp : Form
    {
        public MainApp()
        {
            InitializeComponent();
        }

        public bool previewOn = false;

        private void enableSeachButton(object sender, EventArgs e)
        {
            if (FieldTextBox.Text.Trim() != "" && ObjectTextBox.Text.Trim() != "" && PathTextBox.Text.Trim() != "")
                SearchButton.Enabled = true;
            else
                SearchButton.Enabled = false;
        }

        private bool searchButtonValidation(object sender, EventArgs e)
        {
            if (FieldTextBox.Text.Trim().Length != 0 && !FieldTextBox.Text.Trim().ToLower().EndsWith("__c"))
            {
                if (ObjectTextBox.Text.Trim().Length == 0)
                {
                    MessageBox.Show("The field to be deleted must be a Custom Field.\r\nMust define object.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show("The field to be deleted must be a Custom Field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (FieldTextBox.Text.Trim().Length != 0 && FieldTextBox.Text.Trim().ToLower().Equals("__c"))
            {
                if (ObjectTextBox.Text.Trim().Length == 0)
                {
                    MessageBox.Show("The field to be deleted must be a Custom Field.\r\nMust define object.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show("The field to be deleted must be a Custom Field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (FieldTextBox.Text.Trim().Length == 0)
            {
                if (ObjectTextBox.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Must define field to be deleted.\r\nMust define object.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show("Must define field to be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else if (ObjectTextBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Must define object.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void searchButtonProcess(object sender, EventArgs e, string fieldLower, string componentLower, FileInfo file, StreamReader filestr, string typeFile, string type, XmlDocument doc)
        {
            try
            {

                int i = 0;
                bool bkr = false;
                if (type == "ReportType")
                {
                    XmlNodeList NodeLista = doc.GetElementsByTagName(type);
                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("sections");
                    for (int lis = 0; lis < NodeLista2.Count && bkr == false; lis++)
                    {
                        XmlNodeList NodeLista3 = null;
                        NodeLista3 = ((XmlElement)NodeLista2[i]).GetElementsByTagName("columns");
                        foreach (XmlElement nodo in NodeLista3)
                        {
                            XmlNodeList nField = nodo.GetElementsByTagName("field");
                            if (nField[0].InnerText.ToLower().EndsWith(fieldLower))
                            {
                                string match = "Partial";
                                if (nField[0].InnerText.ToLower().Equals(componentLower + "." + fieldLower))
                                {
                                    match = "Full";
                                }
                                string fileName = file.Name.Replace("." + type, "");
                                MainDGV.Rows.Add(false, fileName, typeFile, match, nField[0].InnerText, file.FullName);
                                bkr = true;
                                break;
                            }
                        }
                        i++;
                    }
                }
                else if (type == "PermissionSet" || type == "Profile")
                {
                    XmlNodeList NodeLista = doc.GetElementsByTagName(type);
                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                    foreach (XmlElement nodo in NodeLista2)
                    {
                        XmlNodeList nField = nodo.GetElementsByTagName("field");
                        if (nField[0].InnerText.ToLower().Equals(componentLower + "." + fieldLower))
                        {
                            string match = "Full";
                            string fileName = file.Name.Replace("." + type, "");
                            MainDGV.Rows.Add(false, fileName, typeFile, match, nField[0].InnerText, file.FullName);
                            break;
                        }
                    }
                }
                else if (type == "CustomObjectTranslation")
                {
                    XmlNodeList NodeLista = doc.GetElementsByTagName(type);
                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fields");
                    foreach (XmlElement nodo in NodeLista2)
                    {
                        XmlNodeList nField = nodo.GetElementsByTagName("name");
                        if (nField[0].InnerText.ToLower().Equals(fieldLower))
                        {
                            string match = "Full";
                            string fileName = file.Name.Replace("." + type, "");
                            MainDGV.Rows.Add(false, fileName, typeFile, match, nField[0].InnerText, file.FullName);
                            break;
                        }
                    }
                }
                else if (type == "Report")
                {
                    XmlNodeList NodeLista = doc.GetElementsByTagName(type);
                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("columns");
                    foreach (XmlElement nodo in NodeLista2)
                    {
                        XmlNodeList nField = nodo.GetElementsByTagName("field");
                        if (nField[0].InnerText.ToLower().EndsWith(fieldLower))
                        {
                            string match = "Partial";
                            if (nField[0].InnerText.ToLower().Equals(componentLower + "." + fieldLower))
                            {
                                match = "Full";
                            }
                            string fileName = file.Name.Replace("." + type, "");
                            MainDGV.Rows.Add(false, fileName, typeFile, match, nField[0].InnerText, file.FullName);
                            bkr = true;
                            break;
                        }

                    }
                }
            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void searchButtonClick(object sender, EventArgs e)
        {
            if (searchButtonValidation(sender, e))
            {
                MainDGV.Rows.Clear();
                Cursor.Current = Cursors.WaitCursor;
                Task.Delay(2000);
                List<XmlDocument> docs = new List<XmlDocument>();
                string field = FieldTextBox.Text.Trim();
                string fieldLower = field.ToLower();
                string component = ObjectTextBox.Text.Trim();
                string componentLower = component.ToLower();
                if (Directory.Exists(PathTextBox.Text))
                {
                    DirectoryInfo root = new DirectoryInfo(PathTextBox.Text);
                    DirectoryInfo[] dirs = root.GetDirectories();
                    foreach (DirectoryInfo dir in dirs)
                    {
                        if (dir.Name == "reportTypes" || dir.Name == "permissionsets" || dir.Name == "profiles" || dir.Name == "objectTranslations")
                        {
                            FileInfo[] files = dir.GetFiles();
                            foreach (FileInfo file in files)
                            {
                                XmlDocument doc = new XmlDocument();
                                string filepath = file.FullName.ToString();
                                doc.Load(filepath);
                                StreamReader filestr = file.OpenText();
                                if (file.FullName.Contains("reportType"))
                                {
                                    searchButtonProcess(sender, e, fieldLower, componentLower, file, filestr, "reportType", "ReportType", doc);
                                }
                                else if (file.FullName.Contains("permissionset"))
                                {
                                    searchButtonProcess(sender, e, fieldLower, componentLower, file, filestr, "permissionset", "PermissionSet", doc);
                                }
                                else if (file.FullName.Contains("profile"))
                                {
                                    searchButtonProcess(sender, e, fieldLower, componentLower, file, filestr, "profile", "Profile", doc);
                                }
                                else if (file.FullName.Contains("objectTranslation"))
                                {
                                    searchButtonProcess(sender, e, fieldLower, componentLower, file, filestr, "objectTranslation", "CustomObjectTranslation", doc);
                                }

                            }
                        }
                        else if (dir.Name == "reports")
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
                                        XmlDocument doc = new XmlDocument();
                                        string filepath = reportfile.FullName.ToString();
                                        doc.Load(filepath);
                                        StreamReader reportfilestr = reportfile.OpenText();
                                        if (reportfile.FullName.Contains("report"))
                                        {
                                            searchButtonProcess(sender, e, fieldLower, componentLower, reportfile, reportfilestr, "report", "Report", doc);
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
                        previewOn = true;
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
                else
                {
                    MessageBox.Show("Please validate that the folder exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void fileRenew(object sender, EventArgs e, string pathCell, XmlDocument doc, XmlWriter writer, string typeFile, DataGridViewRow row, bool srcAsDest, bool existDir)
        {
            try
            {
                string pathCellfinal;
                int pFrom = pathCell.IndexOf("\\src\\");
                if (typeFile == "report")
                {
                    if (srcAsDest)
                    {
                        pathCellfinal = PathTextBox.Text + pathCell.Substring(pFrom + 4);
                        String reportFolder = pathCell.Substring(pFrom + 4);
                        reportFolder = reportFolder.Replace(row.Cells[1].Value.ToString(), "");
                        System.IO.Directory.CreateDirectory(PathTextBox.Text + reportFolder);
                    }
                    else
                    {
                        pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                        String reportFolder = pathCell.Substring(pFrom);
                        reportFolder = reportFolder.Replace(row.Cells[1].Value.ToString(), "");
                        System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + reportFolder);
                    }
                }
                else
                {
                    if (srcAsDest)
                    {
                        pathCellfinal = PathTextBox.Text + pathCell.Substring(pFrom + 4);
                        System.IO.Directory.CreateDirectory(PathTextBox.Text + "\\" + typeFile);
                    }
                    else
                    {
                        pathCellfinal = SaveFolderDialog.SelectedPath + pathCell.Substring(pFrom);
                        System.IO.Directory.CreateDirectory(SaveFolderDialog.SelectedPath + "\\src\\" + typeFile);
                    }
                }
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
                if (typeFile == "objectTranslations")
                {
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
                }
                using (StreamWriter sw = File.CreateText(pathCellfinal + ".tmp"))
                {
                    sw.Write(xmlpart1 + xmlpart2);
                    sw.WriteLine("");
                }

                StreamReader sr1 = File.OpenText(pathCellfinal + ".tmp");
                StreamReader sr2;
                if (srcAsDest)
                {
                    sr2 = File.OpenText(pathCellfinal);
                }
                else
                {
                    sr2 = File.OpenText(pathCell);
                }
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

            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void cleanButtonProcess(object sender, EventArgs e, int i, string matchLine, bool bkr, XmlDocument doc, XmlWriter writer, DataGridViewRow row, string pathCell, string type, string typeFile, bool srcAsDest, bool existDir)
        {
            try
            {
                if (type == "ReportType")
                {
                    string fieldMatched = matchLine.Replace("<field>", "");
                    fieldMatched = fieldMatched.Replace("</field>", "");
                    XmlNodeList NodeLista = doc.GetElementsByTagName(type);
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
                                fileRenew(sender, e, pathCell, doc, writer, typeFile, row, srcAsDest, existDir);
                                bkr = true;
                                break;
                            }
                        }
                        i++;
                    }
                }
                else if (type == "PermissionSet" || type == "Profile")
                {
                    string fieldMatched = matchLine.Replace("<field>", "");
                    fieldMatched = fieldMatched.Replace("</field>", "");
                    XmlNodeList NodeLista = doc.GetElementsByTagName(type);
                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fieldPermissions");
                    foreach (XmlElement nodo in NodeLista2)
                    {
                        XmlNodeList nField = nodo.GetElementsByTagName("field");
                        if (nField[0].InnerText.Contains(fieldMatched))
                        {
                            nodo.ParentNode.RemoveChild(nodo);
                            fileRenew(sender, e, pathCell, doc, writer, typeFile, row, srcAsDest, existDir);
                            bkr = true;
                            break;
                        }
                        i++;
                    }
                }
                else if (type == "Report")
                {
                    string fieldMatched = matchLine.Replace("<field>", "");
                    fieldMatched = fieldMatched.Replace("</field>", "");
                    XmlNodeList NodeLista = doc.GetElementsByTagName(type);
                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("columns");
                    foreach (XmlElement nodo in NodeLista2)
                    {
                        XmlNodeList nField = nodo.GetElementsByTagName("field");
                        if (nField[0].InnerText.Contains(fieldMatched))
                        {
                            nodo.ParentNode.RemoveChild(nodo);
                            fileRenew(sender, e, pathCell, doc, writer, typeFile, row, srcAsDest, existDir);
                            bkr = true;
                            break;
                        }
                        i++;
                    }
                }
                else if (type == "CustomObjectTranslation")
                {
                    string fieldMatched = matchLine.Replace("<name>", "");
                    fieldMatched = fieldMatched.Replace("</name>", "");
                    XmlNodeList NodeLista = doc.GetElementsByTagName(type);
                    XmlNodeList NodeLista2 = ((XmlElement)NodeLista[0]).GetElementsByTagName("fields");
                    foreach (XmlElement nodo in NodeLista2)
                    {
                        XmlNodeList nField = nodo.GetElementsByTagName("name");
                        if (nField[0].InnerText.Contains(fieldMatched))
                        {
                            nodo.ParentNode.RemoveChild(nodo);
                            fileRenew(sender, e, pathCell, doc, writer, typeFile, row, srcAsDest, existDir);
                            bkr = true;
                            break;
                        }
                        i++;
                    }
                }
            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void cleanButtonClick(object sender, EventArgs e)
        {
            try
            {
                bool srcAsDest = srcAsDestCheckBox.Checked;
                bool existDir = false;
                bool executeClean = false;
                if (!srcAsDest)
                {
                    DialogResult result = SaveFolderDialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        existDir = Directory.Exists(SaveFolderDialog.SelectedPath + "\\src");
                    }
                }
                if (srcAsDest || existDir)
                {
                    DialogResult result2 = MessageBox.Show("This process will overwrite the checked files in the path selected.\r\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result2 == DialogResult.Yes)
                    {
                        executeClean = true;
                    }
                }
                if (executeClean || !(srcAsDest || existDir || executeClean))
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
                                    cleanButtonProcess(sender, e, i, matchLine, bkr, doc, writer, row, pathCell, "ReportType", "reportTypes", srcAsDest, existDir);
                                }
                                else if (pathCell.Contains("\\permissionsets\\"))
                                {
                                    cleanButtonProcess(sender, e, i, matchLine, bkr, doc, writer, row, pathCell, "PermissionSet", "permissionsets", srcAsDest, existDir);
                                }
                                else if (pathCell.Contains("\\reports\\"))
                                {
                                    cleanButtonProcess(sender, e, i, matchLine, bkr, doc, writer, row, pathCell, "Report", "report", srcAsDest, existDir);
                                }
                                else if (pathCell.Contains("\\profiles\\"))
                                {
                                    cleanButtonProcess(sender, e, i, matchLine, bkr, doc, writer, row, pathCell, "Profile", "profiles", srcAsDest, existDir);
                                }
                                else if (pathCell.Contains("\\objectTranslations\\"))
                                {
                                    cleanButtonProcess(sender, e, i, matchLine, bkr, doc, writer, row, pathCell, "CustomObjectTranslation", "objectTranslations", srcAsDest, existDir);
                                }
                            }
                        }
                        Cursor.Current = Cursors.Default;
                        if (chkboxtrue)
                        {
                            MessageBox.Show("Task Completed!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            previewOn = false;
                        }
                        else
                        {
                            MessageBox.Show("There was no row checked", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void selectAllButtonClick(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (DataGridViewRow dgv in MainDGV.Rows)
                {
                    dgv.Cells[0].Value = true;
                }
                Cursor.Current = Cursors.Default;

            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void unselectAllButtonClick(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (DataGridViewRow dgv in MainDGV.Rows)
                {
                    dgv.Cells[0].Value = false;
                }
                Cursor.Current = Cursors.Default;
            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void loadPathButtonClick(object sender, EventArgs e)
        {
            try
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
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void selectMatchedButtonClick(object sender, EventArgs e)
        {
            try
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
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AboutForm about = new AboutForm();
                about.ShowDialog();
            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void previewDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && previewOn)
                {
                    DataGridViewSelectedRowCollection rowPrev = MainDGV.SelectedRows;
                    if (rowPrev.Count != 1)
                    {
                        MessageBox.Show("Please select only one row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                else if (!previewOn)
                {
                    MessageBox.Show("Please perform a new search to re-enable the preview.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void mainApp_Load(object sender, EventArgs e)
        {
            try
            {
                Text = "F.O.C.A. v1.0";
                PathTextBox.Text = Properties.Settings.Default.PathReminder;
                timer1 = new Timer();
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Interval = 10; // in miliseconds
                timer1.Start();
                try
                {
                    WebClient download = new WebClient();
                    string orig = download.DownloadString("https://raw.githubusercontent.com/fabriziodandrea/myFOCA/master/Field%20Obliteration%20Clean%20Automation/Field%20Obliteration%20Clean%20Automation/Form1.cs");
                    if (!orig.Contains(Text))
                    {
                        DialogResult result = MessageBox.Show("There is a new version available!\nDownload it now?", "Good News", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("https://myoffice.accenture.com/personal/f_dandrea_lopez_accenture_com/_layouts/15/guestaccess.aspx?guestaccesstoken=boDb5BRdrnYTXnW0YG9KJPEY1PQ9aZ3NE2KlGFiCLz4%3d&docid=2_05fd3fb8b40c44299bae36dfe8fed05f7&rev=1");
                            Close();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Please verify your internet connection\n there might be a new version available.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void dataGridView1_Click_1(object sender, EventArgs e)
        {
            try
            {
                MainDGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private Timer timer1;

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                MainDGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
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
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void resizeColumns()
        {
            try
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
            catch
            {
                MessageBox.Show("There was an error in the execution, please restart the app and try again.\nIf the issue persist please reach the dev.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

    }

}
