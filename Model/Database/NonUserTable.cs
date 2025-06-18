using Microsoft.Data.Sqlite;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine.Model.Database
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

        public void NewUnit(string fullname, string post, string struct_division)
        {
            fullname = fullname.Replace("'", "");
            post = post.Replace("'", "");
            struct_division = struct_division.Replace("'", "");

            new SqliteCommand($"INSERT INTO NonUserTable " +
                $"(fullname, post, struct_division) VALUES " +
                $"('{fullname}', '{post}', '{struct_division}')", _connection).ExecuteNonQuery();
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

        // Returns unit found by his id (throws an exception, if not exists)
        public NonUser GetUnit(int id)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM NonUserTable WHERE id = {id}",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new NonUser(Convert.ToInt32(reader["id"]), (string)reader["fullname"],
                        (string)reader["post"], (string)reader["struct_division"]);
                }
                else
                {
                    throw new Exception($"Человек с id: {id} не найден в справочнике");
                }
            }
        }

        // Changes [fullname, post, struct_division] of unit with exact id
        public void UpdateUnit(NonUser unit)
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
        public void DeleteUnit(int id)
        {
            new SqliteCommand($"DELETE FROM NonUserTable WHERE id = {id}", _connection).ExecuteNonQuery();
        }

        // Returns true, if unit with exact id exists in database
        public bool UnitExists(int id)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT id FROM NonUserTable WHERE id = {id}",
                _connection).ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        public void ImportFromCSV(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath);

            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                NewUnit(values[0], values[1], values[2]);
            }
        }
    }
}
