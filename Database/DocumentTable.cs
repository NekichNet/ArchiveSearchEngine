using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Doc;
using System.Windows;

namespace ArchiveSearchEngine.Database
{
    public class DocumentTable
    {
        SqliteConnection _connection;

        public DocumentTable(SqliteConnection connection)
        {
            _connection = connection;

            new SqliteCommand("CREATE TABLE IF NOT EXISTS DocumentTable (" +
                "registration_num TEXT PRIMARY KEY NOT NULL, " +
                "volume_num TEXT, " +
                "book_num TEXT, " +
                "content_quantity INTEGER NOT NULL, " +
                "inventory_date DATE, " +
                "inventory_num TEXT, " +
                "object_index TEXT NOT NULL, " +
                "object_name TEXT NOT NULL, " +
                "rack TEXT NOT NULL, " +
                "shelf TEXT NOT NULL, " +
                "expiring_in TEXT NOT NULL, " +
                "documents_date DATE NOT NULL, " +
                "case_num TEXT NOT NULL, " +
                "destruct_act_num TEXT, " +
                "destruct_act_date DATE, " +
                "struct_division TEXT NOT NULL, " +
                "gived_post TEXT NOT NULL, " +
                "gived_fullname TEXT NOT NULL, " +
                "achieved_username TEXT NOT NULL, " +
                "taken_username TEXT, " +
                "taken_datetime TEXT, " +
                "note TEXT)", _connection).ExecuteNonQuery();
        }

        // Creates new document. If document with this registrationNum already exists, returns false
        public bool NewDocument(string registrationNum,
            string volumeNum, string bookNum,
            int contentQuantity,
            DateTime inventoryDate, string inventoryNum,
            string objectIndex, string objectName,
            string rack, string shelf, string expiringIn,
            DateTime documentsDate, string caseNum,
            string destructActNum, DateTime destructActDate,
            string structDivision,
            string givedPost, string givedFullname,
            string achievedUsername, string note)
        {
            if (!DocumentExists(registrationNum))
            {
                new SqliteCommand($"INSERT INTO DocumentTable " +
                $"(registration_num, volume_num, book_num, content_quantity, " +
                $"inventory_date, inventory_num, object_index, object_name," +
                $"rack, shelf, expiring_in, documents_date, case_num," +
                $"destruct_act_num, destruct_act_date, struct_division," +
                $"gived_post, gived_fullname, achieved_username, note) VALUES " +
                $"('{registrationNum.Replace("'", "")}', '{volumeNum.Replace("'", "")}', '{bookNum.Replace("'", "")}', {contentQuantity}, " +
                $"'{inventoryDate}', '{inventoryNum.Replace("'", "")}', '{objectIndex.Replace("'", "")}', '{objectName.Replace("'", "")}', " +
                $"'{rack.Replace("'", "")}', '{shelf.Replace("'", "")}', '{expiringIn.Replace("'", "")}', '{documentsDate}', '{caseNum.Replace("'", "")}', " +
                $"'{destructActNum.Replace("'", "")}', '{destructActDate}', '{structDivision.Replace("'", "")}', " +
                $"'{givedPost.Replace("'", "")}', '{givedFullname.Replace("'", "")}', '{achievedUsername}', '{note.Replace("'", "")}')",
                _connection).ExecuteNonQuery();
            }
            return !DocumentExists(registrationNum);
        }

