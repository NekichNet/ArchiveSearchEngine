using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
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
                "achieved_username TEXT NOT NULL," +
                "note TEXT)", _connection).ExecuteNonQuery();
        }

        //public Document NewDocument(string registrationNum,
        //    string volumeNum, string bookNum,
        //    int contentQuantity,
        //    DateOnly inventoryDate, string inventoryNum,
        //    string objectIndex, string objectName,
        //    int rack, int shelf, string expiringIn,
        //    DateOnly documentsDate, string caseNum,
        //    string destructActNum, DateOnly destructActDate,
        //    string structDivision,
        //    string givedPost, string givedFullname,
        //    string achievedUsername, string note)
        //{
        //    using (SHA256 hash = SHA256.Create())
        //    {
        //        new SqliteCommand($"INSERT INTO DocumentTable " +
        //            $"(username, password_hash, fullname, post, struct_division, is_admin) VALUES " +
        //            $"({user.Username}, {Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(password)))}, " +
        //            $"{user.Fullname}, {user.Post}, " +
        //            $"{user.StructDivision}, {user.IsAdmin})", _connection);
        //    }
        //}

        // Returns user found by his username (throws an exception, if not exists)
        public User GetUser(string username)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM UserTable WHERE username = {username}",
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

        // Returns list of users, what have promt in their username or fullname
        public List<User> GetUsers(string promt)
        {
            List<User> users = new List<User>();

            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM UserTable WHERE username LIKE {promt} OR fullname LIKE {promt}",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(new User((string)reader["username"], (string)reader["fullname"],
                        (string)reader["post"], (string)reader["struct_division"], (bool)reader["is_admin"]));
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
                        (string)reader["post"], (string)reader["struct_division"], (bool)reader["is_admin"]));
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
                $"SELECT password_hash FROM UserTable WHERE username = {username}",
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
                $"SELECT username FROM UserTable WHERE username = {username}",
                _connection).ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        // Deletes user with exact username
        public void DeleteUser(string username)
        {
            new SqliteCommand($"DELETE FROM UserTable WHERE username = {username}", _connection).ExecuteNonQuery();
        }
    }
}
