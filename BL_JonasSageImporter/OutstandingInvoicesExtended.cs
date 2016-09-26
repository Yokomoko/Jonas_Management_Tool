namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OutstandingInvoicesExtended")]
    public partial class OutstandingInvoicesExtended
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CustRef { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string CustName { get; set; }

        [StringLength(255)]
        public string ClassID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string DocumentNumber { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Type { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? DueDate { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "money")]
        public decimal OriginalTrxAmount { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "money")]
        public decimal CurrentTrxAmount { get; set; }

        [Key]
        [Column(Order = 6, TypeName = "money")]
        public decimal Current { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days31_60 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days61_90 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days91_180 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days181_270 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days271_364 { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days365_Plus { get; set; }
    }
}
