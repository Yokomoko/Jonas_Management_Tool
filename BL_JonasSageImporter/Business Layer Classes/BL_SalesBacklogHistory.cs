using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL_JonasSageImporter.Business_Layer_Classes
{
    class BL_SalesBacklogHistory : BL_JonasSageImporter.SalesBacklogHistory
    {

        //Gets value by date
        public static IQueryable<BL_JonasSageImporter.SalesBacklogHistory> GetByDate(DateTime dt)
        {
            var context = new Purchase_SaleLedgerEntities(ConnectionProperties.GetConnectionString());
            var sbh = from t in context.SalesBacklogHistories
                      where (t.SbhDate.Date == dt.Date)
                      select t;
            return sbh;
        }


    }
}
