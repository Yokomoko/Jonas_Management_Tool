namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdminStatuses")]
    public partial class AdminStatus
    {
        public int AdminStatusId { get; set; }

        [StringLength(50)]
        public string AdminStatusName { get; set; }
    }
}
