using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml;
using System.IO;

namespace test__
{
    public partial class FormTable : Form
    {
        int depth = 0;
        public static string Saveas;
        string[][] M;
        List<int> row = new List<int>();
        List<int> clmn = new List<int>();
        const double ERR = -63889721.45699917;
        string file;
        List<string> Z = new List<string>();
        coord m = new coord();
        List<coord> toCopy = new List<coord>();
        public FormTable(string fileToOpen)
        {
            InitializeComponent();
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            DataSet ds = new DataSet();
            dataGridView1.DataMember = "Table1";
            ds.ReadXml(fileToOpen);
            dataGridView1.DataSource = ds;
            file = fileToOpen;
            for (int i = 0; i < dataGridView1.RowCount; ++i)
                for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; ++j)
                    if (dataGridView1.Rows[i].Cells[j].Value == null)
                        Z.Add("");
                    else
                        Z.Add(dataGridView1.Rows[i].Cells[j].Value.ToString());
            M = new string[100][];
            for (int i = 0; i < 100; ++i)
                M[i] = new string[100];
            for (int i = 0; i < 100; ++i)
                for (int j = 0; j < 100; ++j)
                    M[i][j] = "";
            this.Text = fileToOpen;
        }

        private void addColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewColumn temp = new DataGridViewColumn();
            temp.CellTemplate = new DataGridViewTextBoxCell();
            if (dataGridView1.SelectedCells.Count == 0 || dataGridView1.ColumnCount == 0)
                dataGridView1.Columns.Add("A", "A");
            else
                dataGridView1.Columns.Insert(dataGridView1.SelectedCells[dataGridView1.SelectedCells.Count - 1].ColumnIndex, temp);
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) contextMenuStrip1.Show();
        }

        private string NameAdd(string s)
        {
            if (s == "Z") return "AA";
            if (Char.IsLetter((char)((int)s.Last() + 1)))
                return s.Remove(s.Length - 1) + (char)((int)s.Last() + 1);
            else return NameAdd(s.Remove(s.Length - 1)) + "A";
        }

        private int NameVal(string s)
        {
            int r = 0;
            for (int i = s.Length; i > 0; --i)
                if ((int)s[i - 1] > 90 || (int)s[i - 1] < 65)
                    return -1;
                else
                    r += (int)Math.Pow(26, s.Length - i) * ((int)s[i - 1] - 64);
            return r;
        }

        private void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)dataGridView1.DataSource;
            DataTable table = ds.Tables[0];
            DataRow newRow = table.NewRow();
            if (dataGridView1.ColumnCount == 0)
                dataGridView1.Columns.Add("A", "A");
            if (dataGridView1.SelectedCells.Count == 0)
                table.Rows.Add(newRow);
            else
                table.Rows.InsertAt(newRow, dataGridView1.SelectedCells[dataGridView1.SelectedCells.Count - 1].RowIndex);
            for (int i = 0; i < ds.Tables[0].Rows.Count; ++i)
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            for (int i = 0; i < dataGridView1.RowCount; ++i)
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();

        }


        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            int cntr = 0;
            if (e.Column.Index == 0)
            {
                dataGridView1.Columns[e.Column.Index].Name = ("A");
                dataGridView1.Columns[e.Column.Index].HeaderText = ("A");
                ++cntr;
            }
            else
            {
                dataGridView1.Columns[e.Column.Index].Name = NameAdd(dataGridView1.Columns[e.Column.Index - 1].Name);
                dataGridView1.Columns[e.Column.Index].HeaderText = NameAdd(dataGridView1.Columns[e.Column.Index - 1].HeaderText);
                ++cntr;
            }
            for (int i = e.Column.Index + cntr; i <= dataGridView1.Columns[dataGridView1.Columns.Count - 1].Index; ++i)
            {
                dataGridView1.Columns[i].Name = NameAdd(dataGridView1.Columns[i - 1].Name);
                dataGridView1.Columns[i].HeaderText = NameAdd(dataGridView1.Columns[i - 1].HeaderText);
            }
        }

        private void deleteColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.SelectedCells.Count;
            if (dataGridView1.SelectedCells.Count == 0)
                dataGridView1.Columns.RemoveAt(dataGridView1.ColumnCount - 1);
            else if (dataGridView1.SelectedCells.Count == 1)
                dataGridView1.Columns.RemoveAt(dataGridView1.SelectedCells[0].OwningColumn.Index);
            else
            {
                for (int i = 0; i < dataGridView1.SelectedCells.Count; ++i)
                    if (dataGridView1.SelectedCells.Count > 0)
                        if (dataGridView1.SelectedCells[i].OwningColumn != null && dataGridView1.SelectedCells.Count != 1)
                        {
                            dataGridView1.Columns.RemoveAt(dataGridView1.SelectedCells[i--].OwningColumn.Index);
                            --n;
                        }
                if (n == 1 && dataGridView1.SelectedCells.Count == 1)
                    dataGridView1.Columns.RemoveAt(dataGridView1.SelectedCells[0].OwningColumn.Index);
            }
            if (dataGridView1.ColumnCount > 0) dataGridView1.Columns[0].Name = "A";
            if (dataGridView1.ColumnCount > 0) dataGridView1.Columns[0].HeaderText = "A";
            if(dataGridView1.ColumnCount > 0)
                for (int i = 1; i <= dataGridView1.Columns[dataGridView1.Columns.Count - 1].Index; ++i)
                {
                    dataGridView1.Columns[i].Name = NameAdd(dataGridView1.Columns[i - 1].Name);
                    dataGridView1.Columns[i].HeaderText = NameAdd(dataGridView1.Columns[i - 1].HeaderText);
                }
        }

        private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int n = dataGridView1.SelectedCells.Count;
                if (dataGridView1.SelectedCells.Count == 0)
                    dataGridView1.Rows.RemoveAt(dataGridView1.RowCount - 2);
                else
                        if (dataGridView1.SelectedCells.Count == 1)
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].OwningRow.Index);
                else
                {
                    for (int i = 0; i < dataGridView1.SelectedCells.Count; ++i)
                        if (dataGridView1.SelectedCells.Count > 0)
                            if (dataGridView1.SelectedCells[i].OwningRow != null && dataGridView1.SelectedCells[i].OwningRow.Index != dataGridView1.NewRowIndex && dataGridView1.SelectedCells.Count != 1)
                            {
                                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[i--].OwningRow.Index);
                                --n;
                            }
                    if (n == 1 && dataGridView1.SelectedCells.Count == 1)
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].OwningRow.Index);
                }
                if (dataGridView1.RowCount > 0) dataGridView1.Rows[0].HeaderCell.Value = "1";

                for (int i = 1; i < dataGridView1.RowCount; ++i)
                    dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            catch (Exception) { }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }
        private double Atom(string s)
        {
            int fl = 0;
            if (s == "")
                return 0;
            double j = s.IndexOf("inc");
            if(j != -1)
            {
                ++fl;
                s = s.Remove((int)j, 3);
                if (s.Length > 0) j = s.Substring((int)j).Remove(0, 1).IndexOf("inc");
                else j = -1;
            }
            j = s.IndexOf("dec");
            if (j != -1)
            {
                --fl;
                s = s.Remove((int)j, 3);
                if (s.Length > 0) j = s.Substring((int)j).Remove(0, 1).IndexOf("dec");
                else j = -1;
            }
            if (double.TryParse(s, out j))
                j = double.Parse(s);
            else
            {

                int i = 0;
                int temp1 = s.IndexOfAny("123456789".ToCharArray());
                int temp2;
                if (temp1 > -1)
                    temp2 = NameVal(s.Substring(0, temp1));
                else return ERR;
                if (int.TryParse(s.Substring(temp1), out i))
                    for(int l = 0; l  < row.Count - 1; ++l)
                if (temp2 == clmn[l] + 1 && int.Parse(s.Substring(temp1)) == row[l] + 1)
                    return ERR;
                if (temp1 != -1 && temp2 != -1)
                {
                    if (int.TryParse(s.Substring(temp1), out i))
                        temp1 = int.Parse(s.Substring(temp1));
                    else return ERR;
                    if (dataGridView1.RowCount < temp1 || dataGridView1.ColumnCount < temp2)
                        return ERR;
                    try
                    {
                        string temp;
                        row.Add(temp1 - 1);
                        clmn.Add(temp2 - 1);
                        if (M[temp1 - 1][temp2 - 1] != "")
                            return Atom(Parse(M[temp1 - 1][temp2 - 1]));
                        temp = Parse(dataGridView1.Rows[temp1 - 1].Cells[temp2 - 1].Value.ToString());
                        return double.Parse(temp);
                    }
                    catch (Exception) { return ERR; }
                }
                else return ERR;
            }
            j += fl;
            return j;
        }
        private string Parse(string s)
        {
            ++depth;
            if (s.IndexOf("ERR") != -1 && depth == 1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf(ERR.ToString()) != -1 && depth == 1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf("INF") != -1)
            {
                depth--;
                return "INF";
            }
            if (s.IndexOf("ERR") != -1)
            {
                --depth;
                return "ERR";
            }
            if (s.IndexOf(ERR.ToString()) != -1)
            {
                --depth;
                return "ERR";
            }
            int j = -1;
            int l; int t;
            s = s.Replace('-', '$');
            s = s.Replace(" ", "");
            do
            {
                j = s.LastIndexOf('(');
                if (j == -1) s = s.Trim(')');
                else
                if (s.Substring(j).IndexOf(')') != -1)
                    s = s.Substring(0, j) + Parse(s.Substring(j + 1, s.Substring(j + 1).IndexOf(')'))) + s.Substring(j + s.Substring(j).IndexOf(')')).Remove(0, 1);
                else s = s.Remove(j, 1);
            } while (j != -1);
            if (s.IndexOf("ERR") != -1 && depth == 1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf(ERR.ToString()) != -1 && depth == 1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf("INF") != -1)
            {
                depth--;
                return "INF";
            }
            if (s.IndexOf("ERR") != -1)
            {
                --depth;
                return "ERR";
            }
            if (s.IndexOf(ERR.ToString()) != -1)
            {
                --depth;
                return "ERR";
            }

            do
            {
                j = s.LastIndexOf("inc");
                t = s.Substring(j + 1).IndexOfAny("^+$*/%id".ToCharArray());
                int c = row.Count;
                if (j != -1)
                    if (t != -1)
                        s = s.Substring(0, j) + Atom(s.Substring(j, t + 1)).ToString() + s.Substring(j + t + 1);
                    else
                        s = s.Substring(0, j) + Atom(s.Substring(j)).ToString();
                while (c != row.Count)
                    row.RemoveAt(row.Count - 1);
            } while (j != -1);
            if (s.IndexOf("ERR") != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf(ERR.ToString()) != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf("INF") != -1)
            {
                depth--;
                return "INF";
            }
            if (s.IndexOf("ERR") != -1)
            {
                --depth;
                return "ERR";
            }
            if (s.IndexOf(ERR.ToString()) != -1)
            {
                --depth;
                return "ERR";
            }

            do
            {
                int c = row.Count;
                j = s.LastIndexOf("dec");
                t = s.Substring(j + 1).IndexOfAny("^+$*/%id".ToCharArray());
                if (j != -1)
                    if (t != -1)
                        s = s.Substring(0, j) + Atom(s.Substring(j, t + 1)).ToString() + s.Substring(j + t + 1);
                    else
                        s = s.Substring(0, j) + Atom(s.Substring(j)).ToString();
                while (c != row.Count)
                    row.RemoveAt(row.Count - 1);
            } while (j != -1);

            if (s.IndexOf("ERR") != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf(ERR.ToString()) != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf("INF") != -1)
            {
                depth--;
                return "INF";
            }
            if (s.IndexOf("ERR") != -1)
            {
                --depth;
                return "ERR";
            }
            if (s.IndexOf(ERR.ToString()) != -1)
            {
                --depth;
                return "ERR";
            }

            do
            {
                int c = row.Count;
                j = s.LastIndexOf("^");
                if (j != -1) t = s.Substring(j + 1).IndexOfAny("^+$*/%id".ToCharArray());
                else t = -1;
                if (j != -1) l = s.Substring(0, j).LastIndexOfAny("^+$*/%cv".ToCharArray());
                else l = -1;
                if (j != -1)
                    if (t != -1)
                        if (l != -1)
                            s = s.Substring(0, l + 1) + Math.Pow(Atom(s.Substring(l + 1, j - l - 1)), Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                        else
                            s = Math.Pow(Atom(s.Substring(0, j)), Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                    else
                        if (l != -1)
                        s = s.Substring(0, l + 1) + Math.Pow(Atom(s.Substring(l + 1, j - l - 1)), Atom(s.Substring(j + 1))).ToString();
                    else
                        s = Math.Pow(Atom(s.Substring(0, j)), Atom(s.Substring(j + 1))).ToString();
                while (c != row.Count)
                    row.RemoveAt(row.Count - 1);
            } while (j != -1);

            if (s.IndexOf("ERR") != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf(ERR.ToString()) != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf("INF") != -1)
            {
                depth--;
                return "INF";
            }
            if (s.IndexOf("ERR") != -1)
            {
                --depth;
                return "ERR";
            }
            if (s.IndexOf(ERR.ToString()) != -1)
            {
                --depth;
                return "ERR";
            }

            do
            {
                int c = row.Count;
                j = s.IndexOf("*");
                if (j != -1) t = s.Substring(j + 1).IndexOfAny("^+$*/%id".ToCharArray());
                else t = -1;
                if (j != -1) l = s.Substring(0, j).LastIndexOfAny("^+$*/%cv".ToCharArray());
                else l = -1;
                if (j != -1)
                    if (t != -1)
                        if (l != -1)
                            s = s.Substring(0, l + 1) + (Atom(s.Substring(l + 1, j - l - 1)) * Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                        else
                            s = (Atom(s.Substring(0, j)) * Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                    else
                        if (l != -1)
                        s = s.Substring(0, l + 1) + (Atom(s.Substring(l + 1, j - l - 1)) * Atom(s.Substring(j + 1))).ToString();
                    else
                        s = (Atom(s.Substring(0, j)) * Atom(s.Substring(j + 1))).ToString();
                while (c != row.Count)
                    row.RemoveAt(row.Count - 1);
            } while (j != -1);

            if (s.IndexOf("ERR") != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf(ERR.ToString()) != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf("INF") != -1)
            {
                depth--;
                return "INF";
            }
            if (s.IndexOf("ERR") != -1)
            {
                --depth;
                return "ERR";
            }
            if (s.IndexOf(ERR.ToString()) != -1)
            {
                --depth;
                return "ERR";
            }

            try
            {
                do
                {
                    int c = row.Count;
                    j = s.IndexOf("/");
                    if (j != -1) t = s.Substring(j + 1).IndexOfAny("^+$*/%id".ToCharArray());
                    else t = -1;
                    if (j != -1) l = s.Substring(0, j).LastIndexOfAny("^+$*/%cv".ToCharArray());
                    else l = -1;
                    if (j != -1)
                        if (t != -1)
                            if (l != -1)
                                s = s.Substring(0, l + 1) + (Atom(s.Substring(l + 1, j - l - 1)) / Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                            else
                                s = (Atom(s.Substring(0, j)) / Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                        else
                            if (l != -1)
                            s = s.Substring(0, l + 1) + (Atom(s.Substring(l + 1, j - l - 1)) / Atom(s.Substring(j + 1))).ToString();
                        else
                            s = (Atom(s.Substring(0, j)) / Atom(s.Substring(j + 1))).ToString();
                    while (c != row.Count)
                        row.RemoveAt(row.Count - 1);
                } while (j != -1);

                if (s.IndexOf("ERR") != -1)
                    MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (s.IndexOf(ERR.ToString()) != -1)
                    MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (s.IndexOf("INF") != -1)
                {
                    depth--;
                    return "INF";
                }
                if (s.IndexOf("ERR") != -1)
                {
                    --depth;
                    return "ERR";
                }
                if (s.IndexOf(ERR.ToString()) != -1)
                {
                    --depth;
                    return "ERR";
                }
                do
                {
                    int c = row.Count;
                    j = s.IndexOf("div");
                    if (j != -1) t = s.Substring(j + 1).IndexOfAny("^+$*/%d".ToCharArray());
                    else t = -1;
                    if (j != -1) l = s.Substring(0, j).LastIndexOfAny("^+$*/%cv".ToCharArray());
                    else l = -1;
                    if (j != -1)
                        if (t != -1)
                            if (l != -1)
                                s = s.Substring(0, l + 1) + ((int)Atom(s.Substring(l + 1, j - l - 1)) / (int)Atom(s.Substring(j + 3, t - 2))).ToString() + s.Substring(j + t + 2);
                            else
                                s = ((int)Atom(s.Substring(0, j)) / (int)Atom(s.Substring(j + 3, t - 2))).ToString() + s.Substring(j + t + 2);
                        else
                            if (l != -1)
                            s = s.Substring(0, l + 1) + ((int)Atom(s.Substring(l + 1, j - l - 1)) / (int)Atom(s.Substring(j + 3))).ToString();
                        else
                            s = ((int)Atom(s.Substring(0, j)) / (int)Atom(s.Substring(j + 3))).ToString();
                    while (c != row.Count)
                        row.RemoveAt(row.Count - 1);
                } while (j != -1);
            }
            catch(Exception) { return "INF"; }

            if (s.IndexOf("ERR") != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf(ERR.ToString()) != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf("INF") != -1)
            {
                depth--;
                return "INF";
            }
            if (s.IndexOf("ERR") != -1)
            {
                --depth;
                return "ERR";
            }
            if (s.IndexOf(ERR.ToString()) != -1)
            {
                --depth;
                return "ERR";
            }

            do
            {
                int c = row.Count;
                j = s.IndexOf("%");
                if (j != -1) t = s.Substring(j + 1).IndexOfAny("^+$*/%id".ToCharArray());
                else t = -1;
                if (j != -1) l = s.Substring(0, j).LastIndexOfAny("^+$*/%cv".ToCharArray());
                else l = -1;
                if (j != -1)
                    if (t != -1)
                        if (l != -1)
                            s = s.Substring(0, l + 1) + (Atom(s.Substring(l + 1, j - l - 1)) % Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                        else
                            s = (Atom(s.Substring(0, j)) % Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                    else
                        if (l != -1)
                        s = s.Substring(0, l + 1) + (Atom(s.Substring(l + 1, j - l - 1)) % Atom(s.Substring(j + 1))).ToString();
                    else
                        s = (Atom(s.Substring(0, j)) % Atom(s.Substring(j + 1))).ToString();
                while (c != row.Count)
                    row.RemoveAt(row.Count - 1);
            } while (j != -1);

            if (s.IndexOf("ERR") != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf(ERR.ToString()) != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf("INF") != -1)
            {
                depth--;
                return "INF";
            }
            if (s.IndexOf("ERR") != -1)
            {
                --depth;
                return "ERR";
            }
            if (s.IndexOf(ERR.ToString()) != -1)
            {
                --depth;
                return "ERR";
            }

            do
            {
                int c = row.Count;
                j = s.IndexOf("$");
                if (j != -1) t = s.Substring(j + 1).IndexOfAny("^+$*/%id".ToCharArray());
                else t = -1;
                if (j != -1) l = s.Substring(0, j).LastIndexOfAny("^+$*/%cv".ToCharArray());
                else l = -1;
                if (j != -1)
                    if (t != -1)
                        if (l != -1)
                            s = s.Substring(0, l + 1) + (Atom(s.Substring(l + 1, j - l - 1)) - Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                        else
                            s = (Atom(s.Substring(0, j)) - Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                    else
                        if (l != -1)
                            s = s.Substring(0, l + 1) + (Atom(s.Substring(l + 1, j - l - 1)) - Atom(s.Substring(j + 1))).ToString();
                        else
                            s = (Atom(s.Substring(0, j)) - Atom(s.Substring(j + 1))).ToString();
                while (c != row.Count)
                    row.RemoveAt(row.Count - 1);
            } while (j != -1);

            if (s.IndexOf("ERR") != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf(ERR.ToString()) != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf("INF") != -1)
            {
                depth--;
                return "INF";
            }
            if (s.IndexOf("ERR") != -1)
            {
                --depth;
                return "ERR";
            }
            if (s.IndexOf(ERR.ToString()) != -1)
            {
                --depth;
                return "ERR";
            }

            do
            {
                j = s.IndexOf("+");
                if (j != -1) t = s.Substring(j + 1).IndexOfAny("^+$*/%id".ToCharArray());
                else t = -1;
                if (j != -1) l = s.Substring(0, j).LastIndexOfAny("^+$*/%cv".ToCharArray());
                else l = -1;
                int c = row.Count;
                if (j != -1)
                    if (t != -1)
                        if (l != -1)
                            s = s.Substring(0, l + 1) + (Atom(s.Substring(l + 1, j - l - 1)) + Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                        else
                            s = (Atom(s.Substring(0, j)) + Atom(s.Substring(j + 1, t))).ToString() + s.Substring(j + t + 1);
                    else
                        if (l != -1)                         s = s.Substring(0, l + 1) + (Atom(s.Substring(l + 1, j - l - 1)) + Atom(s.Substring(j + 1))).ToString();
                    else
                        s = (Atom(s.Substring(0, j)) + Atom(s.Substring(j + 1))).ToString();
                while (c != row.Count)
                    row.RemoveAt(row.Count - 1);
            } while (j != -1);
            s = Atom(s).ToString();
            if (s.IndexOf("ERR") != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf(ERR.ToString()) != -1)
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (s.IndexOf("INF") != -1)
            {
                depth--;
                return "INF";
            }
            if (s.IndexOf("ERR") != -1)
            {
                --depth;
                return "ERR";
            }
            if (s.IndexOf(ERR.ToString()) != -1)
            {
                --depth;
                return "ERR";
            }
            double test;
            if (double.TryParse(s, out test))
                if (double.Parse(s) > 1000000 || double.Parse(s) < -1000000)
                    s = "ERR";

            --depth;
            return s;
        }

        private void calcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            row.Clear();
            clmn.Clear();
            row.Add(dataGridView1.SelectedCells[0].OwningRow.Index);
            clmn.Add(dataGridView1.SelectedCells[0].OwningColumn.Index);
            if (dataGridView1.CurrentCell != null)
                if (Parse(dataGridView1.CurrentCell.Value.ToString()) != "ERR")
                    dataGridView1.CurrentCell.Value = Parse(dataGridView1.CurrentCell.Value.ToString());
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toCopy.Clear();
            m.x = 1000;
            m.y = 1000;
            for (int i = 0; i < dataGridView1.SelectedCells.Count; ++i)
            {
                coord c1 = new coord();
                c1.x = dataGridView1.SelectedCells[i].OwningColumn.Index;
                c1.y = dataGridView1.SelectedCells[i].OwningRow.Index;
                c1.s = dataGridView1.SelectedCells[i].Value.ToString();
                toCopy.Add(c1);
                if (c1.x < m.x) m = c1;
                else if (c1.x == m.x && c1.y < m.y) m = c1;
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            coord d = new coord();
            d.x = dataGridView1.SelectedCells[0].ColumnIndex;
            d.y = dataGridView1.SelectedCells[0].RowIndex;
            for (int i = 0; i < toCopy.Count; ++i)
               if (d.y + toCopy[i].y - m.y < dataGridView1.RowCount && d.y + toCopy[i].y - m.y > 0 && d.x + toCopy[i].x - m.x < dataGridView1.ColumnCount && d.x + toCopy[i].x - m.x > 0)
                    dataGridView1.Rows[d.y + toCopy[i].y - m.y].Cells[d.x + toCopy[i].x - m.x].Value = toCopy[i].s;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.SelectedCells.Count; ++i)
                dataGridView1.SelectedCells[i].Value = "";
            for (int i = 0; i < dataGridView1.SelectedCells.Count; ++i)
                M[dataGridView1.SelectedCells[i].RowIndex][dataGridView1.SelectedCells[i].ColumnIndex] = "";
        }

        private DataTable DgvToDt()
        {
            var dt = new DataTable();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
                dt.Columns.Add();
            object[] cellValues = new object[dataGridView1.Columns.Count];
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                    cellValues[i] = row.Cells[i].Value;
                dt.Rows.Add(cellValues);
            }
            return dt;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Saveas = "";
            DataSet temp = new DataSet();
            temp.Tables.Add(DgvToDt());
            FormSaveas f = new FormSaveas();
            if (file == "C:\\Help\\New File.xml")
                if (Saveas != "!!!")
                {
                    f.ShowDialog();
                    file = Saveas;
                    temp.WriteXml(file);
                }
                else temp.WriteXml("C:\\Test\\8.xml");
            if (Saveas != "!!!")
            {
                Z.Clear();
                temp.WriteXml(file);
                for (int i = 0; i < dataGridView1.RowCount; ++i)
                    for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; ++j)
                        if (dataGridView1.Rows[i].Cells[j].Value == null)
                            Z.Add("");
                        else
                            Z.Add(dataGridView1.Rows[i].Cells[j].Value.ToString());
            }
        }

        private void FormTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataSet temp = new DataSet();
            temp.Tables.Add(DgvToDt());
            List<string> N = new List<string>();
            for (int i = 0; i < dataGridView1.RowCount; ++i)
                for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; ++j)
                    if (dataGridView1.Rows[i].Cells[j].Value == null)
                        N.Add("");
                    else
                        N.Add(dataGridView1.Rows[i].Cells[j].Value.ToString());
            bool flag = true;
            if (N.Count == Z.Count)
                for (int i = 0; i < N.Count; ++i)
                    if (Z[i] != N[i]) flag = false;
                    else;
            else flag = false;
            if (!flag)
            {
                if (DialogResult.Yes == (DialogResult)MessageBox.Show("File is not saved. Save?", "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    if(file != "C:\\Help\\New File.xml")
                        temp.WriteXml(file);
                else
                    {
                        FormSaveas f = new FormSaveas();
                        f.ShowDialog();
                        if (Saveas != "!!!")
                        {
                            file = Saveas;
                            temp.WriteXml(file);
                        }
                    }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            /*M[e.RowIndex][e.ColumnIndex] = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            row.Clear();
            clmn.Clear();
            for (int i = 0; i < dataGridView1.ColumnCount; ++i)
                for (int j = 0; j < dataGridView1.RowCount; ++j)
                    if (dataGridView1.Rows[j].Cells[i].Value != null)
                    {
                        row.Clear();
                        clmn.Clear();
                        row.Add(j);
                        clmn.Add(i);
                        string temp;
                        if (M[j][i] != "")
                            temp = Parse(M[j][i]);
                        else
                        temp = Parse(dataGridView1.Rows[j].Cells[i].Value.ToString());
                        if (temp != "ERR")
                            dataGridView1.Rows[j].Cells[i].Value = temp;
                     } */
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTextEditor f = new FormTextEditor("C:\\Help\\F1_1.txt");
            f.Show();
        }

        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTextEditor f = new FormTextEditor("C:\\Help\\F4_1.txt");
            f.Show();
        }

        private void saveasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet temp = new DataSet();
                temp.Tables.Add(DgvToDt());
                FormSaveas f = new FormSaveas();
                f.ShowDialog();
                if (Saveas != "!!!")
                {
                    file = Saveas;
                    temp.WriteXml(file);
                    Z.Clear();
                    for (int i = 0; i < dataGridView1.RowCount; ++i)
                        for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; ++j)
                            if (dataGridView1.Rows[i].Cells[j].Value == null)
                                Z.Add("");
                            else
                                Z.Add(dataGridView1.Rows[i].Cells[j].Value.ToString());
                }
            }
            catch (Exception) { MessageBox.Show("Invalid Name", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFilter f = new FormFilter(this, file);
            f.ShowDialog();
        }

        private void saveHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            using (XmlReader reader = XmlReader.Create(new StringReader("")))
            {
                transform.Load(reader);
            }
            StringWriter results = new StringWriter();
            using (XmlReader reader = XmlReader.Create(new StringReader(file)))
            {
                transform.Transform(reader, null, results);
            }
            MessageBox.Show(results.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press Ctrl+F to enable filters", "Help");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet temp = new DataSet();
                temp.Tables.Add(DgvToDt());
                FormSaveas f = new FormSaveas();
                f.ShowDialog();
                if (Saveas != "!!!")
                {
                    file = Saveas;
                    temp.WriteXml(file);
                    Z.Clear();
                    for (int i = 0; i < dataGridView1.RowCount; ++i)
                        for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; ++j)
                            if (dataGridView1.Rows[i].Cells[j].Value == null)
                                Z.Add("");
                            else
                                Z.Add(dataGridView1.Rows[i].Cells[j].Value.ToString());
                }
            }
            catch (Exception) { MessageBox.Show("Invalid Name", "Invalid Name", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            XDocument D;
            int cc = 0;
            D = XDocument.Load("C:\\Test\\11.xml");
            XDocument doc = D;
            var A = D.Elements("NewDataSet").Elements("Table1").Elements("Column6");
            var B = D.Elements("NewDataSet").Elements("Table1").Elements("Column5");
            A.Attributes();
            for (int i = 0; i < A.Nodes().Count(); ++i)
                if (A.Nodes().ElementAt(i).ToString() == "New York" && B.Nodes().ElementAt(i).ToString() == "Ukraine")
                    ++cc;
            MessageBox.Show(cc.ToString());
        }
    }
    public class coord
    {
        public coord()
        {
            x = 1000;
            y = 1000;
            s = "0";
        }
        public int x;
        public int y;
        public string s;
    }
}
