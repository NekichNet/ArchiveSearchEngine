using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine
{
    public class Document
    {
        public Document(int id,
            string registrationNum,
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
            Id = id;
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
            AchievedUsername = achievedUsername;
            Note = note;
        }

        public int Id { get; } // Id документа в базе данных

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
        public string CaseNum { get; set; } // 13 Дело № (валовый номер)

        public string DestructActNum { get; set; } // 14 Номер акта на уничтожение
        public DateTime DestructActDate { get; set; } // 15 Дата акта на уничтожение

        public string StructDivision { get; set; } // 16 Структурное подразделение

        public string GivedPost { get; set; } // 17 Должность сдавшего в архив
        public string GivedFullname { get; set; } // 18 ФИО сдавшего в архив

        public string Note { get; set; } // 27 Примечание

        public string AchievedUsername { get; } // username того, кто принял документ со стороны архива
    }
}
