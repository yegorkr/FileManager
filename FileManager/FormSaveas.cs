﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test__
{
    public partial class FormSaveas : Form
    {
        public FormSaveas()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormTable.Saveas = "!!!";
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Substring(textBox1.Text.Length - 5) != ".xml")
                    textBox1.Text += ".xml";
                FormTable.Saveas = textBox1.Text;
                this.Close();
            }
            catch (Exception) { }
        }
    }
}
