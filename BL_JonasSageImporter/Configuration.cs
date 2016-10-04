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
        [StringLength(50)]
        public string Label { get; set; }

        [Required]
        [StringLength(50)]
        public string ConfigSetting { get; set; }
    }
}
