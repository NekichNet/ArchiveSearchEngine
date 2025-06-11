using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArchiveSearchEngine.Database
{
    public class HistoryTable
    {
        private SqliteConnection _connection;

        public HistoryTable(SqliteConnection connection) {
            _connection = connection;

            new SqliteCommand("CREATE TABLE IF NOT EXISTS HistoryTable (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "archive_id INTEGER NOT NULL," +
                "username TEXT NOT NULL," +
                "datetime_taken TEXT NOT NULL," +
                "datetime_returned TEXT)", _connection).ExecuteNonQuery();
        }

        // Adding a row in HistoryTable, pointing at user, taken document and current datetime.
        // Does nothing, if doc is not available
        public void TakeDocument(string username, int documentId)
        {
            MessageBox.Show("Док взятие");
            if (IsDocumentAvailable(documentId))
            {
                MessageBox.Show("Док взят");
                new SqliteCommand($"INSERT INTO HistoryTable " +
                    $"(archive_id, username, datetime_taken) VALUES " +
                    $"({documentId}, '{username}', '{DateTime.Now}')", _connection).ExecuteNonQuery();
            }
        }

        // Checks, if doc was not returned back to archive (false), or available (true)
        public bool IsDocumentAvailable(int documentId)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT id FROM HistoryTable WHERE datetime_returned IS NULL AND id = '{documentId}'",
                _connection).ExecuteReader())
            {
                MessageBox.Show(!reader.HasRows ? "true" : "false");
                return !reader.HasRows;
            }
        }

        // Do document available, whoever took this doc. Does nothing, if doc is already available
        public void ReturnDocument(int documentId)
        {
            
            int value = new SqliteCommand($"UPDATE HistoryTable SET " +
                $"datetime_returned='{DateTime.Now}' WHERE " +
                $"datetime_returned IS NULL AND archive_id = {documentId}",
                _connection).ExecuteNonQuery();
            MessageBox.Show($"{value}");
        }

        // Returns a username of user, who lastly took this doc (throws exception, if doc is available)
        public string UserWhoTook(int documentId)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT username FROM HistoryTable WHERE " +
                $"datetime_returned IS NULL AND archive_id = {documentId} " +
                $"ORDER BY id DESC LIMIT 1",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    throw new Exception("Документ доступен в архиве");
                }
            }
        }
    }
}
