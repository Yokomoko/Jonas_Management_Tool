namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class JonasGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short GroupNo { get; set; }

        [StringLength(255)]
        public string GroupName { get; set; }
    }
}
