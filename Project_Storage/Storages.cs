using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Storage
{
     class Storages
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
