from datetime import date


class Document:
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
        self.note = note
        self.achieved_fullname = achieved_fullname
        self.achieved_post = achieved_post
        self.gived_fullname = gived_fullname
        self.gived_post = gived_post
        self.struct_division = struct_division
        self.destruct_act_date = destruct_act_date
        self.destruct_act_num = destruct_act_num
        self.case_num = case_num
        self.documents_date = documents_date
        self.expiring_in = expiring_in
        self.shelf = shelf
        self.rack = rack
        self.object_name = object_name
        self.object_index = object_index
        self.inventory_date = inventory_date
        self.inventory_num = inventory_num
        self.content_quantity = content_quantity
        self.book_num = book_num
        self.volume_num = volume_num
        self.registration_num = registration_num
        self.row_id = row_id

