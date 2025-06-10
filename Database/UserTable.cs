using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine.Database
{
    public class UserTable
    {
        SqliteConnection _connection { get; }

        public UserTable(SqliteConnection connection, bool add_default_admin)
        {
            _connection = connection;

            new SqliteCommand("CREATE TABLE IF NOT EXISTS UserTable (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "username TEXT NOT NULL," +
                "password_hash TEXT NOT NULL," +
                "fullname TEXT NOT NULL," +
                "post TEXT NOT NULL," +
                "struct_division TEXT NOT NULL," +
                "is_admin INTEGER NOT NULL)", _connection).ExecuteNonQuery();

            // adding a default admin account, if database was recreated
            if (add_default_admin)
            {
                using (SHA256 hash = SHA256.Create())
                {
                    new SqliteCommand($"INSERT INTO UserTable " +
                    $"(username, password_hash, fullname, post, struct_division, is_admin) " +
                    $"VALUES ('admin', " +
                    $"{Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes("admin")))}, " +
                    $"'Администратор по умолчанию', " +
                    $"'Администратор по умолчанию', " +
                    $"'Администратор по умолчанию', " +
                    $"1)", _connection).ExecuteNonQuery();
                }
            }
        }

        public void NewUser(User user, string password)
        {
            using (SHA256 hash = SHA256.Create())
            {
                new SqliteCommand($"INSERT INTO UserTable " +
                    $"(username, password_hash, fullname, post, struct_division, is_admin) VALUES " +
                    $"({user.Username}, {Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(password)))}, " +
                    $"{user.Fullname}, {user.Post}, " +
                    $"{user.StructDivision}, {user.IsAdmin})", _connection);
            }
        }

        // Only if user exists
        public User GetUser(string username)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM UserTable WHERE username == {username}",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new User((string)reader["username"], (string)reader["fullname"],
                        (string)reader["post"], (string)reader["struct_division"], (bool)reader["is_admin"]);
                }
                else
                {
                    throw new Exception($"Пользователь с username: {username} не найден");
                }
            }
        }
    }
}