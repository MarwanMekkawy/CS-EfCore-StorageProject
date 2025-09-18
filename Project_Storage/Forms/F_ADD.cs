using Project_Storage.Data;
using Project_Storage.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project_Storage
{
    public partial class F_ADD : Form
    {
        Context db = new Context();
       

        public F_ADD()
        {    
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            ///client combobox
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Importer");
            comboBox1.Items.Add("Exporter");
            comboBox1.SelectedItem = "Exporter";
            ///transfer comboboxs
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox5.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox6.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox7.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox13.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();
            comboBox7.Items.Clear();
            comboBox13.Items.Clear();

            //transfer type
            comboBox2.Items.Add("in");
            comboBox2.Items.Add("out");
            comboBox2.Items.Add("internal");
            comboBox2.SelectedItem = "out";

            //internal move?
            comboBox3.Items.Add(true);
            comboBox3.Items.Add(false);
            comboBox3.SelectedItem = false;

            //importer client
            ImportClient();
            comboBox4.SelectedIndex = 0;

            //importer storage
            ImportStorage();
            comboBox5.SelectedIndex = 0;

            //exporter client
            ExportClient();
            comboBox6.SelectedIndex = 0;

            //exporter storage
            ExportStorage();
            comboBox7.SelectedIndex = 0;

            //item name
            NewItem();
            if (comboBox13.Items.Count > 0) comboBox13.SelectedIndex = 0;
        }

        //func.s for refreshing lists
        public void ImportClient()
        {
            List<string> list = db.Clients.Where(x => x.Type == "Importer").Select(x => x.Name).ToList();
            list.Insert(0, "none");                 //setting 1st value =null
            comboBox4.DataSource = list;
        }
        public void ExportClient()
        {
            List<string> list3 = db.Clients.Where(x => x.Type == "Exporter").Select(x => x.Name).ToList();
            list3.Insert(0, "none");
            comboBox6.DataSource = list3;
        }
        public void ImportStorage()
        {
            List<string> list2 = db.Storages.Select(x => x.Name).ToList();
            list2.Insert(0, "none");
            comboBox5.DataSource = list2;
        }
        public void ExportStorage()
        {
            List<string> list4 = db.Storages.Select(x => x.Name).ToList();
            list4.Insert(0, "none");
            comboBox7.DataSource = list4;
        }
        public void NewItem()
        {
            List<string> list5 = db.Items.Select(x => x.Name).ToList();
            comboBox13.DataSource = list5;
        }

        //add storage button
        private void button1_Click(object sender, EventArgs e)
        {
            // Check if any required field is empty
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||  string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please fill in all required fields before adding storage.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
                string name = textBox1.Text;
            string address = textBox2.Text;
            string super = textBox3.Text;
            Storages storage = new Storages();
            storage.Name = name;
            storage.Address = address;
            storage.Supervisor = super;

            db.Storages.Add(storage);
            db.SaveChanges();

            ExportStorage(); //refreshs the cobmo list including storages
            ImportStorage();
        }
        //add item button
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please fill in all required fields before adding item.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string name = textBox6.Text;
            string code = textBox4.Text;
            Items item = new Items();
            item.Name = name;
            item.Code = code;
            db.Items.Add(item);
            db.SaveChanges();

            NewItem(); //refreshs the cobmo list including item
        }

        //add client button
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text) || string.IsNullOrWhiteSpace(textBox7.Text) || string.IsNullOrWhiteSpace(textBox8.Text) ||
                string.IsNullOrWhiteSpace(textBox9.Text) || string.IsNullOrWhiteSpace(textBox10.Text) ||  string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Please fill in all required fields before adding client.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string name = textBox8.Text;
            string phone = textBox5.Text;
            string fax = textBox7.Text;
            string mobile = textBox11.Text;
            string email = textBox9.Text;
            string website = textBox10.Text;
            string type = comboBox1.SelectedItem.ToString();


            Clients client = new Clients();
            client.Name = name;
            client.Phone = phone;
            client.Fax = fax;
            client.Mobile = mobile;
            client.Email = email;
            client.Website = website;
            client.Type = type;
            db.Clients.Add(client);
            db.SaveChanges();

            ImportClient(); //refreshs the cobmo list including client
            ExportClient();
        }
        //add transfer button
        private void button4_Click(object sender, EventArgs e)
        {
            Transfers transfer = new Transfers();
            transfer.Type = comboBox2.SelectedItem.ToString();
            transfer.Move = (bool)comboBox3.SelectedItem;

            //client picking cases
            if ((comboBox4.SelectedItem as string) == "none" && (comboBox6.SelectedItem as string) == "none")
            {
                transfer.ClientName = null;
            }
            else if ((comboBox4.SelectedItem as string) == "none")
            {
                transfer.ClientName = comboBox6.SelectedItem.ToString();
            }
            else if ((comboBox6.SelectedItem as string) == "none")
            {
                transfer.ClientName = comboBox4.SelectedItem.ToString();
            }
            //storage picking cases
            if ((comboBox7.SelectedItem as string) != "none" && (comboBox5.SelectedItem as string) != "none")
            {
                transfer.ImporterStorageName = comboBox5.SelectedItem.ToString();
                transfer.ExporterStorageName = comboBox7.SelectedItem.ToString();
            }
            else if ((comboBox5.SelectedItem as string) == "none")
            {
                transfer.ImporterStorageName = null;
                transfer.ExporterStorageName = comboBox7.SelectedItem.ToString();
            }
            else if ((comboBox7.SelectedItem as string) == "none")
            {
                transfer.ImporterStorageName = comboBox5.SelectedItem.ToString();
                transfer.ExporterStorageName = null;
            }

            transfer.TransferDate = dateTimePicker1.Value;
            transfer.ProductionDate = dateTimePicker2.Value;
            transfer.ExpiryDate = dateTimePicker3.Value;
            transfer.ItemName = comboBox13.SelectedItem.ToString();
            transfer.UnitCount = int.Parse(textBox13.Text); ;
 
            db.Transfers.Add(transfer);


            //adding stored item per transfer in stored table
            Stored stored = new Stored();
            Stored removed = new Stored();
            if (comboBox2.SelectedItem.ToString() == "in")
            {
                stored.StorageName = comboBox5.SelectedItem.ToString();
                stored.ItemName = comboBox13.SelectedItem.ToString();
                stored.TotalUnits = int.Parse(textBox13.Text);
                db.Stored.Add(stored);
            }
            else if (comboBox2.SelectedItem.ToString() == "out")
            {
                removed.StorageName = comboBox7.SelectedItem.ToString();
                removed.ItemName = comboBox13.SelectedItem.ToString();
                removed.TotalUnits = -int.Parse(textBox13.Text);
                db.Stored.Add(removed);
            }
            else if (comboBox2.SelectedItem.ToString() == "internal")
            {
                stored.StorageName = comboBox5.SelectedItem.ToString();
                stored.ItemName = comboBox13.SelectedItem.ToString();
                stored.TotalUnits = int.Parse(textBox13.Text);

                removed.StorageName = comboBox7.SelectedItem.ToString();
                removed.ItemName = comboBox13.SelectedItem.ToString();
                removed.TotalUnits = -int.Parse(textBox13.Text);
                db.Stored.Add(stored);
                db.Stored.Add(removed);
            }

            db.SaveChanges();
        }


        //making sure internal/external transfers triggers the right comboboxes [ensuring no combo conflicts]
        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((bool)comboBox3.SelectedItem == true)
            {
                comboBox4.SelectedItem = "none";
                comboBox6.SelectedItem = "none";
                comboBox2.SelectedItem = "internal";
                comboBox4.Enabled = false;
                comboBox6.Enabled = false;
                comboBox2.Enabled = false;
                comboBox5.Enabled = true;
                comboBox7.Enabled = true;
            }
            else if ((bool)comboBox3.SelectedItem == false)
            {
                comboBox2.Enabled = true;
                comboBox2.SelectedIndex = -1;
                comboBox4.Enabled = false;
                comboBox6.Enabled = false;
                comboBox5.Enabled = false;
                comboBox7.Enabled = false;
                comboBox4.SelectedItem = "none";
                comboBox6.SelectedItem = "none";
                comboBox5.SelectedItem = "none";
                comboBox7.SelectedItem = "none";

            }
        }
        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((comboBox2.SelectedItem as string) == "in")
            {
                comboBox4.SelectedItem = "none";
                comboBox7.SelectedItem = "none";
                comboBox4.Enabled = false;
                comboBox7.Enabled = false;
                comboBox5.Enabled = true;
                comboBox6.Enabled = true;
            }
            else if ((comboBox2.SelectedItem as string) == "out")
            {
                comboBox5.SelectedItem = "none";
                comboBox6.SelectedItem = "none";
                comboBox5.Enabled = false;
                comboBox6.Enabled = false;
                comboBox4.Enabled = true;
                comboBox7.Enabled = true;
            }
            else if ((comboBox2.SelectedItem as string) == "internal")
            {
                comboBox4.SelectedItem = "none";
                comboBox6.SelectedItem = "none";
                comboBox3.SelectedItem = true;
                comboBox4.Enabled = false;
                comboBox6.Enabled = false;
                comboBox5.Enabled = true;
                comboBox7.Enabled = true;
                
            }
        }

        ////closing importer storage/exporter client options
        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((comboBox4.SelectedItem as string) != "none")
            {
                comboBox6.SelectedItem = "none";
                comboBox5.SelectedItem = "none";
                comboBox6.Enabled = false;
                comboBox5.Enabled = false;
            }
            else
            {
                comboBox6.Enabled = true;
                comboBox5.Enabled = true;
            }
        }

        ////closing importing client/exporter storage  options
        private void comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((comboBox6.SelectedItem as string) != "none")
            {
                comboBox4.SelectedItem = "none";
                comboBox7.SelectedItem = "none";
                comboBox4.Enabled = false;
                comboBox7.Enabled = false;
            }
            else
            {
                comboBox4.Enabled = true;
                comboBox7.Enabled = true;
            }
        }

        //////disposing the connection
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }













        //assuring that importing storage not the exporting storage at same time (endless loop problem to solve) #######
        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            //List<string> list4 = db.Storages.Select(x => x.Name).ToList();
            //list4.Insert(0, "none");
            //string selected = comboBox5.SelectedItem as string;
            //if (selected != null)
            //{
            //    list4.Remove(selected);
            //}
            //comboBox7.DataSource = list4;
        }
        //vise versa
        private void comboBox7_SelectedValueChanged(object sender, EventArgs e)
        {
            //List<string> list2 = db.Storages.Select(x => x.Name).ToList();
            //list2.Insert(0, "none");
            //string selected = comboBox7.SelectedItem as string;
            //if (selected != null)
            //{
            //    list2.Remove(selected);
            //}
            //comboBox5.DataSource = list2;
        }

        
    }
}
