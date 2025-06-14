using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine.Database
{
    public class NonUserTable
    {
        SqliteConnection _connection { get; }

        public NonUserTable(SqliteConnection connection)
        {
            _connection = connection;

            new SqliteCommand("CREATE TABLE IF NOT EXISTS NonUserTable (" +
                "id INTEGER PRIMARY KEY, " +
                "fullname TEXT NOT NULL, " +
                "post TEXT NOT NULL, " +
                "struct_division TEXT NOT NULL)", _connection).ExecuteNonQuery();
        }

        public void NewUnit(NonUser unit)
        {
            unit.Fullname = unit.Fullname.Replace("'", "");
            unit.Post = unit.Post.Replace("'", "");
            unit.StructDivision = unit.StructDivision.Replace("'", "");

            new SqliteCommand($"INSERT INTO NonUserTable " +
                $"(fullname, post, struct_division) VALUES " +
                $"('{unit.Fullname}', '{unit.Post}', '{unit.StructDivision}')", _connection).ExecuteNonQuery();
        }

        // Returns list of units, what have promt in their username or fullname
        public List<NonUser> GetUnits(string promt)
        {
            List<NonUser> units = new List<NonUser>();

            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM NonUserTable WHERE " +
                $"fullname LIKE '%{promt}%' OR " +
                $"post LIKE '%{promt}%' OR " +
                $"struct_division LIKE '%{promt}%'",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        units.Add(new NonUser(Convert.ToInt32(reader["id"]), (string)reader["fullname"],
                        (string)reader["post"], (string)reader["struct_division"]));
                    }
                }
            }

            return units;
        }

        // Returns list of all units in database
        public List<NonUser> GetUnits()
        {
            List<NonUser> units = new List<NonUser>();

            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM NonUserTable",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        units.Add(new NonUser(Convert.ToInt32(reader["id"]), (string)reader["fullname"],
                        (string)reader["post"], (string)reader["struct_division"]));
                    }
                }
            }

            return units;
        }

        // Changes [fullname, post, struct_division] of unit with exact id
        public void UpdateUser(NonUser unit)
        {
            unit.Fullname = unit.Fullname.Replace("'", "");
            unit.Post = unit.Post.Replace("'", "");
            unit.StructDivision = unit.StructDivision.Replace("'", "");

            new SqliteCommand($"UPDATE NonUserTable SET " +
                $"fullname='{unit.Fullname}', " +
                $"post='{unit.Post}', " +
                $"struct_division='{unit.StructDivision}' " +
                $"WHERE id = '{unit.Id}'",
                _connection).ExecuteNonQuery();
        }

        // Deletes unit with exact id
        public void DeleteUser(int id)
        {
            new SqliteCommand($"DELETE FROM NonUserTable WHERE id = {id}", _connection).ExecuteNonQuery();
        }
    }
}
