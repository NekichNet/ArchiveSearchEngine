using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine.Database
{
    public class DocumentTable
    {
        SqliteConnection _connection;

        public DocumentTable(SqliteConnection connection)
        {
            _connection = connection;

            new SqliteCommand("CREATE TABLE IF NOT EXISTS DocumentTable (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "registration_num TEXT NOT NULL," +
                "volume_num TEXT NOT NULL," +
                "book_num TEXT NOT NULL," +
                "content_quantity INTEGER NOT NULL," +
                "inventory_date DATE," +
                "inventory_num TEXT," +
                "object_index TEXT NOT NULL," +
                "object_name TEXT NOT NULL," +
                "rack INTEGER NOT NULL," +
                "shelf INTEGER NOT NULL," +
                "expiring_in TEXT NOT NULL," +
                "documents_date DATE NOT NULL," +
                "case_num TEXT NOT NULL," +
                "destruct_act_num TEXT," +
                "destruct_act_date DATE," +
                "struct_division TEXT NOT NULL," +
                "gived_post TEXT NOT NULL," +
                "gived_fullname TEXT NOT NULL," +
                "achieved_post TEXT NOT NULL," +
                "achieved_fullname TEXT NOT NULL," +
                "note TEXT)", _connection).ExecuteNonQuery();
        }
    }
}
