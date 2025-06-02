using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            comboBox9.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox9.AutoCompleteSource = AutoCompleteSource.ListItems;

            //client data fetch
            List<string> Clist = db.Clients.Select(x => x.Name).ToList();
            comboBox10.DataSource = Clist;
            comboBox10.SelectedIndex = -1;
            comboBox10.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox10.AutoCompleteSource = AutoCompleteSource.ListItems;

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
                    dataGridView1.DataSource = new[] { new { Name = item, Code = code } };
                }
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
            string NewCode = textBox4.Text?.Trim();

            if (string.IsNullOrWhiteSpace(NewCode))
            {
                MessageBox.Show("Please enter a new code.");
                return;
            }

            var itemToUpdate = db.Items.FirstOrDefault(x => x.Name == Iname);

            itemToUpdate.Code = NewCode;

            db.SaveChanges();                                                        //save  

            MessageBox.Show(Iname + " updated successfully.");
            //refresh
            dataGridView1.DataSource = new[] { new { Name = Iname, Code = NewCode } };

        }
        //deleting item
        private void button2_Click(object sender, EventArgs e)
        {
            string Iname= comboBox8.Text?.Trim();
            if (string.IsNullOrWhiteSpace(Iname)) 
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }
            var ItemToDelete = db.Items.FirstOrDefault(x => x.Name == Iname);

            if (ItemToDelete == null) { MessageBox.Show("Item not found.");return; }

            var confirm = MessageBox
                .Show($"Are you sure you want to delete '{Iname}'?","Confirm Delete",MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                db.Items.Remove(ItemToDelete);
                db.SaveChanges();

                MessageBox.Show($"{Iname} deleted successfully.");

                // Refresh 
                comboBox8.DataSource = db.Items.Select(x => x.Name).ToList();
                comboBox8.SelectedIndex = -1;
                textBox4.Text = null;
                textBox4.Enabled = false;
                dataGridView1.DataSource = null;
            }
        }

        //select storage
        private void comboBox9_TextChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox2.Text = null;
            textBox3.Text = null;
            dataGridView1.DataSource = null;

            string Sname = comboBox9.Text;

            if (!string.IsNullOrWhiteSpace(Sname))
            {
                var Sdata = db.Storages.Where(x => x.Name == Sname).Select(x => new { x.Address, x.Supervisor }).ToList();
                if (Sdata.Any())
                {
                    textBox2.Text = Sdata[0].Address;
                    textBox3.Text = Sdata[0].Supervisor;
                    textBox2.Enabled = true;
                    textBox3.Enabled = true;
                    dataGridView1.DataSource = new[] { new { StorageName = Sname, StorageAddress = Sdata[0].Address, StorageSupervisor = Sdata[0].Supervisor } };
                }
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }
        //submitting storage edit
        private void button1_Click(object sender, EventArgs e)
        {
            string Sname = comboBox9.Text;
            string NewSaddress = textBox2.Text.Trim();
            string NewSsuper = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(NewSaddress))
            {
                MessageBox.Show("Please enter a new Address.");
                return;
            }
            if (string.IsNullOrWhiteSpace(NewSsuper))
            {
                MessageBox.Show("Please enter a new Supervisor Name.");
                return;
            }
            var itemToUpdate = db.Storages.FirstOrDefault(x => x.Name == Sname);

            itemToUpdate.Address = NewSaddress;
            itemToUpdate.Supervisor = NewSsuper;

            db.SaveChanges();                                                          //save

            MessageBox.Show(Sname + " updated successfully.");
            //refresh
            dataGridView1.DataSource = new[] { new { Storagename = Sname, StorageAddress = NewSaddress, StorageSupervisor = NewSsuper } };
        }

        //select client
        private void comboBox10_TextChanged(object sender, EventArgs e)
        {
            textBox5.Enabled = false;
            textBox7.Enabled = false;
            textBox11.Enabled = false;
            textBox9.Enabled = false;
            textBox10.Enabled = false;
            comboBox1.Enabled = false;

            textBox5.Text = null;
            textBox7.Text = null;
            textBox11.Text = null;
            textBox9.Text = null;
            textBox10.Text = null;
            comboBox1.Text = null;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            dataGridView1.DataSource = null;

            string Cname = comboBox10.Text;
            if (!string.IsNullOrWhiteSpace(Cname))
            {

                comboBox1.Items.Clear();
                comboBox1.Items.Add("Importer");
                comboBox1.Items.Add("Exporter");

                var Cdata = db.Clients.FirstOrDefault(x => x.Name == Cname);
                if (Cdata != null)
                {
                    textBox5.Enabled = true;
                    textBox7.Enabled = true;
                    textBox11.Enabled = true;
                    textBox9.Enabled = true;
                    textBox10.Enabled = true;
                    comboBox1.Enabled = true;

                    textBox5.Text = Cdata.Phone;
                    textBox7.Text = Cdata.Fax;
                    textBox11.Text = Cdata.Mobile;
                    textBox9.Text = Cdata.Email;
                    textBox10.Text = Cdata.Website;
                    comboBox1.SelectedItem = Cdata.Type.ToString();

                    dataGridView1.DataSource = new[] { new {ClientName=Cname,Phone= Cdata.Phone ,Fax= Cdata.Fax ,Mobile= Cdata.Mobile,
                    Email= Cdata.Email,Website= Cdata.Website,Type=Cdata.Type.ToString()} };
                }
            }
            else
            {
                dataGridView1.DataSource = null;
            }

        }

        //submitting client edit
        private void button3_Click(object sender, EventArgs e)
        {
            string Cname = comboBox10.Text;
            string NewCphone = textBox5.Text.Trim();
            string NewCfax = textBox7.Text.Trim();
            string NewCmobile = textBox11.Text.Trim();
            string NewCemail = textBox9.Text.Trim();
            string NewCwebsite = textBox10.Text.Trim();
            string NewCtype = comboBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(NewCphone)) { MessageBox.Show("Please enter a new Phone."); return; }
            if (string.IsNullOrWhiteSpace(NewCfax)) { MessageBox.Show("Please enter a new Fax."); return; }
            if (string.IsNullOrWhiteSpace(NewCmobile)) { MessageBox.Show("Please enter a new Mobile."); return; }
            if (string.IsNullOrWhiteSpace(NewCemail)) { MessageBox.Show("Please enter a new Email."); return; }
            if (string.IsNullOrWhiteSpace(NewCwebsite)) { MessageBox.Show("Please enter a new Website."); return; }

            if (!string.IsNullOrEmpty(Cname))
            {
                var itemToUpdate = db.Clients.FirstOrDefault(x => x.Name == Cname);
                itemToUpdate.Phone = NewCphone;
                itemToUpdate.Fax = NewCfax;
                itemToUpdate.Mobile = NewCmobile;
                itemToUpdate.Email = NewCemail;
                itemToUpdate.Website = NewCwebsite;
                itemToUpdate.Type = NewCtype;
                
                db.SaveChanges();                                            //save

                //refresh
                dataGridView1.DataSource = new[] { new {ClientName=Cname,Phone= NewCphone ,Fax= NewCfax ,Mobile= NewCmobile,
                Email= NewCemail,Website= NewCwebsite, Type=NewCtype} };
                MessageBox.Show(Cname + " updated successfully.");
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }
        
    }
}

