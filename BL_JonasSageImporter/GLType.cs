namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GLType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GLNo { get; set; }

        [StringLength(255)]
        public string GLDescription { get; set; }
    }
}
