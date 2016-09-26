namespace BL_JonasSageImporter {
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class JonasSageImporter : DbContext {
        public JonasSageImporter()
            : base("name=JonasSageImporter") {
        }

        public virtual DbSet<AdminStatus> AdminStatuses { get; set; }
        public virtual DbSet<MaintenanceType> MaintenanceTypes { get; set; }
        public virtual DbSet<Number> Numbers { get; set; }
        public virtual DbSet<TerminalType> TerminalTypes { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<GP_Temp_InvoiceLedger> GP_Temp_InvoiceLedger { get; set; }
        public virtual DbSet<GP_Temp_PostedInvoices> GP_Temp_PostedInvoices { get; set; }
        public virtual DbSet<JonasGroup> JonasGroups { get; set; }
        public virtual DbSet<JonasType> JonasTypes { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<OutstandingInvoice> OutstandingInvoices { get; set; }
        public virtual DbSet<PostedInvoice> PostedInvoices { get; set; }
        public virtual DbSet<SaleLedger> SaleLedgers { get; set; }
        public virtual DbSet<OutstandingInvoicesExtended> OutstandingInvoicesExtendeds { get; set; }
        public virtual DbSet<PostedInvoicesExtended> PostedInvoicesExtendeds { get; set; }
        public virtual DbSet<SaleLedgerExtended> SaleLedgerExtendeds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<AdminStatus>()
                .Property(e => e.AdminStatusName)
                .IsUnicode(false);

            modelBuilder.Entity<TerminalType>()
                .Property(e => e.TerminalTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<GP_Temp_InvoiceLedger>()
                .Property(e => e.Gross)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GP_Temp_InvoiceLedger>()
                .Property(e => e.Net)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GP_Temp_InvoiceLedger>()
                .Property(e => e.VAT)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GP_Temp_InvoiceLedger>()
                .Property(e => e.OTTA)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GP_Temp_PostedInvoices>()
                .Property(e => e.DebitAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GP_Temp_PostedInvoices>()
                .Property(e => e.CreditAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<GP_Temp_PostedInvoices>()
                .Property(e => e.OriginatingMasterID)
                .IsUnicode(false);

            modelBuilder.Entity<GP_Temp_PostedInvoices>()
                .Property(e => e.OriginatingDocumentNo)
                .IsUnicode(false);

            modelBuilder.Entity<OutstandingInvoice>()
                .Property(e => e.OriginalTrxAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoice>()
                .Property(e => e.CurrentTrxAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoice>()
                .Property(e => e.Days0)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoice>()
                .Property(e => e.Days1)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoice>()
                .Property(e => e.Days2)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoice>()
                .Property(e => e.Days3)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoice>()
                .Property(e => e.Days4)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoice>()
                .Property(e => e.Days5)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoice>()
                .Property(e => e.Days6)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PostedInvoice>()
                .Property(e => e.DebitAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PostedInvoice>()
                .Property(e => e.CreditAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PostedInvoice>()
                .Property(e => e.OriginatingMasterID)
                .IsUnicode(false);

            modelBuilder.Entity<PostedInvoice>()
                .Property(e => e.OriginatingDocumentNo)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.CustRef)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.Qty)
                .HasPrecision(16, 6);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.Net)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.Tax)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.Gross)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.Profit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.Cost)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.SiteSurveyDate)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.BacklogComments)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.Deposit)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.AssignedTo)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.MegJobNo)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedger>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<OutstandingInvoicesExtended>()
                .Property(e => e.OriginalTrxAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoicesExtended>()
                .Property(e => e.CurrentTrxAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoicesExtended>()
                .Property(e => e.Current)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoicesExtended>()
                .Property(e => e.Days31_60)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoicesExtended>()
                .Property(e => e.Days61_90)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoicesExtended>()
                .Property(e => e.Days91_180)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoicesExtended>()
                .Property(e => e.Days181_270)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoicesExtended>()
                .Property(e => e.Days271_364)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OutstandingInvoicesExtended>()
                .Property(e => e.Days365_Plus)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PostedInvoicesExtended>()
                .Property(e => e.DebitAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PostedInvoicesExtended>()
                .Property(e => e.CreditAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PostedInvoicesExtended>()
                .Property(e => e.OriginatingMasterID)
                .IsUnicode(false);

            modelBuilder.Entity<PostedInvoicesExtended>()
                .Property(e => e.OriginatingDocumentNo)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.CustRef)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.QtyValue)
                .HasPrecision(16, 6);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.NetValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.TaxValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.GrossValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.ProfitValue)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.SiteSurveyDate)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.BacklogComments)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.Deposit)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.AssignedTo)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.MegJobNo)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.Spare1)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.TerminalTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.Spare2)
                .IsUnicode(false);

            modelBuilder.Entity<SaleLedgerExtended>()
                .Property(e => e.AdminStatusName)
                .IsUnicode(false);
        }
    }
}
