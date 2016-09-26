namespace BL_JonasSageImporter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TerminalType
    {
        public int TerminalTypeId { get; set; }

        [StringLength(50)]
        public string TerminalTypeName { get; set; }
    }
}
