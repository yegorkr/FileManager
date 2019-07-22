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

namespace test__
{
    public partial class FormTextEditor : Form
    {
        static public List<string> ls = new List<string>();
        string def = "";
        string fileName = "";
        public FormTextEditor(string fileToOpen)
        {
            InitializeComponent();
            string text = File.ReadAllText(fileToOpen);
            richTextBox1.Text = text;
            def = richTextBox1.Text;
            fileName = fileToOpen;
            this.Text = fileName;
        }

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) contextMenuStrip1.Show();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.SelectedText);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.SelectedText);
            richTextBox1.Cut();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SaveFile(fileName, RichTextBoxStreamType.PlainText);
            def = richTextBox1.Text;
        }

        private void FormTextEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < def.Length; ++i)
                if (def[i] == '\r') def = def.Remove(i, 1);
            if (def != richTextBox1.Text)
            {
                if(DialogResult.Yes == (DialogResult)MessageBox.Show("File is not saved. Save?", "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                richTextBox1.SaveFile(fileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void wordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = richTextBox1.Text;
            s = s.ToUpper();
            int i = 0;
            s = s.Insert(0, " ");
            s += " ";
            while (i < s.Length)
            {
                if (!isLetter(s[i]) && s[i] != ' ')
                {
                    s = s.Remove(i, 1);
                    s = s.Insert(i, " ");
                }
                ++i;
            }
            i = 0;
            while (i < s.Length)
            {
                string word = "";
                while (isLetter(s[i]))
                {
                    word += s[i];
                    ++i;
                    if (i == s.Length) break;
                }
                if(word != "") if (s.LastIndexOf(" " + word + " ") != i - word.Length - 1)
                {
                    i = i - word.Length - 1;
                    s = s.Replace(" " + word + " ", " ");
                }
                ++i;
            }
            i = 0;
            s = s.ToLower();
            MessageBox.Show(s, "Words that appear in the text once");
        }

        private bool isLetter(char c)
        {
            if ((c > 64 && c < 91) || (c > 96 && c < 123) || (c > 1039 && c < 1072) || c == 1030) return true;
            return false;
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = richTextBox1.Text;
            int fl = 0;
            for (int i = 0; i < s.Length; ++i)
                if (s[i] == '\n')
                {
                    ls.Add(s.Substring(fl, i - fl));
                    s = s.Remove(i, 1);
                    fl = i;
                }
            ls.Add(s.Substring(fl));
            FormStringSearch f = new FormStringSearch();
            f.ShowDialog();
        }
    }
}
