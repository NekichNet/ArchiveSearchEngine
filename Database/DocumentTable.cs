using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Doc;

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
                "rack TEXT NOT NULL," +
                "shelf TEXT NOT NULL," +
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

        public void NewDocument(string registrationNum,
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

        // Returns document found by his id (throws an exception, if not exists)
        public Document GetDocument(int id)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM DocumentTable WHERE id = {id}",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new Document(Convert.ToInt32(reader["id"]), (string)reader["registration_num"],
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
                    throw new Exception($"Документ с id: {id} не найден");
                }
            }
        }

        // Returns list of all documents in database
        public List<Document> GetDocuments()
        {
            List<Document> documents = new List<Document>();

            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT * FROM DocumentTable",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        documents.Add(new Document(Convert.ToInt32(reader["id"]), (string)reader["registration_num"],
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

        // Updates document info in database, found by his id
        public void UpdateDocument(Document doc)
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

                $"WHERE id = {doc.Id}",
                _connection).ExecuteNonQuery();
        }

        // Deletes document with exact id
        public void DeleteDocument(int id)
        {
            new SqliteCommand($"DELETE FROM UserDocument WHERE id = {id}", _connection).ExecuteNonQuery();
        }

        // ToDo: Генерация описей четырёх видов
        //public void ExportToWord()
        //{
        //    Spire.Doc.Document document = new Spire.Doc.Document();
        //    Section section = document.AddSection();
        //}
    }
}
