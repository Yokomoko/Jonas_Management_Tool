namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EntryType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short EntryTypeNo { get; set; }

        [StringLength(20)]
        public string EntryTypeDescription { get; set; }
    }
}
