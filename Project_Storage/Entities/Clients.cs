using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_Storage.Entities
{
    public class Clients
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

        public ICollection<Transfers> Transfers { get; set; }
    }
}
