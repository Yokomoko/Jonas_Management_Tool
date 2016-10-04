namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MaintenanceGLBridge")]
    public partial class MaintenanceGLBridge
    {
        public short? MaintenanceType { get; set; }

        public int? GLNumber { get; set; }

        public long Id { get; set; }
    }
}
