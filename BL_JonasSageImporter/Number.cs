namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Number
    {
        [Key]
        [Column("Number")]
        public short Number1 { get; set; }
    }
}
