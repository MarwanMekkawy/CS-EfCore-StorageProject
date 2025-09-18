using Project_Storage.Data;

namespace Project_Storage
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //////creating db
            using (var db = new Context())
            {
                db.Database.EnsureCreated();
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());


        }
    }
}