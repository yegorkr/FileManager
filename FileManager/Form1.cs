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
using System.Collections;

namespace test__
{
    public partial class Form1 : Form 
    {
        int createFlag = 0;
        System.IO.StreamWriter log = new System.IO.StreamWriter("C:\\Help\\log.txt");
        string renameContainer = "";  // used for renaming
        string nameTrans = "";  // transfers new folder name from cpyAll
        int cpyAllDepth = 0; // depth of cpyAll recursion
        int cutFlag = 0;  // used to distinguish between copying and cutting
        public Form1()
        {
            InitializeComponent();
            PopulateTree1();
            PopulateTree2();
            listView1.Items.Add("For help press F1");
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            FormTextEditor F1 = new FormTextEditor("C:\\Help\\F1.txt");
            //F1.Show();
            log.WriteLine("{0} Opened C:\\Help\\F1.txt", DateTime.Now);
        }
        private void copyAll(string dir, DirectoryInfo target)  // recursive folder copying
        {
            try
            {
                ++cpyAllDepth;
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                string dirName = dir.Substring(dir.LastIndexOf('\\') + 1);
                if (DialogResult.Yes == (DialogResult)MessageBox.Show("File already exists, rewrite?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) ;
                else
                    while (Directory.Exists(target.FullName + '\\' + dirName))
                {
                    int i = 1;
                    dirName += i.ToString();
                    ++i;
                }
                target.CreateSubdirectory(dirName);
                foreach (FileInfo file in dirInfo.GetFiles())
                    File.Copy(file.FullName, target.FullName + '\\' + dirName + '\\' + file.Name, true);
                foreach (DirectoryInfo folder in dirInfo.GetDirectories())
                {
                    DirectoryInfo newTarget = new DirectoryInfo(target.FullName + '\\' + dirName);
                    string name1 = folder.FullName.Substring(folder.FullName.LastIndexOf('\\') + 1);
                    string name2 = newTarget.FullName.Substring(newTarget.FullName.LastIndexOf('\\') + 1);
                    if (name1 != name2)
                        copyAll(folder.FullName, newTarget);
                }
                if (cpyAllDepth == 1) nameTrans = target.FullName + '\\' + dirName;
            }
            catch (Exception) { }
        }
        DirectoryInfo currentFolder1 = new DirectoryInfo("C:\\");
        DirectoryInfo currentFolder2 = new DirectoryInfo("C:\\");
        DirectoryInfo cpyDir = new DirectoryInfo("C:\\");
        List<string> cpyFiles = new List<string>();  // contains path to copied files
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
        private void PopulateTree1()
        {
            TreeNode root;
            string[] drives = Environment.GetLogicalDrives();
            DirectoryInfo infoRoot = new DirectoryInfo(@"C:\\Program Files (x86)");
            infoRoot = infoRoot.Parent;
            if (infoRoot.Exists)
            {
                treeView1.Nodes.Clear();
                root = new TreeNode(infoRoot.Name);
                root.Tag = infoRoot;
                GetDir(infoRoot.GetDirectories(), root);
                treeView1.Nodes.Add(root);
            }
        }
        private void PopulateTree2()
        {
            TreeNode root;
            string[] drives = Environment.GetLogicalDrives();
            DirectoryInfo infoRoot = new DirectoryInfo(@"C:\\Program Files (x86)");
            infoRoot = infoRoot.Parent;
            if (infoRoot.Exists)
            {
                treeView2.Nodes.Clear();
                root = new TreeNode(infoRoot.Name);
                root.Tag = infoRoot;
                GetDir(infoRoot.GetDirectories(), root);
                treeView2.Nodes.Add(root);
            }
        }
        private void GetDir(DirectoryInfo[] subDirs, TreeNode targetNode)
        {
            try
            {
                TreeNode nodeA;
                DirectoryInfo[] subSubDirs;
                foreach (DirectoryInfo subdir in subDirs)
                {
                    nodeA = new TreeNode(subdir.Name, 0, 0);
                    nodeA.Tag = subdir;
                    nodeA.ImageIndex = 0;
                    subSubDirs = subdir.GetDirectories();
                    if (subSubDirs.Length != 0)
                        GetDir(subSubDirs, nodeA);
                    targetNode.Nodes.Add(nodeA);
                }
            }
            catch (Exception) { }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            listView1.Items.Clear();
            DirectoryInfo root = new DirectoryInfo(@"C:\\Program Files (x86)");
            root = root.Parent;
            if (selectedNode.FullPath == "C:\\")
                selectedNode.Tag = root;
            DirectoryInfo nodeDirInfo = (DirectoryInfo)selectedNode.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                item.ImageIndex = 0;
                listView1.Items.Add(item);
            }
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "File"),
                    new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                item.ImageIndex = 1;
                listView1.Items.Add(item);
            }
            currentFolder1 = nodeDirInfo;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            listView2.Items.Clear();
            DirectoryInfo root = new DirectoryInfo(@"C:\\Program Files (x86)");
            root = root.Parent;
            if (selectedNode.FullPath == "C:\\")
                selectedNode.Tag = root;
            DirectoryInfo nodeDirInfo = (DirectoryInfo)selectedNode.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                item.ImageIndex = 0;
                listView2.Items.Add(item);
            }
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "File"),
                    new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                item.ImageIndex = 1;
                listView2.Items.Add(item);
            }
            currentFolder2 = nodeDirInfo;
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void showDir(string path, int l) // refresh window N l (1 or 2)
        {
            DirectoryInfo fldr = new DirectoryInfo(path);
            if (l == 1)
            {
                listView1.Items.Clear();
                foreach (DirectoryInfo dir in fldr.GetDirectories())
                {
                    ListViewItem item = new ListViewItem(dir.Name, 0);
                    ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[]
                    {
                        new ListViewItem.ListViewSubItem(item, "Directory"),
                        new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                    };
                    item.SubItems.AddRange(subItems);
                    item.ImageIndex = 0;
                    listView1.Items.Add(item);
                }
                foreach (FileInfo file in fldr.GetFiles())
                {
                    ListViewItem item = new ListViewItem(file.Name, 1);
                    ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[]
                    {
                        new ListViewItem.ListViewSubItem(item, "File"),
                        new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())
                    };
                    item.SubItems.AddRange(subItems);
                    item.ImageIndex = 1;
                    listView1.Items.Add(item);
                }
                currentFolder1 = fldr;
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            else
            if (l == 2)
            {
                listView2.Items.Clear();
                foreach (DirectoryInfo dir in fldr.GetDirectories())
                {
                    ListViewItem item = new ListViewItem(dir.Name, 0);
                    ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[]
                    {
                        new ListViewItem.ListViewSubItem(item, "Directory"),
                        new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                    };
                    item.SubItems.AddRange(subItems);
                    item.ImageIndex = 0;
                    listView2.Items.Add(item);
                }
                foreach (FileInfo file in fldr.GetFiles())
                {
                    ListViewItem item = new ListViewItem(file.Name, 1);
                    ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[]
                    {
                        new ListViewItem.ListViewSubItem(item, "File"),
                        new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())
                    };
                    item.SubItems.AddRange(subItems);
                    item.ImageIndex = 1;
                    listView2.Items.Add(item);
                }
                currentFolder2 = fldr;
                listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            string alt = listView1.FocusedItem.Text;
            int showFlag = 0;
            DirectoryInfo listDir = new DirectoryInfo("C:\\");
            if (Directory.Exists(currentFolder1.FullName + '\\' + alt)) listDir = new DirectoryInfo(currentFolder1.FullName + '\\' + alt);
            else
            {
                listDir = new DirectoryInfo(alt.Remove(alt.LastIndexOf('\\'), alt.Substring(alt.LastIndexOf('\\')).Length));
                showFlag = 1;
            }
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            listView1.Items.Clear();
            foreach (DirectoryInfo dir in listDir.GetDirectories())
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                item.ImageIndex = 0;
                listView1.Items.Add(item);
            }
            foreach (FileInfo file in listDir.GetFiles())
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "File"),
                    new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                item.ImageIndex = 1;
                listView1.Items.Add(item);
                if (showFlag == 1) if (item.Text == alt.Substring(alt.LastIndexOf('\\') + 1)) item.Selected = true;
            }
            currentFolder1 = listDir;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            string alt = listView2.FocusedItem.Text;
            DirectoryInfo listDir = new DirectoryInfo(currentFolder2.FullName + '\\' + alt);
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            listView2.Items.Clear();
            foreach (DirectoryInfo dir in listDir.GetDirectories())
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "Directory"),
                    new ListViewItem.ListViewSubItem(item, dir.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                item.ImageIndex = 0;
                listView2.Items.Add(item);
            }
            foreach (FileInfo file in listDir.GetFiles())
            {
                item = new ListViewItem(file.Name, 1);
                subItems = new ListViewItem.ListViewSubItem[]
                {
                    new ListViewItem.ListViewSubItem(item, "File"),
                    new ListViewItem.ListViewSubItem(item, file.LastAccessTime.ToShortDateString())
                };
                item.SubItems.AddRange(subItems);
                item.ImageIndex = 1;
                listView2.Items.Add(item);
            }
            currentFolder2 = listDir;
            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) contextMenuStrip1.Show();
        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) contextMenuStrip1.Show();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Focused)
            {
                cpyFiles.Clear();
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    cpyDir = currentFolder1;
                    if (cpyDir.FullName != "C:\\") cpyFiles.Add(cpyDir.FullName + "\\" + item.Text);
                    else cpyFiles.Add(cpyDir.FullName + item.Text);
                }
            }
            else if (listView2.Focused)
            {
                cpyFiles.Clear();
                foreach (ListViewItem item in listView2.SelectedItems)
                {
                    cpyDir = currentFolder2;
                    if (cpyDir.FullName != "C:\\") cpyFiles.Add(cpyDir.FullName + "\\" + item.Text);
                    else cpyFiles.Add(cpyDir.FullName + item.Text);
                }
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Focused)
            {
                DirectoryInfo targetDir = currentFolder1;
                foreach(string item in cpyFiles)
                {
                    string itemName = item.Substring(item.LastIndexOf('\\') + 1);
                    string itemFolder = item.Remove(item.LastIndexOf('\\') + 1, itemName.Length);
                    try
                    {
                        if (File.Exists(item))
                        {
                            int i = 1; string temp = targetDir.FullName + "\\" + itemName;
                            string ext = temp.Substring(temp.LastIndexOf('.') + 1);
                            if(File.Exists(temp)) if (DialogResult.Yes == (DialogResult)MessageBox.Show("File already exists, rewrite?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) ;
                            else
                                while (File.Exists(temp))
                            {
                                temp = temp.Remove(temp.LastIndexOf('.'), ext.Length + 1);
                                temp += i.ToString();
                                ++i;
                                temp += "." + ext;
                            }
                            File.Copy(item, temp, true);
                            listView1.Items.Add(temp.Substring(temp.LastIndexOf('\\') + 1), 1);
                            showDir(currentFolder1.FullName, 1);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                            log.WriteLine("{1} Copied {0} to {2}", item, DateTime.Now, temp);
                        }
                        else
                        {
                            copyAll(item, targetDir);
                            cpyAllDepth = 0;
                            listView1.Items.Add(nameTrans.Substring(nameTrans.LastIndexOf('\\') + 1), 0);
                            showDir(currentFolder1.FullName, 1);
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                            log.WriteLine("{1} Copied {0} to {2}", item, DateTime.Now, targetDir);
                        }
                    }
                    catch (Exception) { }
                }
                if (cutFlag == 1)
                {
                    cutFlag = 0;
                    foreach (string item in cpyFiles)
                    {
                        if (Directory.Exists(item))
                        {
                            System.IO.Directory.Delete(item);
                        }
                        else if (File.Exists(item))
                        {
                            System.IO.File.Delete(item);
                        }
                    }
                    cpyFiles.Clear();
                }
            }
            if (listView2.Focused)
            {
                DirectoryInfo targetDir = currentFolder2;
                foreach (string item in cpyFiles)
                {
                    string itemName = item.Substring(item.LastIndexOf('\\') + 1);
                    string itemFolder = item.Remove(item.LastIndexOf('\\') + 1, itemName.Length);
                    try
                    {
                        if (File.Exists(item))
                        {
                            int i = 1; string temp = targetDir.FullName + "\\" + itemName;
                            string ext = temp.Substring(temp.LastIndexOf('.') + 1);
                            if (File.Exists(temp)) if (DialogResult.Yes == (DialogResult)MessageBox.Show("File already exists, rewrite?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) ;
                            else
                                while (File.Exists(temp))
                            {
                                temp = temp.Remove(temp.LastIndexOf('.'), ext.Length + 1);
                                temp += i.ToString();
                                ++i;
                                temp += "." + ext;
                            }
                            File.Copy(item, temp, true);
                            log.WriteLine("{1} Copied {0} to {2}", item, DateTime.Now, temp);
                            listView2.Items.Add(temp.Substring(temp.LastIndexOf('\\') + 1), 1);
                            showDir(currentFolder2.FullName, 2);
                            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        }
                        else
                        {
                            copyAll(item, targetDir);
                            cpyAllDepth = 0;
                            listView2.Items.Add(nameTrans.Substring(nameTrans.LastIndexOf('\\') + 1), 0);
                            showDir(currentFolder2.FullName, 2);
                            listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                            log.WriteLine("{1} Copied {0} to {2}", item, DateTime.Now, targetDir);
                        }
                    }
                    catch (Exception) { }
                }
                if (cutFlag == 1)
                {
                    cutFlag = 0;
                    foreach (string item in cpyFiles)
                    {
                        if (Directory.Exists(item))
                        {
                            System.IO.Directory.Delete(item);
                        }
                        else if (File.Exists(item))
                        {
                            System.IO.File.Delete(item);
                        }
                    }
                    cpyFiles.Clear();
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Focused)
            {
                if(MessageBox.Show("Delete?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                        if (Directory.Exists(currentFolder1.FullName + '\\' + item.Text))
                        {
                            DirectoryInfo dir = new DirectoryInfo(currentFolder1.FullName + '\\' + item.Text);
                            int crutch = 0;
                            foreach (FileInfo file in dir.GetFiles()) crutch = 1;
                            foreach (DirectoryInfo folder in dir.GetDirectories()) crutch = 1;
                            if (crutch == 1) if (DialogResult.Yes == (DialogResult)MessageBox.Show("Folder is not empty, delete?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                            deleteAll(dir);
                            showDir(currentFolder1.FullName, 1);
                            log.WriteLine("{1} Deleted {0}", dir.FullName, DateTime.Now);
                        }
                        else if (File.Exists(currentFolder1.FullName + '\\' + item.Text))
                        {
                            System.IO.File.Delete(currentFolder1.FullName + '\\' + item.Text);
                            listView1.Items.Remove(item);
                            log.WriteLine("{1} Deleted {0}", currentFolder1.FullName + '\\' + item.Text, DateTime.Now);
                        }
                    }
            }
            else if (listView2.Focused)
            {
                if (MessageBox.Show("Delete?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    foreach (ListViewItem item in listView2.SelectedItems)
                    {
                        if (Directory.Exists(currentFolder2.FullName + '\\' + item.Text))
                        {
                            DirectoryInfo dir = new DirectoryInfo(currentFolder2.FullName + '\\' + item.Text);
                            deleteAll(dir);
                            listView2.Items.Remove(item);
                            log.WriteLine("{1} Deleted {0}", dir.FullName, DateTime.Now);
                        }
                        else if (File.Exists(currentFolder2.FullName + '\\' + item.Text))
                        {
                            System.IO.File.Delete(currentFolder2.FullName + '\\' + item.Text);
                            listView2.Items.Remove(item);
                            log.WriteLine("{1} Deleted {0}", currentFolder2.FullName + '\\' + item.Text, DateTime.Now);
                        }
                    }
            }
        }

        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Focused)
            {
                cpyFiles.Clear();
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    cpyDir = currentFolder1;
                    if (cpyDir.FullName != "C:\\") cpyFiles.Add(cpyDir.FullName + "\\" + item.Text);
                    else cpyFiles.Add(cpyDir.FullName + item.Text);
                }
                cutFlag = 1;
            }
            else if (listView2.Focused)
            {
                cpyFiles.Clear();
                foreach (ListViewItem item in listView2.SelectedItems)
                {
                    cpyDir = currentFolder2;
                    if (cpyDir.FullName != "C:\\") cpyFiles.Add(cpyDir.FullName + "\\" + item.Text);
                    else cpyFiles.Add(cpyDir.FullName + item.Text);
                }
                cutFlag = 1;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Focused)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    string fileToOpen = currentFolder1.FullName.ToString() + "\\" + item.Text;
                    FormTextEditor textProc = new FormTextEditor(fileToOpen);
                    if (File.Exists(fileToOpen) && (fileToOpen.Substring(fileToOpen.Length - 3) == "txt" || fileToOpen.Substring(fileToOpen.Length - 4) == "html"))
                    {
                        textProc.Show();
                        log.WriteLine("{1} Opening file {0}", fileToOpen, DateTime.Now);
                    }
                    FormTable tableProc = new FormTable(fileToOpen);
                    if (File.Exists(fileToOpen) && (fileToOpen.Substring(fileToOpen.Length - 3) == "xml"))
                    {
                        tableProc.Show();
                        log.WriteLine("{1} Opening file {0}", fileToOpen, DateTime.Now);
                    }
                }
            }
            else if (listView2.Focused)
            {
                foreach (ListViewItem item in listView2.SelectedItems)
                {
                    string fileToOpen = currentFolder2.FullName.ToString() + "\\" + item.Text;
                    FormTextEditor textProc = new FormTextEditor(fileToOpen);
                    if (File.Exists(fileToOpen) && (fileToOpen.Substring(fileToOpen.Length - 3) == "txt" || fileToOpen.Substring(fileToOpen.Length - 4) == "html"))
                    {
                        textProc.Show();
                        log.WriteLine("{1} Opening file {0}", fileToOpen, DateTime.Now);
                    }
                    FormTable tableProc = new FormTable(fileToOpen);
                    if (File.Exists(fileToOpen) && (fileToOpen.Substring(fileToOpen.Length - 3) == "xml"))
                    {
                        tableProc.Show();
                        log.WriteLine("{1} Opening file {0}", fileToOpen, DateTime.Now);
                    }
                }
            }
        }
        private List<FileInfo> HTMLsFound = new List<FileInfo>();
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSearch dialog = new FormSearch();
            log.WriteLine("{0} Initiating search", DateTime.Now);
            dialog.ShowDialog();
            if (FormSearch.headers != "")
            {
                int flag = 0;
                string name = FormSearch.headers;
                List<string> headers = new List<string>();
                List<string> headersFound = new List<string>();
                List<FileInfo> results = new List<FileInfo>();
                HTMLsFound.Clear();
                DirectoryInfo searchRoot = new DirectoryInfo("C:\\");
                if (listView1.Focused) searchRoot = currentFolder1;
                else if (listView2.Focused) searchRoot = currentFolder2;
                string temp = "";
                for(int i = 0; i < name.Length; ++i)
                {
                    if (name[i] != ',') temp += name[i];
                    else
                    {
                        headers.Add(temp);
                        temp = "";
                        ++i;
                    }
                }
                headers.Add(temp);
                HTML(searchRoot);
                try
                {
                    foreach (FileInfo file in HTMLsFound)
                    {
                        headersFound.Clear();
                        string text = System.IO.File.ReadAllText(file.FullName);
                        string head = "";
                        for (int i = 0; i < text.Length; ++i)
                        {
                            if (i + 4 < text.Length)
                                if (text.Substring(i, 4) == "<h1>" || text.Substring(i, 4) == "<h2>" || text.Substring(i, 4) == "<h3>" || text.Substring(i, 4) == "<h4>" || text.Substring(i, 4) == "<h5>" || text.Substring(i, 4) == "<h6>")
                                {
                                    i += 4;
                                    flag = 1;
                                }
                            if (text[i] == '<')
                            {
                                i += 5;
                                headersFound.Add(head);
                                flag = 0;
                                head = "";
                            }
                            if (flag == 1) head += text[i];
                        }
                        int check = 0;
                        for (int i = 0; i < headers.Count; ++i)
                            for (int j = 0; j < headersFound.Count; ++j)
                                if (headers[i] == headersFound[j]) ++check;
                        if (check == headers.Count) results.Add(file);
                    }
                }
                catch(Exception) { }
                listView1.Items.Clear();
                foreach (FileInfo file in results) listView1.Items.Add(file.FullName, 2);
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
               if(results.Count != 0) currentFolder1 = new DirectoryInfo(results[0].FullName.Substring(results[0].FullName.LastIndexOf('\\') + 1));
            }        
        }
        private void HTML (DirectoryInfo searchRoot)
        {
            try
            {
                foreach (FileInfo file in searchRoot.GetFiles())
                {
                    string name = file.FullName;
                    name = name.Substring(name.LastIndexOf('\\') + 1);
                    if (name.Substring(name.Length - 4) == "html") HTMLsFound.Add(file);
                }
                foreach (DirectoryInfo dir in searchRoot.GetDirectories()) HTML(dir);
            }
            catch(Exception) { }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormMerge merge = new FormMerge();
            log.WriteLine("{0} Initiating merge", DateTime.Now);
            merge.ShowDialog();
            string HTML1 = FormMerge.HTML1;
            string HTML2 = FormMerge.HTML2;
            if (File.Exists(HTML1) && File.Exists(HTML2))
            {
                string text1 = System.IO.File.ReadAllText(HTML1);
                string text2 = System.IO.File.ReadAllText(HTML2);
                DirectoryInfo folder1 = new DirectoryInfo(HTML1.Substring(0, HTML1.LastIndexOf('\\') + 1));
                int i = 0;
                string img = "";
                List<FileInfo> HTML1imgs = new List<FileInfo>();
                FileInfo imgFile = new FileInfo("C\\Test\\1\\1.txt");
                text1 += text2;
                while (i < text1.Length)
                {
                    i = text1.IndexOf("<img src=");
                    if (i > -1)
                    {
                        text1 = text1.Remove(i, 9);
                        ++i;
                        while (text1[i] != '\"')
                        {
                            img += text1[i];
                            ++i;
                        }
                        imgFile = new FileInfo(img);
                        HTML1imgs.Add(imgFile);
                        img = "";
                    }
                    else break;
                }
                string HTMLfldr = HTML1.Substring(0, HTML1.LastIndexOf('.')) + " img";
                List<string> HTMLcpy = new List<string>();
                foreach (FileInfo file in HTML1imgs)
                    HTMLcpy.Add(file.FullName);
                folder1 = folder1.CreateSubdirectory("HTML img");
                log.WriteLine("{0} Created folder {1}", DateTime.Now, folder1.FullName + "\\" + "HTML img");
                foreach (FileInfo file in HTML1imgs)
                    if (file.Exists)
                    {
                        string temp = file.FullName;
                        file.MoveTo(folder1.FullName + file.FullName.Substring(file.FullName.LastIndexOf('\\')));
                        log.WriteLine("{0} Moved {2} to {1}", DateTime.Now, folder1.FullName + file.FullName.Substring(file.FullName.LastIndexOf('\\')), file.FullName);
                        text1 = File.ReadAllText(HTML1) + File.ReadAllText(HTML2);
                        text1.Replace(temp, file.FullName);
                    }
                string fileName = folder1.Parent.FullName + "\\" + "Merge HTML.html";
                System.IO.StreamWriter mFile = new StreamWriter(fileName);
                foreach (string file in HTMLcpy)
                {
                    string nm = file.Substring(file.LastIndexOf('\\') + 1);
                    string newFile = folder1.FullName + "\\" + nm;
                    text1 = text1.Replace(file, newFile);
                }
                mFile.Write(text1);
                mFile.Close();
                showDir(currentFolder1.FullName, 1);
            }
        }
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Focused) showDir(currentFolder1.FullName, 1);
            if (listView2.Focused) showDir(currentFolder2.FullName, 2);
        }

        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.Focused)
            {
                if(currentFolder1.FullName != "C:\\") currentFolder1 = currentFolder1.Parent;
                showDir(currentFolder1.FullName, 1);
            }
            if (listView2.Focused)
            {
                if (currentFolder1.FullName != "C:\\") currentFolder2 = currentFolder2.Parent;
                showDir(currentFolder2.FullName, 2);
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)  // Create File
        {
            if(listView1.Focused)
            {
                string temp = "New File";
                string ext = ".txt";
                createFlag = 1;
                ListViewItem file = listView1.Items.Add(temp + ext, 1);
                file.BeginEdit();
            }
            if (listView2.Focused)
            {
                string temp = "New File";
                string ext = ".txt";
                createFlag = 2;
                ListViewItem file = listView2.Items.Add(temp + ext, 1);
                file.BeginEdit();
            }
        }

        private void directoryToolStripMenuItem_Click(object sender, EventArgs e)  // Create Folder
        {
            if (listView1.Focused)
            {
                string temp = "New Folder";
                createFlag = 3;
                ListViewItem file = listView1.Items.Add(temp, 0);
                file.BeginEdit();

            }
            if (listView2.Focused)
            {
                string temp = "New Folder";
                createFlag = 4;
                ListViewItem file = listView2.Items.Add(temp, 0);
                file.BeginEdit();
            }
        }
        
        private void deleteAll(DirectoryInfo dir)
        {
            foreach (FileInfo file in dir.GetFiles()) if (file.Exists) file.Delete();
            foreach (DirectoryInfo folder in dir.GetDirectories()) if (folder.Exists) deleteAll(folder);
            if (dir.Exists) dir.Delete();
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.Focused)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    renameContainer = currentFolder1.FullName + "\\" + listView1.SelectedItems[0].Text;
                    listView1.SelectedItems[0].BeginEdit();
                }
            }
            if(listView2.Focused)
            {
                if (listView2.SelectedItems.Count > 0)
                {
                    renameContainer = currentFolder1.FullName + "\\" + listView2.SelectedItems[0].Text;
                    listView2.SelectedItems[0].BeginEdit();
                }
            }
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if(createFlag == 0) try
            {
                DirectoryInfo dir = new DirectoryInfo(renameContainer);
                FileInfo file = new FileInfo(renameContainer);
                if (dir.Exists) dir.MoveTo(currentFolder1.FullName + "\\" + e.Label);
                if (file.Exists) file.MoveTo(currentFolder1.FullName + "\\" + e.Label);
                log.WriteLine("{1} Renamed {0} to {2}", renameContainer, DateTime.Now, currentFolder1.FullName + "\\" + e.Label);
            }
            catch(Exception)
            {
                e.CancelEdit = true;
                MessageBox.Show("Chosen name is inappropriate", "invalid name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                log.WriteLine("{0} Rename canceled", DateTime.Now);
            }
            else
                try
                {
                    string temp = e.Label;
                    string ext = "";
                    int i = 0;
                    if (createFlag == 1)
                    {
                        if (e.Label == null) temp = "New File.txt";
                        if (temp.LastIndexOf('.') != -1)
                        {
                            ext = temp.Substring(temp.LastIndexOf('.'));
                            temp = temp.Remove(temp.LastIndexOf('.'), ext.Length);
                        }
                        if (File.Exists(currentFolder1.FullName + "\\" + temp + ext))
                            if (DialogResult.Yes == (DialogResult)MessageBox.Show("File already exists, rewrite?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) ;
                            else
                                while (File.Exists(currentFolder1.FullName + "\\" + temp + ext))
                                {
                                    ++i;
                                    if (i > 1) temp = temp.Remove(temp.Length - 1);
                                    temp += i;
                                }
                        e.CancelEdit = true;
                        FileStream fs = File.Create(currentFolder1.FullName + "\\" + temp + ext);
                        fs.Close();
                        log.WriteLine("{0} Created {1}", DateTime.Now, currentFolder1.FullName + "\\" + temp + ext);
                        showDir(currentFolder1.FullName, 1);
                    }
                    if (e.Label == null) temp = "New Folder";
                    if (createFlag == 3)
                    {
                        if (e.Label == null) temp = "New Folder";
                        if (Directory.Exists(currentFolder1.FullName + "\\" + temp))
                            if (DialogResult.Yes == (DialogResult)MessageBox.Show("Folder already exists, merge?", "Folder already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) ;
                            else
                                while (Directory.Exists(currentFolder1.FullName + "\\" + temp))
                                {
                                    ++i;
                                    if (i > 1) temp = temp.Remove(temp.Length - 1);
                                    temp += i;
                                }
                        e.CancelEdit = true;
                        currentFolder1.CreateSubdirectory(temp);
                        log.WriteLine("{0} Created {1}", DateTime.Now, currentFolder1.FullName + "\\" + temp);
                        showDir(currentFolder1.FullName, 1);
                    }
                    createFlag = 0;
                }
                catch (Exception)
                {
                    e.CancelEdit = true;
                    MessageBox.Show("Chosen name is inappropriate", "invalid name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    log.WriteLine("{0} Rename canceled", DateTime.Now);
                    showDir(currentFolder1.FullName, 1);
                    createFlag = 0;
                }
        }

        private void listView2_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if(createFlag == 0) try
            {
                DirectoryInfo dir = new DirectoryInfo(renameContainer);
                FileInfo file = new FileInfo(renameContainer);
                if (dir.Exists) dir.MoveTo(currentFolder2.FullName + "\\" + e.Label);
                if (file.Exists) file.MoveTo(currentFolder2.FullName + "\\" + e.Label);
                log.WriteLine("{1} Renamed {0} to {2}", renameContainer, DateTime.Now, currentFolder2.FullName + "\\" + e.Label);
            }
            catch (Exception)
            {
                e.CancelEdit = true;
                MessageBox.Show("Chosen name is inappropriate", "invalid name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                log.WriteLine("{0} Rename canceled", DateTime.Now);
            }
            else
                try
                {
                    string temp = e.Label;
                    string ext = "";
                    int i = 0;
                    if (createFlag == 2)
                    {
                        if (e.Label == null) temp = "New File.txt";
                        if (temp.LastIndexOf('.') != -1)
                        {
                            ext = temp.Substring(temp.LastIndexOf('.'));
                            temp = temp.Remove(temp.LastIndexOf('.'), ext.Length);
                        }
                        if (File.Exists(currentFolder2.FullName + "\\" + temp + ext))
                            if (DialogResult.Yes == (DialogResult)MessageBox.Show("File already exists, rewrite?", "File already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) ;
                            else
                                while (File.Exists(currentFolder2.FullName + "\\" + temp + ext))
                                {
                                    ++i;
                                    if (i > 1) temp = temp.Remove(temp.Length - 1);
                                    temp += i;
                                }
                        e.CancelEdit = true;
                        FileStream fs1 = File.Create(currentFolder2.FullName + "\\" + temp + ext);
                        fs1.Close();
                        log.WriteLine("{0} Created {1}", DateTime.Now, currentFolder2.FullName + "\\" + temp + ext);
                        showDir(currentFolder2.FullName, 2);
                    }
                    if (createFlag == 4)
                    {
                        if (e.Label == null) temp = "New Folder";
                        if (Directory.Exists(currentFolder2.FullName + "\\" + temp))
                            if (DialogResult.Yes == (DialogResult)MessageBox.Show("Folder already exists, merge?", "Folder already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) ;
                            else
                                while (Directory.Exists(currentFolder2.FullName + "\\" + temp))
                                {
                                    ++i;
                                    if (i > 1) temp = temp.Remove(temp.Length - 1);
                                    temp += i;
                                }
                        e.CancelEdit = true;
                        currentFolder2.CreateSubdirectory(temp);
                        log.WriteLine("{0} Created {1}", DateTime.Now, currentFolder2.FullName + "\\" + temp);
                        showDir(currentFolder2.FullName, 2);
                    }
                    createFlag = 0;
                }
                catch (Exception)
                {
                    e.CancelEdit = true;
                    MessageBox.Show("Chosen name is inappropriate", "invalid name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    log.WriteLine("{0} Rename canceled", DateTime.Now);
                    showDir(currentFolder2.FullName, 1);
                    createFlag = 0;
                }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTextEditor F1 = new FormTextEditor("C:\\Help\\F1.txt");
            F1.Show();
            log.WriteLine("{0} Opened C:\\Help\\F1.txt", DateTime.Now);
        }

        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTextEditor F4 = new FormTextEditor("C:\\Help\\F4.txt");
            F4.Show();
            log.WriteLine("{0} Opened C:\\Help\\F4.txt", DateTime.Now);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.WriteLine("{0} Closing File Manager", DateTime.Now);
            log.Close();
        }

        private void tableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.File.Copy("C:\\Help\\Templ.xml", "C:\\Help\\New File.xml", true);
            log.WriteLine("{0} Initiating table edditor", DateTime.Now);
            FormTable f = new FormTable("C:\\Help\\New File.xml");
            f.Show();
        }
    }
}