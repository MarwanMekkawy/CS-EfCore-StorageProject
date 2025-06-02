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
    public partial class F_Edit : Form
    {
        Context db = new Context();
        public F_Edit()
        {
            InitializeComponent();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //itmes data fetch
            List<string> Ilist = db.Items.Select(x => x.Name).ToList();
            comboBox8.DataSource = Ilist;
            comboBox8.SelectedIndex = -1;
            comboBox8.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox8.AutoCompleteSource = AutoCompleteSource.ListItems;

            //storage data fetch
            List<string> Slist = db.Storages.Select(x => x.Name).ToList();
            comboBox9.DataSource = Slist;
            comboBox9.SelectedIndex = -1;

            //client data fetch
            List<string> Clist = db.Clients.Select(x => x.Name).ToList();
            comboBox10.DataSource = Clist;
            comboBox10.SelectedIndex = -1;
        }

        //selecting item
        private void comboBox8_TextChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = false;
            textBox4.Text = null;
            dataGridView1.DataSource = null;

            string item = comboBox8.Text;
            if (!string.IsNullOrEmpty(item))
            {
                string code = db.Items.Where(x => x.Name == item).Select(x => x.Code).FirstOrDefault();
                if (!string.IsNullOrEmpty(code))
                {
                    textBox4.Text = code;
                    textBox4.Enabled = true;
                }
                dataGridView1.DataSource = new[] { new { Name = item, Code = code } };
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        //submitting item edit
        private void button8_Click(object sender, EventArgs e)
        {
            string Iname = comboBox8.Text?.Trim();
            string newCode = textBox4.Text?.Trim();

            if (string.IsNullOrEmpty(newCode))
            {
                MessageBox.Show("Please enter a new code.");
                return;
            }

            var itemToUpdate = db.Items.FirstOrDefault(x => x.Name == Iname);

            itemToUpdate.Code = newCode;

            db.SaveChanges();

            MessageBox.Show("Code updated successfully.");
            //refresh
            var updatedItem = new[] { new { Name = itemToUpdate.Name, Code = itemToUpdate.Code } };
            dataGridView1.DataSource = updatedItem;
        }
    }
}

