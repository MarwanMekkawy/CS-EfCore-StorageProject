namespace Project_Storage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
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
 
    }
}
