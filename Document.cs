using ArchiveSearchEngine.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine
{
    public class Document
    {
        public Document(string registrationNum,
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
            RegistrationNum = registrationNum;
            VolumeNum = volumeNum;
            BookNum = bookNum;
            ContentQuantity = contentQuantity;
            InventoryDate = inventoryDate;
            InventoryNum = inventoryNum;
            ObjectIndex = objectIndex;
            ObjectName = objectName;
            Rack = rack;
            Shelf = shelf;
            ExpiringIn = expiringIn;
            DocumentsDate = documentsDate;
            CaseNum = caseNum;
            DestructActNum = destructActNum;
            DestructActDate = destructActDate;
            StructDivision = structDivision;
            GivedPost = givedPost;
            GivedFullname = givedFullname;
            IsPersonnel = isPersonnel;
            AchievedUsername = achievedUsername;
            Note = note;
        }
        static public DocumentTable Table { get; set; }

        public string RegistrationNum { get; set; } // 1 Номер регистрации объекта
        public string VolumeNum { get; set; } // 2 Номер тома
        public string BookNum { get; set; } // 3 Номер книги
        public int ContentQuantity { get; set; } // 4 Количество листов / дисков

        public DateTime InventoryDate { get; set; } // 5 Дата описи
        public string InventoryNum { get; set; } // 6 Номер описи

        public string ObjectIndex { get; set; } // 7 Код/индекс дела
        public string ObjectName { get; set; } // 8 Наименование объекта

        public string Rack { get; set; } // 9 Стеллаж
        public string Shelf { get; set; } // 10 Полка

        public string ExpiringIn { get; set; } // 11 Срок хранения дела, по перечню
        public DateTime DocumentsDate { get; set; } // 12 Дата документов
        public int CaseNum { get; set; } // 13 Дело № (валовый номер)

        public string DestructActNum { get; set; } // 14 Номер акта на уничтожение
        public DateTime DestructActDate { get; set; } // 15 Дата акта на уничтожение

        public string StructDivision { get; set; } // 16 Структурное подразделение

        public string GivedPost { get; set; } // 17 Должность сдавшего в архив
        public string GivedFullname { get; set; } // 18 ФИО сдавшего в архив

        public string Note { get; set; } // 27 Примечание

        public bool IsPersonnel { get; set; } // Является ли делом по личному составу

        public bool Available { get { return Table.IsAvailable(RegistrationNum); } } // Доступен ли к выдаче

        public string AchievedUsername { get; } // username того, кто принял документ со стороны архива
    }
}
