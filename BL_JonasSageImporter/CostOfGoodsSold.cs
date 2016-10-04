namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CostOfGoodsSold")]
    public partial class CostOfGoodsSold
    {
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string CogsCompanyName { get; set; }

        [Required]
        [StringLength(255)]
        public string CogsSiteName { get; set; }

        public int CogsStatus { get; set; }

        [Required]
        [StringLength(30)]
        public string CogsGPCode { get; set; }

        public DateTime? CogsDueDate { get; set; }

        [Required]
        [StringLength(255)]
        public string CogsGPCategory { get; set; }

        [Required]
        public string CogsDescription { get; set; }

        public long CogsSalesOrderId { get; set; }

        public decimal CogsItemQuantity { get; set; }

        [Column(TypeName = "money")]
        public decimal CogsItemListPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal CogsItemBuyPrice { get; set; }
    }
}
