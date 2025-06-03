using Microsoft.EntityFrameworkCore;
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
        public bool Edit = false;
        private int _selectedTransferId = -1;


        public F_Edit()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            //importer client
            ImportClient();
            comboBox4.SelectedIndex = 0;
            comboBox4.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox4.AutoCompleteSource = AutoCompleteSource.ListItems;

            //importer storage
            ImportStorage();
            comboBox5.SelectedIndex = 0;
            comboBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox5.AutoCompleteSource = AutoCompleteSource.ListItems;

            //exporter client
            ExportClient();
            comboBox6.SelectedIndex = 0;
            comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;

            //exporter storage
            ExportStorage();
            comboBox7.SelectedIndex = 0;
            comboBox7.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox7.AutoCompleteSource = AutoCompleteSource.ListItems;

            //items 
            ItemsNames();
            comboBox13.SelectedIndex = 0;
            comboBox13.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox13.AutoCompleteSource = AutoCompleteSource.ListItems;

            //gridview content-fill
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;


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

            //transfers
            comboBox3.Items.Add(false);
            comboBox3.Items.Add(true);
            comboBox3.SelectedIndex = -1;
            comboBox2.Items.Add("in");
            comboBox2.Items.Add("out");
            comboBox2.Items.Add("internal");
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Enabled = false;
            comboBox4.Enabled = false;
            comboBox6.Enabled = false;
            comboBox5.Enabled = false;
            comboBox7.Enabled = false;
            button5.Visible = false;
            button10.Visible = false;
            comboBox13.Enabled = false;
            textBox13.Enabled = false;
        }
        //transfer combobox fetch
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
        public void ItemsNames()
        {
            List<string> list5 = db.Items.Select(x => x.Name).ToList();
            list5.Insert(0, "none");
            comboBox13.DataSource = list5;
        }
        ///transfer main query///
        private List<Transfers> GetTransfers(bool z)
        {
            var transfers = db.Transfers.Where(x => x.Move == z).ToList();

            if (transfers.Count != 0)
                return transfers;
            else
            {
                MessageBox.Show("No items found.");
                return transfers;
            }
        }


        ////////////selecting item////////////
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
            string Iname = comboBox8.Text?.Trim();
            if (string.IsNullOrWhiteSpace(Iname))
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }
            var ItemToDelete = db.Items.FirstOrDefault(x => x.Name == Iname);

            if (ItemToDelete == null) { MessageBox.Show("Item not found."); return; }

            var confirm = MessageBox
                .Show($"Are you sure you want to delete '{Iname}'? this will also will delete all the reltaed [stored items/transfers]", "Warning", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                db.Items.Remove(ItemToDelete);
                db.SaveChanges();

                MessageBox.Show($"{Iname} deleted successfully.");

                // Refresh 
                comboBox8.DataSource = db.Items.Select(x => x.Name).ToList();
                comboBox8.SelectedIndex = -1;
                dataGridView1.DataSource = null;
            }
        }

        ////////////select storage////////////
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

        //storage delete
        private void button6_Click(object sender, EventArgs e)
        {
            string Sname = comboBox9.Text?.Trim();

            if (string.IsNullOrWhiteSpace(Sname))
            {
                MessageBox.Show("Please select a Storage to delete.");
                return;
            }

            var StorageToDelete = db.Storages.FirstOrDefault(x => x.Name == Sname);
            if (StorageToDelete != null)
            {

                var confirm = MessageBox
                   .Show($"Are you sure you want to delete '{Sname}'? this will also delete all the related data to that storage [stored items, transfer(imported/exported)]", "Warning", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    var relatedTransfers = db.Transfers.Where(t => t.ExporterStorageName == Sname || t.ImporterStorageName == Sname).ToList();  //[deleting related 
                    if (relatedTransfers.Any()) { db.Transfers.RemoveRange(relatedTransfers); }                                                   //transfers first.]

                    db.Remove(StorageToDelete);
                    db.SaveChanges();
                    MessageBox.Show($"{Sname} deleted successfully.");
                }
                //refresh
                comboBox9.DataSource = db.Storages.Select(x => x.Name).ToList();
                comboBox9.SelectedIndex = -1;
                dataGridView1.DataSource = null;
            }
            else
            {
                MessageBox.Show("Storage Not Found");
            }
        }

        ////////////select client////////////
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

        //client delete
        private void button7_Click(object sender, EventArgs e)
        {
            string Cname = comboBox10.Text;
            if (!string.IsNullOrWhiteSpace(Cname))
            {
                var ItemToDelete = db.Clients.FirstOrDefault(x => x.Name == Cname);
                if (ItemToDelete != null)
                {
                    var confirm = MessageBox.Show($"Are you sure you want to delete '{Cname}'? this will delete all the related [transfers]", "Warning", MessageBoxButtons.YesNo);
                    if (confirm == DialogResult.Yes)
                    {
                        db.Remove(ItemToDelete);
                        db.SaveChanges();
                        MessageBox.Show($"{Cname} deleted successfully.");

                        // Refresh 
                        comboBox10.DataSource = db.Clients.Select(x => x.Name).ToList();
                        comboBox10.SelectedIndex = -1;
                        dataGridView1.DataSource = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }
        }

        ////////////select Transfer////////////
        /////making sure internal/external transfers triggers the right comboboxes [ensuring no combo conflicts]
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


                if (!Edit) { dataGridView1.DataSource = GetTransfers(true); }
            }
            else if ((bool)comboBox3.SelectedItem == false)
            {
                comboBox4.Enabled = true;
                comboBox6.Enabled = true;
                comboBox2.Enabled = true;
                comboBox4.Enabled = false;
                comboBox6.Enabled = false;
                comboBox5.Enabled = false;
                comboBox7.Enabled = false;
                comboBox4.SelectedItem = "none";
                comboBox6.SelectedItem = "none";
                comboBox5.SelectedItem = "none";
                comboBox7.SelectedItem = "none";

                if (!Edit) { dataGridView1.DataSource = GetTransfers(false); }
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

                if (!Edit) { dataGridView1.DataSource = GetTransfers(false).Where(z => z.Type == "in").ToList(); }
            }
            else if ((comboBox2.SelectedItem as string) == "out")
            {
                comboBox5.SelectedItem = "none";
                comboBox6.SelectedItem = "none";
                comboBox5.Enabled = false;
                comboBox6.Enabled = false;
                comboBox4.Enabled = true;
                comboBox7.Enabled = true;

                if (!Edit) { dataGridView1.DataSource = GetTransfers(false).Where(z => z.Type == "out").ToList(); }
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
                if (!Edit) { dataGridView1.DataSource = GetTransfers(true).Where(z => z.Type == "internal").ToList(); }
            }
        }
        ////closing importer storage/exporter client options
        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            string Cname = comboBox4.Text.ToString();
            if (!Edit)
            {
                if (comboBox7.Text != "none")
                {
                    string Sname = comboBox7.Text.ToString();
                    dataGridView1.DataSource = GetTransfers(false).Where(z => z.Type == "out" && z.ClientName == Cname && z.ExporterStorageName == Sname).ToList();
                }
            }

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
            string Cname = comboBox6.Text.ToString();
            if (!Edit)
            {
                if (comboBox5.Text != "none")
                {
                    string Sname = comboBox5.Text.ToString();
                    dataGridView1.DataSource = GetTransfers(false).Where(z => z.Type == "in" && z.ClientName == Cname && z.ImporterStorageName == Sname).ToList();
                }
            }

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
        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            string Sname = comboBox5.Text.ToString();
            if (!Edit)
            {
                if (comboBox6.Text != "none")
                {
                    string Cname = comboBox6.Text.ToString();
                    dataGridView1.DataSource = GetTransfers(false).Where(z => z.Type == "in" && z.ClientName == Cname && z.ImporterStorageName == Sname).ToList();
                }
                else if (comboBox7.Text != "none")
                {
                    string Sname2 = comboBox7.Text.ToString();
                    dataGridView1.DataSource = GetTransfers(true).Where(z => z.Type == "internal" && z.ImporterStorageName == Sname && z.ExporterStorageName == Sname2).ToList();
                }
            }
        }

        private void comboBox7_SelectedValueChanged(object sender, EventArgs e)
        {
            string Sname = comboBox7.Text.ToString();
            if (!Edit)
            {
                if (comboBox4.Text != "none")
                {
                    string Cname = comboBox4.Text.ToString();
                    dataGridView1.DataSource = GetTransfers(false).Where(z => z.Type == "out" && z.ClientName == Cname && z.ExporterStorageName == Sname).ToList();
                }
                else if (comboBox5.Text != "none")
                {
                    string Sname2 = comboBox5.Text.ToString();
                    dataGridView1.DataSource = GetTransfers(true).Where(z => z.Type == "internal" && z.ExporterStorageName == Sname && z.ImporterStorageName == Sname2).ToList();
                }

            }
        }
        //edit transactions button
        private void button4_Click(object sender, EventArgs e)
        {
            comboBox13.Enabled = true;
            textBox13.Enabled = true;
            button5.Visible = true;
            button10.Visible = true;
            Edit = true;
            if (dataGridView1.SelectedRows.Count > 0)
                dataGridView1_SelectionChanged(null, null);
        }

        //submit edits button
        private void button5_Click(object sender, EventArgs e)
        {

            button5.Visible = false;
            button10.Visible = false;
            Edit = false;

            Transfers selectedTransfer = null;

            if (dataGridView1.CurrentRow?.DataBoundItem is Transfers currentTransfer)
            {
                selectedTransfer = currentTransfer;
            }
            else if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].DataBoundItem is Transfers selectedRowTransfer)
            {
                selectedTransfer = selectedRowTransfer;
            }

            if (selectedTransfer == null)
            {
                MessageBox.Show("Please select a row to edit.");
                return;
            }

            if (string.IsNullOrWhiteSpace(comboBox13.Text) || string.IsNullOrWhiteSpace(textBox13.Text))
            {
                MessageBox.Show("Item name and unit count are required.");
                return;
            }

            selectedTransfer.Type = comboBox2.SelectedItem?.ToString();
            selectedTransfer.Move = (bool?)comboBox3.SelectedItem;
            selectedTransfer.TransferDate = dateTimePicker1.Value;
            selectedTransfer.ProductionDate = dateTimePicker2.Value;
            selectedTransfer.ExpiryDate = dateTimePicker3.Value;
            selectedTransfer.ItemName = comboBox13.Text.Trim();
            selectedTransfer.UnitCount = int.TryParse(textBox13.Text.Trim(), out int count) ? count : 0;

            selectedTransfer.ImporterStorageName = comboBox5.Text == "none" ? null : comboBox5.Text;
            selectedTransfer.ExporterStorageName = comboBox7.Text == "none" ? null : comboBox7.Text;

            if (selectedTransfer.Type == "in")
            {
                selectedTransfer.ClientName = comboBox6.Text == "none" ? null : comboBox6.Text;
            }
            else if (selectedTransfer.Type == "out")
            {
                selectedTransfer.ClientName = comboBox4.Text == "none" ? null : comboBox4.Text;
            }
            else
            {
                selectedTransfer.ClientName = null;
            }

            db.SaveChanges();

            dataGridView1.DataSource = GetTransfers(selectedTransfer.Move == true);
        }

        //discard edits button
        private void button10_Click(object sender, EventArgs e)
        {
            button5.Visible = false;
            button10.Visible = false;
            Edit = false;
            comboBox13.Enabled = false;
            textBox13.Enabled = false;
        }
        //grid selection
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (!Edit || dataGridView1.SelectedRows.Count == 0) return;

            var selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow.DataBoundItem is not Transfers transfer) return;


            comboBox3.SelectedItem = transfer.Move == true;
            comboBox2.SelectedItem = transfer.Type;
            comboBox13.SelectedItem = transfer.ItemName ?? "none";
            textBox13.Text = transfer.UnitCount?.ToString() ?? "0";
            dateTimePicker1.Value = transfer.TransferDate ?? DateTime.Today;
            dateTimePicker2.Value = transfer.ProductionDate ?? DateTime.Today;
            dateTimePicker3.Value = transfer.ExpiryDate ?? DateTime.Today;


            comboBox4.SelectedItem = "none";
            comboBox6.SelectedItem = "none";
            comboBox5.SelectedItem = "none";
            comboBox7.SelectedItem = "none";


            if (transfer.Type == "in")
            {
                comboBox6.SelectedItem = transfer.ClientName ?? "none"; // Exporter client
                comboBox5.SelectedItem = transfer.ImporterStorageName ?? "none";
                comboBox7.SelectedItem = transfer.ExporterStorageName ?? "none";
            }
            else if (transfer.Type == "out")
            {
                comboBox4.SelectedItem = transfer.ClientName ?? "none"; // Importer client
                comboBox5.SelectedItem = transfer.ImporterStorageName ?? "none";
                comboBox7.SelectedItem = transfer.ExporterStorageName ?? "none";
            }
            else if (transfer.Type == "internal")
            {
                comboBox5.SelectedItem = transfer.ImporterStorageName ?? "none";
                comboBox7.SelectedItem = transfer.ExporterStorageName ?? "none";
            }
        }

        //Delete transfer
        private void button9_Click(object sender, EventArgs e)
        {
            // Get selected transfer safely
            Transfers selectedTransfer = null;

            if (dataGridView1.CurrentRow?.DataBoundItem is Transfers currentTransfer)
            {
                selectedTransfer = currentTransfer;
            }
            else if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].DataBoundItem is Transfers selectedRowTransfer)
            {
                selectedTransfer = selectedRowTransfer;
            }

            if (selectedTransfer == null)
            {
                MessageBox.Show("Please select a row to delete.");
                return;
            }

            // Ask for confirmation
            var confirm = MessageBox.Show("Are you sure you want to delete this transfer?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            // Delete and save
            db.Transfers.Remove(selectedTransfer);
            db.SaveChanges();

            // Refresh grid
            dataGridView1.DataSource = GetTransfers(selectedTransfer.Move == true);
        }










        //disposing connection
    }
}

