namespace Project_Storage.Entities
{
    public class Stored
    {
        public int Id { get; set; }  // primary key

        public string StorageName { get; set; }
        public Storages Storage { get; set; }

        public string ItemName { get; set; }
        public Items Item { get; set; }

        public int TotalUnits { get; set; }
    }
}
