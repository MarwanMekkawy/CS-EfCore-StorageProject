using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Storage
{
    class Clients
    {
        [Key]
        [MaxLength(100)]
        public string Name { get; set; }  // Primary Key

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(20)]
        public string Fax { get; set; }

        [MaxLength(20)]
        public string Mobile { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Website { get; set; }

        [MaxLength(20)]
        public string Type { get; set; }  // Importer / Exporter

        public ICollection<Transfers> ExportTransfers { get; set; }
        public ICollection<Transfers> ImportTransfers { get; set; }
    }
}
