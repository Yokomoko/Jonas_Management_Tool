namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GP_Temp_InvoiceLedger
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Type { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime Date { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string CustRef { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(255)]
        public string CustName { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(255)]
        public string Address { get; set; }

        [Key]
        [Column(Order = 6, TypeName = "money")]
        public decimal Gross { get; set; }

        [StringLength(255)]
        public string UserDefined { get; set; }

        [StringLength(50)]
        public string SaleDocument { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "money")]
        public decimal Net { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "money")]
        public decimal VAT { get; set; }

        [Column(TypeName = "money")]
        public decimal? OTTA { get; set; }
    }
}
