using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine.Model.Database
{
    public class DocumentFilter
    {
        public DocumentFilter(string objectIndex = "",
            string objectName = "", string volumeNum = "",
            string bookNum = "", string contentQuantity = "",
            string expiringIn = "", string documentsDate = "")
        {
            ObjectIndex = objectIndex;
            ObjectName = objectName;
            VolumeNum = volumeNum;
            BookNum = bookNum;
            ContentQuantity = contentQuantity;
            ExpiringIn = expiringIn;
            DocumentsDate = documentsDate;
        }

        public string ObjectIndex { get; set; } // 7 Индекс объекта
        public string ObjectName { get; set; } // 8 Наименование объекта
        public string VolumeNum { get; set; } // 2 Номер тома
        public string BookNum { get; set; } // 3 Номер книги
        public string ContentQuantity { get; set; } // 4 Количество страниц / дисков
        public string ExpiringIn { get; set; } // 11 Срок хранения дела
        public string DocumentsDate { get; set; } // 12 Дата документов

        public bool FilterEnabled
        {
            get
            {
                return ObjectIndex.Length + ObjectName.Length + VolumeNum.Length + BookNum.Length + ContentQuantity.Length + ExpiringIn.Length + DocumentsDate.Length > 0;
            }
        }

    }
}
