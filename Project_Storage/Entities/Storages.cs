using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_Storage.Entities
{
    public class Storages
    {
        [Key]
        [MaxLength(100)]
        public string Name { get; set; }  // Primary Key

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string Supervisor { get; set; }

        public ICollection<Stored> StoredItems { get; set; }
        public ICollection<Transfers> ExportTransfers { get; set; }
        public ICollection<Transfers> ImportTransfers { get; set; }
    }
}
