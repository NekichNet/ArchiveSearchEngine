using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Doc;
using Spire.Doc.Documents;
using System.Drawing;
using System.Windows;
using System.Windows.Documents;

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
                "case_num INTEGER NOT NULL, " +
                "destruct_act_num TEXT, " +
                "destruct_act_date DATE, " +
                "struct_division TEXT NOT NULL, " +
                "gived_post TEXT NOT NULL, " +
                "gived_fullname TEXT NOT NULL, " +
                "achieved_username TEXT NOT NULL, " +
                "is_personnel INTEGER NOT NULL, " +
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
            DateTime documentsDate, int caseNum,
            string destructActNum, DateTime destructActDate,
            string structDivision, string givedPost,
            string givedFullname, bool isPersonnel,
            string achievedUsername, string note)
        {
            if (!DocumentExists(registrationNum))
            {
                new SqliteCommand($"INSERT INTO DocumentTable " +
                $"(registration_num, volume_num, book_num, content_quantity, " +
                $"inventory_date, inventory_num, object_index, object_name," +
                $"rack, shelf, expiring_in, documents_date, case_num," +
                $"destruct_act_num, destruct_act_date, struct_division," +
                $"gived_post, gived_fullname, is_personnel, achieved_username, note) VALUES " +
                $"('{registrationNum.Replace("'", "")}', '{volumeNum.Replace("'", "")}', '{bookNum.Replace("'", "")}', {contentQuantity}, " +
                $"'{inventoryDate}', '{inventoryNum.Replace("'", "")}', '{objectIndex.Replace("'", "")}', '{objectName.Replace("'", "")}', " +
                $"'{rack.Replace("'", "")}', '{shelf.Replace("'", "")}', '{expiringIn.Replace("'", "")}', '{documentsDate.}', {caseNum}, " +
                $"'{destructActNum.Replace("'", "")}', '{destructActDate}', '{structDivision.Replace("'", "")}', " +
                $"'{givedPost.Replace("'", "")}', '{givedFullname.Replace("'", "")}', {isPersonnel}, '{achievedUsername}', '{note.Replace("'", "")}')",
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
                        Convert.ToInt32(reader["case_num"]), (string)reader["destruct_act_num"],
                        Convert.ToDateTime(reader["destruct_act_date"]), (string)reader["struct_division"],
                        (string)reader["gived_post"], (string)reader["gived_fullname"], Convert.ToInt32(reader["is_personnel"]) == 1,
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
                        Convert.ToInt32(reader["case_num"]), (string)reader["destruct_act_num"],
                        Convert.ToDateTime(reader["destruct_act_date"]), (string)reader["struct_division"],
                        (string)reader["gived_post"], (string)reader["gived_fullname"],
                        Convert.ToInt32(reader["is_personnel"]) == 1,
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

            if (page < 0) { throw new Exception("Непредвиденная ошибка: страница меньше единицы"); }

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
                        Convert.ToInt32(reader["case_num"]), (string)reader["destruct_act_num"],
                        Convert.ToDateTime(reader["destruct_act_date"]), (string)reader["struct_division"],
                        (string)reader["gived_post"], (string)reader["gived_fullname"],
                        Convert.ToInt32(reader["is_personnel"]) == 1,
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

                    $"registration_num='{doc.RegistrationNum.Replace("'", "")}', " +
                    $"volume_num='{doc.VolumeNum.Replace("'", "")}', " +
                    $"book_num='{doc.BookNum.Replace("'", "")}', " +
                    $"content_quantity={doc.ContentQuantity}, " +
                    $"inventory_date='{doc.InventoryDate}', " +
                    $"inventory_num='{doc.InventoryNum.Replace("'", "")}', " +
                    $"object_index='{doc.ObjectIndex.Replace("'", "")}', " +
                    $"object_name='{doc.ObjectName.Replace("'", "")}', " +
                    $"rack='{doc.Rack.Replace("'", "")}', " +
                    $"shelf='{doc.Shelf.Replace("'", "")}', " +
                    $"expiring_in='{doc.ExpiringIn.Replace("'", "")}', " +
                    $"documents_date='{doc.DocumentsDate}', " +
                    $"case_num={doc.CaseNum}, " +
                    $"destruct_act_num='{doc.DestructActNum.Replace("'", "")}', " +
                    $"destruct_act_date='{doc.DestructActDate}', " +
                    $"struct_division='{doc.StructDivision.Replace("'", "")}', " +
                    $"gived_post='{doc.GivedPost.Replace("'", "")}', " +
                    $"gived_fullname='{doc.GivedFullname.Replace("'", "")}', " +
                    $"is_personnel={(doc.IsPersonnel? 1 : 0)}, " +
                    $"note='{doc.Note.Replace("'", "")}', " +

                    $"WHERE registration_num = '{oldRegistrationNum}'", _connection).ExecuteNonQuery();
                return true;
            }
            return false;
        }

        public void TakeDocument(string registrationNum, string username)
        {
            int cmd = new SqliteCommand($"UPDATE DocumentTable SET taken_username='{username}', taken_datetime='{DateTime.Now}' WHERE registration_num = '{registrationNum}'",
                _connection).ExecuteNonQuery();
        }

        public void ReturnDocument(string registrationNum)
        {
            int cmd = new SqliteCommand($"UPDATE DocumentTable SET taken_username=NULL, taken_datetime=NULL WHERE registration_num = '{registrationNum}'",
                _connection).ExecuteNonQuery();
        }

        public string UserWhoTook(string registrationNum)
        {
            using (SqliteDataReader reader = new SqliteCommand(
                $"SELECT taken_username FROM DocumentTable WHERE registration_num = '{registrationNum}' AND NOT taken_username IS NULL LIMIT 1",
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
                $"SELECT taken_username FROM DocumentTable WHERE registration_num='{registrationNum}' AND taken_username IS NULL LIMIT 1",
                _connection).ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        // Deletes document with exact id
        public void DeleteDocument(string registrationNum)
        {
            new SqliteCommand($"DELETE FROM UserDocument WHERE registration_num = '{registrationNum}'", _connection).ExecuteNonQuery();
        }

        delegate bool CheckExpiring(string expiring_in);

        // Проверка значения "срока хранения" на то, что документ "временного хранения"
        public bool CheckIsTempExpiring(string expiring_in)
        {
            if (int.TryParse(expiring_in, out _))
            {
                return Int32.Parse(expiring_in) <= 5;
            }
            return false;
        }

        // Проверка значения "срока хранения" на то, что документ "долговременного хранения"
        private bool CheckIsLongExpiring(string expiring_in)
        {
            if (int.TryParse(expiring_in, out _))
            {
                return Int32.Parse(expiring_in) > 10;
            }
            return false;
        }

        private bool CheckIsNoExpiring(string expiring_in)
        {
            return expiring_in.ToLower().Contains("постоянно");
        }

        // ToDo: Генерация описей четырёх видов
        public void ExportToWord(string filepath, string inventory_num, string doc_type,
            string by_year, int startCaseNum, int endCaseNum)
        {
            CheckExpiring check_method;
            if (doc_type == "Дела временного хранения")
            {
                check_method = CheckIsTempExpiring;
            }
            else if (doc_type == "Дела долговременного хранения")
            {
                check_method = CheckIsLongExpiring;
            }
            else if (doc_type == "Дела постоянного хранения")
            {
                check_method = CheckIsNoExpiring;
            }
            else if (doc_type == "Дела по личному составу")
            {
                check_method = (string expiring_in) => { return true; };
            }
            else { throw new Exception("Некоректный тип документа"); }

            Spire.Doc.Document document = new Spire.Doc.Document();

            ParagraphStyle textStyle = new ParagraphStyle(document);
            textStyle.Name = "MainTextStyle";
            textStyle.CharacterFormat.FontName = "Franklin Gothic Book";
            textStyle.CharacterFormat.FontSize = 12f;
            document.Styles.Add(textStyle);

            Spire.Doc.Section section = document.AddSection();
            section.PageSetup.Margins.Left = 90f;

            Spire.Doc.Table heading_table = section.AddTable(false);
            heading_table.ResetCells(1, 2);

            heading_table[0, 0].SetCellWidth(390f, CellWidthType.Point);
            heading_table[0, 1].SetCellWidth(210f, CellWidthType.Point);

            Spire.Doc.Documents.Paragraph heading1 = heading_table[0, 0].AddParagraph();
            heading1.ApplyStyle("MainTextStyle");

            heading1.AppendText("Ноябрьское управление \r\n" +
                "магистральных нефтепроводов\r\n" +
                "Акционерное общество \r\n" +
                "«Транснефть – Сибирь» \r\n" +
                "(АО «Транснефть – Сибирь»)\r\n" +
                "публичного акционерного общества \r\n" +
                "«Транснефть» (ПАО «Транснефть»)\r\n\r\n" +
                "Фонд № ___________\r\n" +
                $"ОПИСЬ № {inventory_num} \r\n" +
                $"{doc_type}\r\n" +
                $"за {by_year} год\r\n");

            Spire.Doc.Documents.Paragraph heading2 = heading_table[0, 1].AddParagraph();
            heading2.ApplyStyle("MainTextStyle");

            heading2.AppendText("УТВЕРЖДАЮ\r\n" +
                "Начальник управления\r\n" +
                "Ноябрьского УМН\r\n" +
                "АО «Транснефть-Сибирь» \r\n" +
                $"________________________\r\n" +
                $"«____»____________ {DateTime.Now.Year} г.\r\n");

            Spire.Doc.Table datatable = section.AddTable(true);
            datatable.DefaultColumnsNumber = 6;

            datatable[0, 0].AddParagraph().AppendText("№ п\\п");
            datatable[1, 0].AddParagraph().AppendText("1");

            datatable[0, 1].AddParagraph().AppendText("Индекс дела");
            datatable[1, 1].AddParagraph().AppendText("2");

            datatable[0, 2].AddParagraph().AppendText("Заголовок дела");
            datatable[1, 2].AddParagraph().AppendText("3");

            datatable[0, 3].AddParagraph().AppendText("Крайние даты");
            datatable[1, 3].AddParagraph().AppendText("4");

            datatable[0, 4].AddParagraph().AppendText("Кол-во листов");
            datatable[1, 4].AddParagraph().AppendText("5");

            datatable[0, 5].AddParagraph().AppendText("Примечание");
            datatable[1, 5].AddParagraph().AppendText("6");

            int docCounter = 0;
            int rowCounter = 2;
            string currentDivision = "";
            int numbers_lost = 0;
            int lastNum = -1;

            // ToDo: Здесь заполнение таблицы описи
            using (SqliteDataReader reader = new SqliteCommand(
                "SELECT case_num, object_index, object_name, documents_date," +
                " content_quantity, struct_division, note, expiring_in" +
                " FROM DocumentTable ORDER BY struct_division, case_num" +
                $" WHERE is_personnel = {(doc_type == "Дела по личному составу" ? 1 : 0)}",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Проверка на соответствии типу описи по сроку хранения
                        if (!check_method((string)reader["expiring_in"]))
                        {
                            continue;
                        }

                        // Делаем горизонтальное разграничение по "структурному подразделению" текущей группы документов
                        if ((string)reader["struct_division"] != currentDivision)
                        {
                            datatable.ApplyHorizontalMerge(rowCounter, 0, 5);
                            datatable[rowCounter, 0].AddParagraph().AppendText((string)reader["struct_division"]);
                            rowCounter++;
                        }

                        if (lastNum == -1) { lastNum = Convert.ToInt32(reader["case_num"]); }
                        numbers_lost += Convert.ToInt32(reader["case_num"]) - lastNum == 1 ? 0 : Convert.ToInt32(reader["case_num"]) - lastNum;

                        datatable[rowCounter, 0].AddParagraph().AppendText((string)reader["case_num"]);
                        datatable[rowCounter, 0].AddParagraph().AppendText((string)reader["object_index"]);
                        datatable[rowCounter, 0].AddParagraph().AppendText((string)reader["object_name"]);
                        datatable[rowCounter, 0].AddParagraph().AppendText((string)reader["documents_date"]);
                        datatable[rowCounter, 0].AddParagraph().AppendText((string)reader["content_quantity"]);
                        datatable[rowCounter, 0].AddParagraph().AppendText((string)reader["note"]);

                        docCounter++;
                        rowCounter++;
                    }
                }
            }

            Spire.Doc.Documents.Paragraph ending = section.AddParagraph();
            ending.ApplyStyle("MainTextStyle");

            ending.AppendText($"В данный раздел описи внесено {docCounter} (двести сорок семь) дел,\r\n" +
                $"с № {startCaseNum} по № {endCaseNum} в том числе: \r\n" +
                "литерные номера: нет\r\n" +
                $"пропущенные номера: {(numbers_lost == 0 ? "нет" : numbers_lost)} \r\n\r\n\r\n" +
                "Начальник АХО ____________________\r\n" +
                $"{DateTime.Now.ToShortDateString()}\r\n\r\n\r\n" +
                "СОГЛАСОВАНО\r\n" +
                "Протокол ЭК Ноябрьского УМН \r\n" +
                "от __________ № ____\r\n");

            document.SaveToFile(filepath, FileFormat.Docx);
            document.Dispose();
        }
    }
}
