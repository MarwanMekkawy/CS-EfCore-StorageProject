namespace Project_Storage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();  
            F_ADD Add = new F_ADD();
            Add.FormClosed += (s, args) => this.Show();
            Add.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            F_Edit Edit = new F_Edit();
            Edit.FormClosed += (s, args) => this.Show();
            Edit.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            F_Delete Delete = new F_Delete();
            Delete.FormClosed += (s, args) => this.Show();
            Delete.Show();
        }
    }
}
