namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SaleLedger")]
    public partial class SaleLedger
    {
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [StringLength(30)]
        public string CustRef { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }

        public int? GL { get; set; }

        [StringLength(255)]
        public string UniqueID { get; set; }

        public string ItemDescription { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Qty { get; set; }

        [Column(TypeName = "money")]
        public decimal Net { get; set; }

        [Column(TypeName = "money")]
        public decimal Tax { get; set; }

        [Column(TypeName = "money")]
        public decimal Gross { get; set; }

        [Column(TypeName = "money")]
        public decimal Profit { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(3)]
        public string Currency { get; set; }

        public string DeliveryAddress { get; set; }

        [StringLength(20)]
        public string CustOrderNo { get; set; }

        [StringLength(50)]
        public string InvoiceNo { get; set; }

        [StringLength(50)]
        public string CustName { get; set; }

        [Column(TypeName = "money")]
        public decimal? Cost { get; set; }

        [StringLength(50)]
        public string ImportType { get; set; }

        [StringLength(255)]
        public string Category { get; set; }

        [StringLength(255)]
        public string SiteName { get; set; }

        public short? MiniPack { get; set; }

        [StringLength(255)]
        public string SiteSurveyDate { get; set; }

        public string BacklogComments { get; set; }

        [StringLength(255)]
        public string Deposit { get; set; }

        [StringLength(255)]
        public string AssignedTo { get; set; }

        [StringLength(255)]
        public string MegJobNo { get; set; }

        public short? DirectDebit { get; set; }

        [StringLength(255)]
        public string Spare1 { get; set; }

        [StringLength(255)]
        public string Spare2 { get; set; }

        public long Id { get; set; }
    }
}
