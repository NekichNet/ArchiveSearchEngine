from datetime import date


class Document:
    names = {"registration_num": "Номер регистрации объекта",
             "volume_num": "Номер тома",
             "book_num": "Номер книги",
             "content_quantity": "Количество листов/дисков",
             "inventory_date": "Дата описи",
             "inventory_num": "Номер описи",
             "object_index": "Код/индекс дела",
             "object_name": "Наименование объекта",
             "rack": "Стеллаж №",
             "shelf": "Полка №",
             "expiring_in": "Срок хранения дела",
             "documents_date": "Дата документов",
             "case_num": "Дело № (валовый номер)",
             "destruct_act_num": "Номер акта на уничтожение",
             "destruct_act_date": "Дата акта на уничтожение",
             "struct_division": "Структурное подразделение / отдел",
             "gived_post": "Должность лица, сдавшего в архив",
             "gived_fullname": "ФИО лица, сдавшего в архив",
             "achieved_post": "Должность принявшего со стороны архива",
             "achieved_fullname": "ФИО принявшего со стороны архива",
             "note": "Примечание"}

    def __init__(self,
                 row_id: int,
                 registration_num: str,
                 volume_num: str,
                 book_num: str,
                 content_quantity: int,
                 inventory_date: date,
                 inventory_num: str,
                 object_index: str,
                 object_name: str,
                 rack: int,
                 shelf: int,
                 expiring_in: str,
                 documents_date: date,
                 case_num: str,
                 destruct_act_num: str,
                 destruct_act_date: date,
                 struct_division: str,
                 gived_post: str,
                 gived_fullname: str,
                 achieved_post: str,
                 achieved_fullname: str,
                 note: str):
        self.values = dict()

        self.values["note"] = note
        self.values["achieved_fullname"] = achieved_fullname
        self.values["achieved_post"] = achieved_post
        self.values["gived_fullname"] = gived_fullname
        self.values["gived_post"] = gived_post
        self.values["struct_division"] = struct_division
        self.values["destruct_act_date"] = destruct_act_date
        self.values["destruct_act_num"] = destruct_act_num
        self.values["case_num"] = case_num
        self.values["documents_date"] = documents_date
        self.values["expiring_in"] = expiring_in
        self.values["shelf"] = shelf
        self.values["rack"] = rack
        self.values["object_name"] = object_name
        self.values["object_index"] = object_index
        self.values["inventory_date"] = inventory_date
        self.values["inventory_num"] = inventory_num
        self.values["content_quantity"] = content_quantity
        self.values["book_num"] = book_num
        self.values["volume_num"] = volume_num
        self.values["registration_num"] = registration_num
        self.values["row_id"] = row_id
