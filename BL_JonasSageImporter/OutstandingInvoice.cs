namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutstandingInvoice
    {
        [Required]
        [StringLength(50)]
        public string CustRef { get; set; }

        [Required]
        [StringLength(255)]
        public string CustName { get; set; }

        [StringLength(255)]
        public string ClassID { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? DueDate { get; set; }

        [Column(TypeName = "money")]
        public decimal OriginalTrxAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal CurrentTrxAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal Days0 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days1 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days2 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days3 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days4 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days5 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days6 { get; set; }

        public long Id { get; set; }
    }
}
