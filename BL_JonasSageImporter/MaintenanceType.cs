namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MaintenanceType
    {
        [Key]
        [Column("MaintenanceType")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short MaintenanceType1 { get; set; }

        [StringLength(20)]
        public string LongReference { get; set; }

        [StringLength(255)]
        public string MaintTypeDescription { get; set; }

        [StringLength(255)]
        public string ReportingDescription { get; set; }

        public short? JonasGroup { get; set; }
    }
}
