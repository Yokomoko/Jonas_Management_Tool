namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Log")]
    public partial class Log
    {
        [Key]
        [Column(Order = 0)]
        public DateTime LogDate { get; set; }

        [Key]
        [Column(Order = 1)]
        public string ExcelPath { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string ImportType { get; set; }

        public long? NumberOfRowsImported { get; set; }
    }
}
