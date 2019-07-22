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

namespace test__
{
    public partial class FormFilter : Form
    {
        XDocument D;
        FormTable F;
        string file;
        string file1;
        public FormFilter(FormTable FT, string File)
        {
            InitializeComponent();
            file = File;
            D = XDocument.Load(File);
            F = FT;
            file1 = file.Insert(file.LastIndexOf('.'), "63521");
            D.Save(file1);
        }

        private void Filters()
        {
            int i;
            D = XDocument.Load(file1);
            XDocument doc = D;
            i = -1;
            DataSet ds = new DataSet();
            while (i == -1)
            {
                i = 0;
                foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                {
                    if (xDes.Elements().Count() != 10)
                    {
                        xDes.Remove();
                        i = -1;
                    }
                }
            }
            doc.Save(file);
 
            if (checkBox1.Checked)
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                       if (xDes.Elements().ElementAt(9).ToString().IndexOf("PC") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                        {
                            xDes.Remove();
                            i = -1;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }
            if (checkBox2.Checked)
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                        if (xDes.Elements().ElementAt(9).ToString().IndexOf("PS4") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                        {
                            xDes.Remove();
                            i = -1;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }

            if (checkBox3.Checked)
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                        if (xDes.Elements().ElementAt(9).ToString().IndexOf("XOne") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                        {
                            xDes.Remove();
                            i = -1;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }

            if (checkBox4.Checked)
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                        if (xDes.Elements().ElementAt(9).ToString().IndexOf("3DS") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                        {
                            xDes.Remove();
                            i = -1;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }

            if (textBox1.Text != "")
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                        if (xDes.Elements().ElementAt(0).ToString().IndexOf(textBox1.Text) == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                        {
                            xDes.Remove();
                            i = -1;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }

            if (textBox2.Text != "")
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                        if ((xDes.Elements().ElementAt(1).ToString() + " " + xDes.Elements().ElementAt(2).ToString() + " " + xDes.Elements().ElementAt(1).ToString()).IndexOf(textBox2.Text) == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                        {
                            xDes.Remove();
                            i = -1;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }

            if (comboBox1.SelectedIndex != 0 && comboBox1.SelectedIndex != -1)
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                        if (xDes.Elements().ElementAt(3).ToString().IndexOf(comboBox1.Items[comboBox1.SelectedIndex].ToString()) == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                        {
                            xDes.Remove();
                            i = -1;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }
            if (comboBox2.SelectedIndex != 0 && comboBox2.SelectedIndex != -1)
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                        if (xDes.Elements().ElementAt(4).ToString().IndexOf(comboBox2.Items[comboBox2.SelectedIndex].ToString()) == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                        {
                            xDes.Remove();
                            i = -1;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }

            if (textBox3.Text != "")
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                        if (xDes.Elements().ElementAt(7).ToString().IndexOf(textBox3.Text) == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                        {
                            xDes.Remove();
                            i = -1;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }

            if (textBox4.Text != "")
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                        if ((xDes.Elements().ElementAt(5).ToString() + " " + xDes.Elements().ElementAt(6).ToString() + " " + xDes.Elements().ElementAt(5).ToString()).IndexOf(textBox4.Text) == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                        {
                            xDes.Remove();
                            i = -1;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }

            if (comboBox3.SelectedIndex != 0 && comboBox3.SelectedIndex != -1)
            {
                i = -1;
                while (i == -1)
                {
                    i = 0;
                    foreach (XElement xDes in doc.Element("NewDataSet").Elements())
                        switch (comboBox3.SelectedIndex)
                        {
                            case 1:
                                if (xDes.Elements().ElementAt(8).ToString().IndexOf(">11") == -1 && xDes.Elements().ElementAt(8).ToString().IndexOf(" 11") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                                {
                                    xDes.Remove();
                                    i = -1;
                                }
                                break;
                            case 2:
                                if (xDes.Elements().ElementAt(8).ToString().IndexOf(">22") == -1 && xDes.Elements().ElementAt(8).ToString().IndexOf(" 22") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                                {
                                    xDes.Remove();
                                    i = -1;
                                }
                                break;
                            case 4:
                                if (xDes.Elements().ElementAt(8).ToString().IndexOf(">44") == -1 && xDes.Elements().ElementAt(8).ToString().IndexOf(" 44") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                                {
                                    xDes.Remove();
                                    i = -1;
                                }
                                break;
                            case 3:
                                switch (comboBox4.SelectedIndex)
                                {
                                    case 1:
                                        if (xDes.Elements().ElementAt(8).ToString().IndexOf(">31") == -1 && xDes.Elements().ElementAt(8).ToString().IndexOf(" 31") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                                        {
                                            xDes.Remove();
                                            i = -1;
                                        }
                                        break;
                                    case 2:
                                        if (xDes.Elements().ElementAt(8).ToString().IndexOf(">32") == -1 && xDes.Elements().ElementAt(8).ToString().IndexOf(" 32") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                                        {
                                            xDes.Remove();
                                            i = -1;
                                        }
                                        break;
                                    case 3:
                                        if (xDes.Elements().ElementAt(8).ToString().IndexOf(">33") == -1 && xDes.Elements().ElementAt(8).ToString().IndexOf(" 33") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                                        {
                                            xDes.Remove();
                                            i = -1;
                                        }
                                        break;
                                    default:
                                        if (xDes.Elements().ElementAt(8).ToString().IndexOf(">3") == -1 && xDes.Elements().ElementAt(8).ToString().IndexOf(" 3") == -1 && xDes.Elements().ElementAt(9).ToString() != "<Column10>Newsletter</Column10>")
                                        {
                                            xDes.Remove();
                                            i = -1;
                                        }
                                        break;
                                }
                                break;
                        }
                }
                doc.Save(file);
                ds = new DataSet();
                ds.ReadXml(file);
                F.dataGridView1.DataSource = ds;
            }

            doc.Save(file);
            ds = new DataSet();
            ds.ReadXml(file);
            F.dataGridView1.DataSource = ds;
            D = XDocument.Load(file1);
            D.Save(file);
        }
        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Items[comboBox2.SelectedIndex].ToString() != "")
            {
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false;
                Filters();
            }
            else
            {
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox3.Text = "";
                textBox4.Text = "";
                Filters();
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Items[comboBox3.SelectedIndex].ToString() != "Credit card")
            {
                label10.Visible = false;
                comboBox4.Visible = false;
                comboBox4.SelectedIndex = 0;
                Filters();
            }
            else
            {
                label10.Visible = true;
                comboBox4.Visible = true;
                Filters();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Filters();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Filters();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Filters();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Filters();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Filters();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Filters();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filters();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Filters();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filters();
        }

        private void FormFilter_FormClosing(object sender, FormClosingEventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            Filters();
            if (System.IO.File.Exists(file1))
                System.IO.File.Delete(file1);
        }
    }
}
