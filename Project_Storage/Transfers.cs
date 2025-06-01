using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Storage
{
     class Transfers
    {
        public int TransferId { get; set; }  // Primary Key

        [MaxLength(20)]
        public string Type { get; set; }     // In / Out

        public bool Move { get; set; }       // true = internal storage move

     

        [MaxLength(100)]
        public string? ExporterClientName { get; set; }
        public Clients ExporterClient { get; set; }

        [MaxLength(100)]
        public string? ExporterStorageName { get; set; }
        public Storages ExporterStorage { get; set; }

        [MaxLength(100)]
        public string? ImporterClientName { get; set; }
        public Clients ImporterClient { get; set; }

        [MaxLength(100)]
        public string? ImporterStorageName { get; set; }
        public Storages ImporterStorage { get; set; }

        public DateTime TransferDate { get; set; }

        [MaxLength(100)]
        public string? ItemName { get; set; }
        public Items Item { get; set; }

        public DateTime ProductionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UnitCount { get; set; }
    }
}
