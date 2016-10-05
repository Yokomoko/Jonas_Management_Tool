namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Statuses")]
    public partial class Status
    {
        public int StatusId { get; set; }

        public int? StatusName { get; set; }

        public bool? StatusEnabled { get; set; }

        public short? SortOrder { get; set; }
    }
}
