using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Ado_net
{
    public interface ISaveToDB<T>
    {
        string Url { get; set; }
        void SaveList(IGetFromAPI<T> getFrom);
        bool ExecuteInTransaction(DbConnection connection, List<DbCommand> commands);
        void TransactionFail(Exception exception, DbTransaction transaction);
    }
}
