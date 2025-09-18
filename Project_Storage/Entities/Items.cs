using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_Storage.Entities
{
    public class Items
    {
        [Key]
        [MaxLength(100)]
        public string Name { get; set; }  // Primary Key

        [MaxLength(50)]
        public string Code { get; set; }

        public ICollection<Stored> StoredEntries { get; set; }
        public ICollection<Transfers> Transfers { get; set; }
    }
}
