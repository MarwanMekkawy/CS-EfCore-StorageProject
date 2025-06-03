using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Storage
{
    public partial class F_Result : Form
    {
        public F_Result()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
        }

        public void LoadResults<T>(string x,List<T> results)
        {
            label1.Text = x;
            dataGridView1.DataSource = results;
        }
    }
}
