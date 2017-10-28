using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MLDBUtils
{
    public partial class MessageBoxL : Form
    {
        public MessageBoxL(string cap,string mes)
        {
            InitializeComponent();

            this.Text = cap;
            richTextBox1.Text = mes;
        }
    }
}