namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sage_Temp_InvoiceLedger
    {
        public DateTime? Date { get; set; }

        [StringLength(30)]
        public string CustRef { get; set; }

        public int? GL { get; set; }

        [StringLength(255)]
        public string UniqueID { get; set; }

        public string ItemDescription { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        [Column(TypeName = "money")]
        public decimal? Net { get; set; }

        [Column(TypeName = "money")]
        public decimal? Tax { get; set; }

        [Column(TypeName = "money")]
        public decimal? Gross { get; set; }

        [Column(TypeName = "money")]
        public decimal? Profit { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(1)]
        public string Currency { get; set; }

        public string DeliveryAddress { get; set; }

        [StringLength(20)]
        public string CustOrderNo { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [StringLength(50)]
        public string CustName { get; set; }

        public long Id { get; set; }
    }
}
