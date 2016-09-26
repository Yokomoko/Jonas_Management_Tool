namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PostedInvoicesExtended")]
    public partial class PostedInvoicesExtended
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long JournalEntry { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string Series { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime TrxDate { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "date")]
        public DateTime OriginatingTrxDate { get; set; }

        public short? JonasGroup { get; set; }

        [StringLength(255)]
        public string GroupName { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(30)]
        public string LongReference { get; set; }

        [StringLength(255)]
        public string MaintTypeDescription { get; set; }

        [StringLength(255)]
        public string ReportingDescription { get; set; }

        [Key]
        [Column(Order = 6, TypeName = "money")]
        public decimal DebitAmount { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "money")]
        public decimal CreditAmount { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(50)]
        public string Reference { get; set; }

        [StringLength(30)]
        public string OriginatingMasterID { get; set; }

        [StringLength(50)]
        public string OriginatingMasterName { get; set; }

        [StringLength(30)]
        public string OriginatingDocumentNo { get; set; }

        [Key]
        [Column(Order = 9)]
        public bool Voided { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(50)]
        public string OriginatingTrxSource { get; set; }

        [StringLength(50)]
        public string OriginatingTrxType { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(50)]
        public string OriginatingType { get; set; }

        [StringLength(50)]
        public string UserWhoPosted { get; set; }
    }
}
