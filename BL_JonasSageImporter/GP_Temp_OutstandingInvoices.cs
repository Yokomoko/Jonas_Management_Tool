namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GP_Temp_OutstandingInvoices
    {
        public string CustRef { get; set; }

        public string CustName { get; set; }

        public string ClassID { get; set; }

        public string TerritoryID { get; set; }

        public string DocumentNumber { get; set; }

        public string Type { get; set; }

        [StringLength(50)]
        public string Date { get; set; }

        [StringLength(50)]
        public string DueDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? OriginalTrxAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? CurrentTrxAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal? Days0 { get; set; }

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

        public long Id { get; set; }
    }
}
