using Microsoft.Data.Sqlite;
using Spire.Doc;
using Spire.Doc.Collections;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
                "inventory_date TEXT, " +
                "inventory_num TEXT NOT NULL, " +
                "object_index TEXT NOT NULL, " +
                "object_name TEXT NOT NULL, " +
                "storage TEXT NOT NULL, " +
                "rack TEXT NOT NULL, " +
                "shelf TEXT NOT NULL, " +
                "expiring_in TEXT NOT NULL, " +
                "documents_date TEXT NOT NULL, " +
                "case_num INTEGER NOT NULL, " +
                "destruct_act_num TEXT NOT NULL, " +
                "destruct_act_date DATE, " +
                "struct_division TEXT, " +
                "gived_post TEXT, " +
                "gived_fullname TEXT, " +
                "achieved_username TEXT, " +
                "is_personnel INTEGER NOT NULL, " +
                "taken_username TEXT, " +
                "taken_datetime TEXT, " +
                "note TEXT)", _connection).ExecuteNonQuery();
        }

        // Creates new document. If document with this registrationNum already exists, returns false
        public bool NewDocument(string registrationNum,
            string volumeNum, string bookNum,
            int contentQuantity, string inventoryNum,
            string objectIndex, string objectName,
            string storage, string rack, string shelf,
            string expiringIn, DateTime documentsDate,
            int caseNum, string destructActNum,
            string structDivision, string givedPost,
            string givedFullname, bool isPersonnel,
            string achievedUsername, string note,
            DateTime? inventoryDate = null,
            DateTime? destructActDate = null)
        {
            if (!DocumentExists(registrationNum))
            {
                new SqliteCommand($"INSERT INTO DocumentTable " +
                $"(registration_num, volume_num, book_num, content_quantity, " +
                $"inventory_date, inventory_num, object_index, object_name," +
                $"storage, rack, shelf, expiring_in, documents_date, case_num," +
                $"destruct_act_num, destruct_act_date, struct_division," +
                $"gived_post, gived_fullname, is_personnel, achieved_username, note) VALUES " +
                $"('{registrationNum.Replace("'", "")}', '{volumeNum.Replace("'", "")}', '{bookNum.Replace("'", "")}', {contentQuantity}, " +
                $"'{inventoryDate}', '{inventoryNum.Replace("'", "")}', '{objectIndex.Replace("'", "")}', '{objectName.Replace("'", "")}', " +
                $"'{storage.Replace("'", "")}', '{rack.Replace("'", "")}', '{shelf.Replace("'", "")}', '{expiringIn.Replace("'", "")}', " +
                $"'{documentsDate}', {caseNum}, '{destructActNum.Replace("'", "")}', '{destructActDate}', '{structDivision.Replace("'", "")}', " +
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
                        Convert.ToInt32(reader["content_quantity"]),
                        (string)reader["inventory_num"], (string)reader["object_index"],
                        (string)reader["object_name"], (string)reader["storage"],
                        (string)reader["rack"], (string)reader["shelf"],
                        (string)reader["expiring_in"], Convert.ToDateTime(reader["documents_date"]),
                        Convert.ToInt32(reader["case_num"]), (string)reader["destruct_act_num"],
                        (string)reader["struct_division"], (string)reader["gived_post"],
                        (string)reader["gived_fullname"], Convert.ToInt32(reader["is_personnel"]) == 1,
                        (string)reader["achieved_username"], (string)reader["note"],
                        reader["inventory_date"] is null ? null : Convert.ToDateTime(reader["inventory_date"]),
                        reader["destruct_act_date"] is null ? null : Convert.ToDateTime(reader["destruct_act_date"]));
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
                        Convert.ToInt32(reader["content_quantity"]),
                        (string)reader["inventory_num"], (string)reader["object_index"],
                        (string)reader["object_name"], (string)reader["storage"],
                        (string)reader["rack"], (string)reader["shelf"],
                        (string)reader["expiring_in"], Convert.ToDateTime(reader["documents_date"]),
                        Convert.ToInt32(reader["case_num"]), (string)reader["destruct_act_num"],
                        (string)reader["struct_division"], (string)reader["gived_post"],
                        (string)reader["gived_fullname"], Convert.ToInt32(reader["is_personnel"]) == 1,
                        (string)reader["achieved_username"], (string)reader["note"],
                        reader["inventory_date"] is null || (string)reader["inventory_date"] == "" ?
                        null : Convert.ToDateTime(reader["inventory_date"]),
                        reader["destruct_act_date"] is null || (string)reader["destruct_act_date"] == "" ?
                        null : Convert.ToDateTime(reader["destruct_act_date"])));
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

            string query = $"SELECT * FROM DocumentTable";

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

            query += $" LIMIT {page * 30}, 30";

            using (SqliteDataReader reader = new SqliteCommand(query, _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        documents.Add(new Document((string)reader["registration_num"],
                        (string)reader["volume_num"], (string)reader["book_num"],
                        Convert.ToInt32(reader["content_quantity"]),
                        (string)reader["inventory_num"], (string)reader["object_index"],
                        (string)reader["object_name"], (string)reader["storage"],
                        (string)reader["rack"], (string)reader["shelf"],
                        (string)reader["expiring_in"], Convert.ToDateTime(reader["documents_date"]),
                        Convert.ToInt32(reader["case_num"]), (string)reader["destruct_act_num"],
                        (string)reader["struct_division"], (string)reader["gived_post"],
                        (string)reader["gived_fullname"], Convert.ToInt32(reader["is_personnel"]) == 1,
                        (string)reader["achieved_username"], (string)reader["note"],
                        reader["inventory_date"] is null || (string)reader["inventory_date"] == "" ?
                        null : Convert.ToDateTime(reader["inventory_date"]),
                        reader["destruct_act_date"] is null || (string)reader["destruct_act_date"] == "" ?
                        null : Convert.ToDateTime(reader["destruct_act_date"])));
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
            if (doc.RegistrationNum == oldRegistrationNum || !DocumentExists(doc.RegistrationNum))
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
                    $"storage='{doc.Storage.Replace("'", "")}', " +
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
                    $"note='{doc.Note.Replace("'", "")}' " +

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
            // ToDo: Вот это заменить на проверку в самом SQL запросе
            CheckExpiring check_method;
            if (doc_type == "Дела временного хранения")
            {
                doc_type = "дел временного хранения";
                check_method = CheckIsTempExpiring;
            }
            else if (doc_type == "Дела долговременного хранения")
            {
                doc_type = "дел долговременного хранения";
                check_method = CheckIsLongExpiring;
            }
            else if (doc_type == "Дела постоянного хранения")
            {
                doc_type = "дел, документов постоянного хранения";
                check_method = CheckIsNoExpiring;
            }
            else if (doc_type == "Дела по личному составу")
            {
                doc_type = "дел по личному составу";
                check_method = (string expiring_in) => { return true; };
            }
            else { throw new Exception("Некоректный тип документа"); }

            Spire.Doc.Document document = new Spire.Doc.Document();

            ParagraphStyle textStyle = new ParagraphStyle(document);
            textStyle.Name = "MainTextStyle";
            textStyle.CharacterFormat.FontName = "Franklin Gothic Book";
            textStyle.CharacterFormat.FontSize = 12f;
            document.Styles.Add(textStyle);

            ParagraphStyle tableStyle = new ParagraphStyle(document);
            tableStyle.Name = "TableTextStyle";
            tableStyle.CharacterFormat.FontName = "Franklin Gothic Book";
            tableStyle.CharacterFormat.FontSize = 12f;
            tableStyle.ParagraphFormat.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            document.Styles.Add(tableStyle);

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

            Spire.Doc.TableRow headingRow = datatable.AddRow();

            Spire.Doc.TableCell caseNumCellH = headingRow.AddCell();
            caseNumCellH.AddParagraph().AppendText("№ п\\п");
            caseNumCellH.FirstParagraph.ApplyStyle("TableTextStyle");

            Spire.Doc.TableCell objectIndexCellH = headingRow.AddCell();
            objectIndexCellH.AddParagraph().AppendText("Индекс дела");
            objectIndexCellH.FirstParagraph.ApplyStyle("TableTextStyle");

            Spire.Doc.TableCell objectNameCellH = headingRow.AddCell();
            objectNameCellH.AddParagraph().AppendText("Заголовок дела");
            objectNameCellH.FirstParagraph.ApplyStyle("TableTextStyle");

            Spire.Doc.TableCell documentsDateCellH = headingRow.AddCell();
            documentsDateCellH.AddParagraph().AppendText("Крайние даты");
            documentsDateCellH.FirstParagraph.ApplyStyle("TableTextStyle");

            Spire.Doc.TableCell contentQuantityCellH = headingRow.AddCell();
            contentQuantityCellH.AddParagraph().AppendText("Кол-во листов");
            contentQuantityCellH.FirstParagraph.ApplyStyle("TableTextStyle");

            Spire.Doc.TableCell noteCellH = headingRow.AddCell();
            noteCellH.AddParagraph().AppendText("Примечание");

            Spire.Doc.TableRow heading2Row = datatable.AddRow();

            heading2Row.Cells[0].AddParagraph().AppendText("1");
            heading2Row.Cells[1].AddParagraph().AppendText("2");
            heading2Row.Cells[2].AddParagraph().AppendText("3");
            heading2Row.Cells[3].AddParagraph().AppendText("4");
            heading2Row.Cells[4].AddParagraph().AppendText("5");
            heading2Row.Cells[5].AddParagraph().AppendText("6");

            heading2Row.Cells[0].FirstParagraph.ApplyStyle("TableTextStyle");
            heading2Row.Cells[1].FirstParagraph.ApplyStyle("TableTextStyle");
            heading2Row.Cells[2].FirstParagraph.ApplyStyle("TableTextStyle");
            heading2Row.Cells[3].FirstParagraph.ApplyStyle("TableTextStyle");
            heading2Row.Cells[4].FirstParagraph.ApplyStyle("TableTextStyle");
            heading2Row.Cells[5].FirstParagraph.ApplyStyle("TableTextStyle");

            int docCounter = 0;
            string currentDivision = "";
            int numbers_lost = -1; // it's gaining one extra number lost somehow, so starting with -1
            int lastNum = -1;
            List<int> mergeIndexes = new List<int>();

            // ToDo: Здесь заполнение таблицы описи
            using (SqliteDataReader reader = new SqliteCommand(
                "SELECT registration_num, case_num, object_index, object_name, documents_date," +
                " content_quantity, struct_division, note, expiring_in" +
                $" FROM DocumentTable WHERE is_personnel = {(doc_type == "дел по личному составу" ? 1 : 0)} AND" +
                $" case_num BETWEEN {startCaseNum} AND {endCaseNum}" +
                " ORDER BY struct_division, case_num",
                _connection).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Проверка документа по фильтрам (todo: потом впихнуть в sql)
                        if (!check_method((string)reader["expiring_in"]) || Convert.ToDateTime((string)reader["documents_date"]).Year > Convert.ToInt32(by_year))
                        {
                            continue;
                        }

                        // Делаем горизонтальное разграничение по "структурному подразделению" текущей группы документов
                        if ((string)reader["struct_division"] != currentDivision)
                        {
                            currentDivision = (string)reader["struct_division"];
                            Spire.Doc.TableRow divisionRow = datatable.AddRow();
                            mergeIndexes.Add(divisionRow.GetRowIndex());
                            divisionRow.Cells[0].AddParagraph().AppendText((string)reader["struct_division"]);
                            divisionRow.Cells[0].FirstParagraph.ApplyStyle("TableTextStyle");
                        }

                        // Проверка на пропущенный номер
                        if (lastNum == -1) { lastNum = Convert.ToInt32(reader["case_num"]); }
                        numbers_lost += Convert.ToInt32(reader["case_num"]) - lastNum == 1 ? 0 : Convert.ToInt32(reader["case_num"]) - lastNum;
                        lastNum = Convert.ToInt32(reader["case_num"]);

                        // Заполнение данными
                        Spire.Doc.TableRow newRow = datatable.AddRow();

                        newRow.Cells[0].AddParagraph().AppendText(Convert.ToString(reader["case_num"]) + ".");
                        newRow.Cells[1].AddParagraph().AppendText((string)reader["object_index"]);
                        newRow.Cells[2].AddParagraph().AppendText((string)reader["object_name"]);
                        newRow.Cells[3].AddParagraph().AppendText((string)reader["documents_date"]);
                        newRow.Cells[4].AddParagraph().AppendText(Convert.ToString(reader["content_quantity"]));
                        newRow.Cells[5].AddParagraph().AppendText((string)reader["note"]);

                        newRow.Cells[0].FirstParagraph.ApplyStyle("TableTextStyle");
                        newRow.Cells[1].FirstParagraph.ApplyStyle("TableTextStyle");
                        newRow.Cells[2].FirstParagraph.ApplyStyle("TableTextStyle");
                        newRow.Cells[3].FirstParagraph.ApplyStyle("TableTextStyle");
                        newRow.Cells[4].FirstParagraph.ApplyStyle("TableTextStyle");
                        newRow.Cells[5].FirstParagraph.ApplyStyle("TableTextStyle");

                        docCounter++;

                        new SqliteCommand($"UPDATE DocumentTable SET " +
                            $"inventory_num = '{inventory_num}', " +
                            $"inventory_date = '{DateTime.Now}'" +
                            $" WHERE registration_num = {(string)reader["registration_num"]}", _connection).ExecuteNonQuery();
                    }
                }
            }

            foreach (int ind in mergeIndexes)
            {
                datatable.ApplyHorizontalMerge(ind, 0, 5);
            }

            datatable.SetColumnWidth(0, 0.118f * 600f, CellWidthType.Point);
            datatable.SetColumnWidth(1, 0.164f * 600f, CellWidthType.Point);
            datatable.SetColumnWidth(2, 0.388f * 600f, CellWidthType.Point);
            datatable.SetColumnWidth(3, 0.119f * 600f, CellWidthType.Point);
            datatable.SetColumnWidth(4, 0.104f * 600f, CellWidthType.Point);
            datatable.SetColumnWidth(5, 0.105f * 600f, CellWidthType.Point);

            Spire.Doc.Documents.Paragraph ending = section.AddParagraph();
            ending.ApplyStyle("MainTextStyle");

            ending.AppendText($"\r\nВ данный раздел описи внесено {docCounter} ({NumToStringConverter(Convert.ToString(docCounter))}) дел,\r\n" +
                $"с № {startCaseNum} по № {endCaseNum} в том числе: \r\n" +
                "литерные номера: нет\r\n" +
                $"пропущенные номера: {(numbers_lost < 1 ? "нет" : numbers_lost)} \r\n\r\n\r\n" +
                "Начальник АХО ____________________\r\n" +
                $"{DateTime.Now.ToShortDateString()}\r\n\r\n\r\n" +
                "СОГЛАСОВАНО\r\n" +
                "Протокол ЭК Ноябрьского УМН \r\n" +
                "от __________ № ____\r\n");

            document.SaveToFile(filepath, FileFormat.Docx);
            document.Dispose();
        }

        // https://github.com/RiftSquadronFounder/IntToStringConverterRus
        private string NumToStringConverter(string value)
        {
            string word = "Ошибка чтения числа";

            List<string> values1 = new List<string> { "", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять" };
            List<string> values2 = new List<string> { "", "", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяноста" };
            List<string> values3 = new List<string> { "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
            List<string> values4 = new List<string> { "тысяч", "одна тысяча", "две тысячи", "три тысячи", "четыре тысячи", "пять тысяч", "шесть тысяч", "семь тысяч", "восемь тысяч", "девять тысяч" };
            List<string> values3AfterTen = new List<string> { "десять", "одинадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
            List<string> Thousands = new List<string> { "тысяч", " тысяча", " тысячи", " тысячи", " тысячи", " тысяч", " тысяч", " тысяч", " тысяч", " тысяч" };
            try
            {
                if (value.Length < 7)
                {
                    word = "";
                    if (value == "0") { return "ноль"; }

                    if (value[0] == '1' && value.Length == 2)
                    {
                        word += values3AfterTen[Int32.Parse(value[value.Length - 1].ToString())];
                    }
                    else
                    {
                        if (value.Length == 6)
                        {
                            word += values3[Int32.Parse(value[0].ToString())] + " ";
                        }
                        if (value.Length >= 5)
                        {
                            if (value[value.Length - 5] == '1')
                            {
                                word += values3AfterTen[Int32.Parse(value[value.Length - 4].ToString())] + " тысяч ";
                            }
                            else
                            {
                                word += values2[Int32.Parse(value[value.Length - 5].ToString())] + " ";
                                word += values4[Int32.Parse(value[value.Length - 4].ToString())] + " ";
                            }
                        }
                        if (value.Length == 4)
                        {
                            word += values4[Int32.Parse(value[value.Length - 4].ToString())] + " ";
                        }
                        if (value.Length >= 3)
                        {
                            word += values3[Int32.Parse(value[value.Length - 3].ToString())] + " ";
                        }
                        if (value.Length >= 2)
                        {
                            word += values2[Int32.Parse(value[value.Length - 2].ToString())] + " ";
                        }
                        if (value.Length >= 1)
                        {
                            word += values1[Int32.Parse(value[value.Length - 1].ToString())];
                        }
                    }
                }
            }
            catch
            {
                word = "Ошибка чтения числа";
            }
            return word;
        }
    }
}
