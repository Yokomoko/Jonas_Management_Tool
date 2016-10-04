namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GP_Temp_InvoiceLedger
    {
        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(50)]
        public string CustRef { get; set; }

        [Required]
        [StringLength(255)]
        public string CustName { get; set; }

        [StringLength(50)]
        public string OrderNo { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Column(TypeName = "money")]
        public decimal Gross { get; set; }

        [StringLength(255)]
        public string UserDefined { get; set; }

        [StringLength(50)]
        public string SaleDocument { get; set; }

        [Column(TypeName = "money")]
        public decimal Net { get; set; }

        [Column(TypeName = "money")]
        public decimal VAT { get; set; }

        [Column(TypeName = "money")]
        public decimal? OTTA { get; set; }

        public long Id { get; set; }
    }
}
