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
        public DateTime LogDate { get; set; }

        [Required]
        public string ExcelPath { get; set; }

        [Required]
        [StringLength(50)]
        public string ImportType { get; set; }

        public long? NumberOfRowsImported { get; set; }

        public long Id { get; set; }
    }
}