        // Returns document found by his id (throws an exception, if not exists)
        public Document GetDocument(string registrationNum)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM DocumentTable WHERE registration_num = {registrationNum}",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new Document((string)reader["registration_num"],
                        (string)reader["volume_num"], (string)reader["book_num"],
                        Convert.ToInt32(reader["content_quantity"]), Convert.ToDateTime(reader["inventory_date"]),
                        (string)reader["inventory_num"], (string)reader["object_index"],
                        (string)reader["object_name"], (string)reader["rack"], (string)reader["shelf"],
                        (string)reader["expiring_in"], Convert.ToDateTime(reader["documents_date"]),
                        (string)reader["case_num"], (string)reader["destruct_act_num"],
                        Convert.ToDateTime(reader["destruct_act_date"]), (string)reader["struct_division"],
                        (string)reader["gived_post"], (string)reader["gived_fullname"],
                        (string)reader["achieved_username"], (string)reader["note"]);
                }
                else
                {
                    throw new Exception($"Документ с регистрационным номером: {registrationNum} не найден");
                }
            }
        }

        // Returns list of all documents in database
        public List<Document> GetDocuments()
        {
            List<Document> documents = new List<Document>();

            using (SqliteDataReader reader = new SqliteCommand(
                "SELECT * FROM DocumentTable",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        documents.Add(new Document((string)reader["registration_num"],
                        (string)reader["volume_num"], (string)reader["book_num"],
                        Convert.ToInt32(reader["content_quantity"]), Convert.ToDateTime(reader["inventory_date"]),
                        (string)reader["inventory_num"], (string)reader["object_index"],
                        (string)reader["object_name"], (string)reader["rack"], (string)reader["shelf"],
                        (string)reader["expiring_in"], Convert.ToDateTime(reader["documents_date"]),
                        (string)reader["case_num"], (string)reader["destruct_act_num"],
                        Convert.ToDateTime(reader["destruct_act_date"]), (string)reader["struct_division"],
                        (string)reader["gived_post"], (string)reader["gived_fullname"],
                        (string)reader["achieved_username"], (string)reader["note"]));
                    }
                }
            }

            return documents;
        }

        // Returns list of 30 documents in database with offset of 30*page (first page is 0)
        public List<Document> GetDocuments(int page, DocumentFilter filter)
        {
            List<Document> documents = new List<Document>();

            if (page < 1) { throw new Exception("Непредвиденная ошибка: страница меньше единицы"); }

            string query = $"SELECT * FROM DocumentTable LIMIT 30 OFFSET {page * 30}";

            if (filter.FilterEnabled) {
                query += " WHERE";
                if (filter.ObjectIndex != "") { query += $" object_index = '{filter.ObjectIndex}'"; }
                if (filter.ObjectName != "") { query += $" object_name LIKE '%{filter.ObjectName}%'"; }
                if (filter.VolumeNum != "") { query += $" volume_num = '{filter.VolumeNum}'"; }
                if (filter.BookNum != "") { query += $" book_num = '{filter.BookNum}'"; }
                if (filter.ContentQuantity != "") { query += $" content_quantity = {filter.ContentQuantity}"; }
                if (filter.ExpiringIn != "") { query += $" expiring_in = '{filter.ExpiringIn}'"; }
                if (filter.DocumentsDate != "") { query += $" documents_date = '{filter.DocumentsDate}'"; }
            }

            using (SqliteDataReader reader = new SqliteCommand(query, _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        documents.Add(new Document((string)reader["registration_num"],
                        (string)reader["volume_num"], (string)reader["book_num"],
                        Convert.ToInt32(reader["content_quantity"]), Convert.ToDateTime(reader["inventory_date"]),
                        (string)reader["inventory_num"], (string)reader["object_index"],
                        (string)reader["object_name"], (string)reader["rack"], (string)reader["shelf"],
                        (string)reader["expiring_in"], Convert.ToDateTime(reader["documents_date"]),
                        (string)reader["case_num"], (string)reader["destruct_act_num"],
                        Convert.ToDateTime(reader["destruct_act_date"]), (string)reader["struct_division"],
                        (string)reader["gived_post"], (string)reader["gived_fullname"],
                        (string)reader["achieved_username"], (string)reader["note"]));
                    }
                }
            }

            return documents;
        }

        // Checks, if document with exact registration_num already exists
        public bool DocumentExists(string registrationNum)
        {
            registrationNum = registrationNum.Replace("'", "");

            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT registration_num FROM DocumentTable WHERE registration_num = '{registrationNum}'",
                _connection).ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        // Updates document info in database, found by his id.
        // Returns false, if doc.RegistrationNum is already claimed
        public bool UpdateDocument(Document doc, string oldRegistrationNum)
        {
            if (doc.RegistrationNum != oldRegistrationNum && DocumentExists(doc.RegistrationNum))
            {
                new SqliteCommand($"UPDATE DocumentTable SET " +

                    $"registration_num={doc.RegistrationNum.Replace("'", "")}, " +
                    $"volume_num={doc.VolumeNum.Replace("'", "")}, " +
                    $"book_num={doc.BookNum.Replace("'", "")}, " +
                    $"content_quantity={doc.ContentQuantity}, " +
                    $"inventory_date={doc.InventoryDate}, " +
                    $"inventory_num={doc.InventoryNum.Replace("'", "")}, " +
                    $"object_index={doc.ObjectIndex.Replace("'", "")}, " +
                    $"object_name={doc.ObjectName.Replace("'", "")}, " +
                    $"rack={doc.Rack.Replace("'", "")}, " +
                    $"shelf={doc.Shelf.Replace("'", "")}, " +
                    $"expiring_in={doc.ExpiringIn.Replace("'", "")}, " +
                    $"documents_date={doc.DocumentsDate}, " +
                    $"case_num={doc.CaseNum.Replace("'", "")}, " +
                    $"destruct_act_num={doc.DestructActNum.Replace("'", "")}, " +
                    $"destruct_act_date={doc.DestructActDate}, " +
                    $"struct_division={doc.StructDivision.Replace("'", "")}, " +
                    $"gived_post={doc.GivedPost.Replace("'", "")}, " +
                    $"gived_fullname={doc.GivedFullname.Replace("'", "")}, " +
                    $"note={doc.Note.Replace("'", "")}, " +

                    $"WHERE registration_num = {oldRegistrationNum}", _connection).ExecuteNonQuery();
                return true;
            }
            return false;
        }

        public void TakeDocument(string registrationNum, string username)
        {
            int cmd = new SqliteCommand($"UPDATE DocumentTable SET taken_username='{username}', taken_datetime='{DateTime.Now}' WHERE registration_num = {registrationNum}",
                _connection).ExecuteNonQuery();
        }

        public void ReturnDocument(string registrationNum)
        {
            int cmd = new SqliteCommand($"UPDATE DocumentTable SET taken_username=NULL, taken_datetime=NULL WHERE registration_num = {registrationNum}",
                _connection).ExecuteNonQuery();
        }

        public string UserWhoTook(string registrationNum)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT taken_username FROM DocumentTable WHERE registration_num = {registrationNum} AND NOT taken_username IS NULL LIMIT 1",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return (string)reader["taken_username"];
                }
                else
                {
                    throw new Exception($"Не существует документа с регистрационным номером: {registrationNum}, либо документ доступен к выдаче");
                }
            }
        }

        public bool IsAvailable(string registrationNum)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT taken_username FROM DocumentTable WHERE registration_num={registrationNum} AND taken_username IS NULL LIMIT 1",
                _connection).ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        // Deletes document with exact id
        public void DeleteDocument(string registrationNum)
        {
            new SqliteCommand($"DELETE FROM UserDocument WHERE registration_num = {registrationNum}", _connection).ExecuteNonQuery();
        }

        // ToDo: Генерация описей четырёх видов
        //public void ExportToWord()
        //{
        //    Spire.Doc.Document document = new Spire.Doc.Document();
        //    Section section = document.AddSection();
        //}
    }
}
