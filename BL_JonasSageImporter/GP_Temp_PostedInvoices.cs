namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GP_Temp_PostedInvoices
    {
        public long JournalEntry { get; set; }

        [Required]
        [StringLength(30)]
        public string Series { get; set; }

        [Column(TypeName = "date")]
        public DateTime TrxDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime OriginatingTrxDate { get; set; }

        [Required]
        [StringLength(30)]
        public string AccountNumber { get; set; }

        [StringLength(255)]
        public string AccountDescription { get; set; }

        [Column(TypeName = "money")]
        public decimal DebitAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal CreditAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string Reference { get; set; }

        [StringLength(30)]
        public string OriginatingMasterID { get; set; }

        [StringLength(50)]
        public string OriginatingMasterName { get; set; }

        [StringLength(30)]
        public string OriginatingDocumentNo { get; set; }

        [StringLength(3)]
        public string Voided { get; set; }

        [Required]
        [StringLength(50)]
        public string OriginatingTrxSource { get; set; }

        [StringLength(50)]
        public string OriginatingTrxType { get; set; }

        [Required]
        [StringLength(50)]
        public string OriginatingType { get; set; }

        [StringLength(50)]
        public string UserWhoPosted { get; set; }

        public long Id { get; set; }
    }
}
