using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Project_Storage
{
    public partial class Form1 : Form
    {
        Context db = new Context();
        public Form1()
        {
            InitializeComponent();

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = true;

            GetItems();
            GetStorages();

            comboBox5.SelectedIndex = -1;
            comboBox6.SelectedIndex = -1;
            comboBox9.SelectedIndex = -1;
            comboBox5.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox5.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox6.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox6.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox9.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox9.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
        //fetch itmes
        private void GetItems()
        {
            Items items = new Items();
            List<string> Ilist = db.Items.Select(x => x.Name).ToList();
            comboBox5.DataSource = Ilist;
            comboBox9.DataSource = Ilist;
        }
        //fetch storages
        private void GetStorages()
        {
            Storages storages = new Storages();
            List<string> Slist = db.Storages.Select(x => x.Name).ToList();
            comboBox6.DataSource = Slist;
        }


        //F_ADD button
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            F_ADD Add = new F_ADD();
            Add.FormClosed += (s, args) => this.Show();
            Add.Show();
        }
        //F_Edit button
        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            F_Edit Edit = new F_Edit();
            Edit.FormClosed += (s, args) => this.Show();
            Edit.Show();
        }

        /////////////////////////////print buttons/////////////////////////////
        //Get item Store Period Report button
        private void button3_Click(object sender, EventArgs e)
        {

            string Iname = comboBox9.Text;
            if (!string.IsNullOrWhiteSpace(Iname))
            {
                int days = (int)numericUpDown1.Value;
                int months = (int)numericUpDown2.Value;
                int years = (int)numericUpDown3.Value;


                DateTime thresholdDate = DateTime.Now.AddDays(-days).AddMonths(-months).AddYears(-years);

                var results = db.Transfers.Where(t => t.TransferDate <= thresholdDate)
                    .Select(x => new { x.TransferId, x.Type, x.ClientName, x.ItemName, x.UnitCount, x.ProductionDate, x.ExpiryDate }).ToList();
                if (results.Count > 0)
                {
                    var ResultForm = new F_Result();
                    ResultForm.LoadResults(Iname, results);
                    ResultForm.ShowDialog();
                }
                else
                {
                    Iname = "No Results For " + "\"" + Iname + "\"";
                    var ResultForm = new F_Result();
                    ResultForm.LoadResults(Iname, results);
                    ResultForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("please pick an item name!");
            }

        }

        //Get Storage Report button
        private void button8_Click(object sender, EventArgs e)
        {
            string Sname = comboBox6.Text;
            if (!string.IsNullOrWhiteSpace(Sname))
            {
                DateTime Dfrom = dateTimePicker3.Value.Date;
                DateTime Dto = dateTimePicker4.Value.Date.Date.AddDays(1).AddTicks(-1);
                var results = db.Transfers.Where(t => t.TransferDate >= Dfrom && t.TransferDate <= Dto && (t.ImporterStorageName == Sname || t.ExporterStorageName == Sname))
                    .Select(x => new { x.TransferId, x.Type, x.Move, x.ClientName, x.ItemName, x.UnitCount, x.TransferDate, x.ProductionDate, x.ExpiryDate }).ToList();
                if (results.Count > 0)
                {
                    var ResultForm = new F_Result();
                    ResultForm.LoadResults(Sname, results);
                    ResultForm.ShowDialog();
                }
                else
                {
                    Sname = "No Results For " + "\"" + Sname + "\"";
                    var ResultForm = new F_Result();
                    ResultForm.LoadResults(Sname, results);
                    ResultForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("please pick a storage name!");
            }
        }

        //Almost expired items
        private void button4_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime threshold = today.AddDays(40);

            var result = db.Transfers
                .Where(t => t.ExpiryDate <= threshold && t.ImporterStorageName != null)
                .Select(t => new
                {
                    ItemName = t.ItemName,
                    DaysLeft = EF.Functions.DateDiffDay(today, t.ExpiryDate.Value),
                    ExpiryDate = t.ExpiryDate,
                    ProductionDate = t.ProductionDate,
                    TransferDate = t.TransferDate,
                    ImporterStorage = t.ImporterStorageName,
                    UnitCount = t.UnitCount
                })
                .ToList();
            if (result.Count > 0)
            {
                var ResultForm = new F_Result();
                ResultForm.LoadResults("Expires in Less than 40 days", result);
                ResultForm.ShowDialog();
            }
            else
            {
                var ResultForm = new F_Result();
                ResultForm.LoadResults("No Results Found", result);
                ResultForm.ShowDialog();
            }

        }

        //Get Transfer Report button
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dateTimePicker5.Value.Date;
            DateTime toDate = dateTimePicker6.Value.Date.Date.AddDays(1).AddTicks(-1);

            var results = db.Transfers
                .Where(t => t.TransferDate != null && t.TransferDate >= fromDate && t.TransferDate <= toDate)
                .Select(t => new
                {
                    ItemName = t.ItemName,
                    TransferDate = t.TransferDate,
                    ProductionDate = t.ProductionDate,
                    ExpiryDate = t.ExpiryDate,
                    ImporterStorage = t.ImporterStorageName,
                    ExporterStorage = t.ExporterStorageName,
                    UnitCount = t.UnitCount
                }).ToList();

            if (results.Count > 0)
            {
                var ResultForm = new F_Result();
                ResultForm.LoadResults($"Transfers from \n{fromDate} to {toDate}", results);
                ResultForm.ShowDialog();
            }
            else
            {
                var ResultForm = new F_Result();
                ResultForm.LoadResults("No Results Found", results);
                ResultForm.ShowDialog();
            }
        }

        //Get item Report button
        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            string Iname=comboBox5.Text;
            var storages = db.Transfers.Where(x=>x.ItemName==Iname&&x.ImporterStorageName!=null).Select(x=>new { x.ImporterStorageName,x.UnitCount }).ToList();
            dataGridView1.DataSource = storages;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            var selectedStorages = dataGridView1.SelectedRows.Cast<DataGridViewRow>().Select(r => r.Cells["ImporterStorageName"].Value?.ToString())
                          .Where(name => !string.IsNullOrEmpty(name)).Distinct().ToList();

            if (selectedStorages.Count == 0)
            {
                MessageBox.Show("Please select at least one storage from the grid.");
                return;
            }

            DateTime fromDate = dateTimePicker1.Value.Date;
            DateTime toDate = dateTimePicker2.Value.Date.Date.AddDays(1).AddTicks(-1);

            // Query transfers grid
            var results = db.Transfers
                .Where(t => t.TransferDate >= fromDate && t.TransferDate <= toDate && selectedStorages.Contains(t.ImporterStorageName)).Select(t => new
                {t.ItemName, t.UnitCount, t.TransferDate, t.ProductionDate, t.ExpiryDate, t.ImporterStorageName}).ToList();

            if (results.Count > 0)
            {
                var ResultForm = new F_Result();
                ResultForm.LoadResults($"Transfers in {selectedStorages} from \n{fromDate} to {toDate}", results);
                ResultForm.ShowDialog();
            }
            else
            {
                var ResultForm = new F_Result();
                ResultForm.LoadResults("No Results Found", results);
                ResultForm.ShowDialog();
            }
        }
        
    }
}
