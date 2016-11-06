using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace BL_JonasSageImporter {
    public class ConnectionProperties {

        public static string GetConnectionString() {
#if DEBUG
            return "Purchase_SaleLedgerEntities";
#else
            return "Purchase_SaleLedgerEntities_Live";
#endif

        }
    }
}
