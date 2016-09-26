namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Configuration")]
    public partial class Configuration
    { 
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Label { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ConfigSetting { get; set; }
    }
}
