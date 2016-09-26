namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OutstandingInvoice
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
    }
}
