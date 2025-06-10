using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine.Database
{
    class HistoryTable
    {
        private SqliteConnection _connection;

        public HistoryTable(SqliteConnection connection) {
            _connection = connection;

            new SqliteCommand("CREATE TABLE IF NOT EXISTS HistoryTable (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "archive_id INTEGER NOT NULL," +
                "user_id INTEGER NOT NULL," +
                "date_taken DATE NOT NULL," +
                "time_taken TIME NOT NULL," +
                "date_returned DATE," +
                "time_returned TIME)", _connection).ExecuteNonQuery();
        }
    }
}
