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
                NewUser(new User("admin", "Администратор по умолчанию", "Администратор по умолчанию",
                    "Администратор по умолчанию", true), "admin");
            }
        }

        public void NewUser(User user, string password)
        {
            using (SHA256 hash = SHA256.Create())
            {
                new SqliteCommand($"INSERT INTO UserTable " +
                    $"(username, password_hash, fullname, post, struct_division, is_admin) VALUES " +
                    $"('{user.Username}', '{Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(password)))}', " +
                    $"'{user.Fullname}', '{user.Post}', " +
                    $"'{user.StructDivision}', {(user.IsAdmin? 1 : 0)})", _connection).ExecuteNonQuery();
            }
        }

        // Returns user found by his username (throws an exception, if not exists)
        public User GetUser(string username)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM UserTable WHERE username = '{username}'",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new User((string)reader["username"], (string)reader["fullname"],
                        (string)reader["post"], (string)reader["struct_division"], Convert.ToInt32(reader["is_admin"]) == 1);
                }
                else
                {
                    throw new Exception($"Пользователь с username: {username} не найден");
                }
            }
        }

        // Returns list of users, what have promt in their username or fullname
        public List<User> GetUsers(string promt)
        {
            List<User> users = new List<User>();

            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM UserTable WHERE username LIKE '{promt}' OR fullname LIKE '{promt}'",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(new User((string)reader["username"], (string)reader["fullname"],
                        (string)reader["post"], (string)reader["struct_division"], Convert.ToInt32(reader["is_admin"]) == 1));
                    }
                }
            }

            return users;
        }

        // Returns list of all users in database
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM UserTable",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(new User((string)reader["username"], (string)reader["fullname"],
                        (string)reader["post"], (string)reader["struct_division"], Convert.ToInt32(reader["is_admin"]) == 1));
                    }
                }
            }

            return users;
        }

        // Changes [password_hash, fullname, post, struct_division] of user with (username == user.Username)
        public void UpdateUser(User user, string password)
        {
            using (SHA256 hash = SHA256.Create())
            {
                new SqliteCommand($"UPDATE UserTable SET " +
                    $"password_hash={Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(password)))}, " +
                    $"fullname={user.Fullname}, " +
                    $"post={user.Post}, " +
                    $"struct_division={user.StructDivision} " +
                    $"WHERE username = {user.Username}",
                    _connection).ExecuteNonQuery();
            }
        }

        // Returns true, if user with these username and password exists.
        public bool CheckUser(string username, string password)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT password_hash FROM UserTable WHERE username = '{username}'",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    using (SHA256 hash = SHA256.Create())
                    {
                        reader.Read();
                        if ((string)reader["password_hash"] ==
                            Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(password))))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        // Returns true, if user with exact username exists in database
        public bool UserExists(string username)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT username FROM UserTable WHERE username = '{username}'",
                _connection).ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        // Deletes user with exact username
        public void DeleteUser(string username)
        {
            new SqliteCommand($"DELETE FROM UserTable WHERE username = '{username}'", _connection).ExecuteNonQuery();
        }
    }
}