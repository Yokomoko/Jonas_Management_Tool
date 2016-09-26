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
        [Key]
        [Column(Order = 0, TypeName = "date")]
        public DateTime Date { get; set; }

        [StringLength(30)]
        public string CustRef { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }

        public int? GL { get; set; }

        [StringLength(255)]
        public string UniqueID { get; set; }

        public string ItemDescription { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal Qty { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "money")]
        public decimal Net { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "money")]
        public decimal Tax { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "money")]
        public decimal Gross { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "money")]
        public decimal Profit { get; set; }

        [Key]
        [Column(Order = 6)]
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
    }
}
