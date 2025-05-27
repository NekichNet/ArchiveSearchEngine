import sqlite3


class DatabaseConnection:
    def __init__(self, dbfilename: str):
        self.connection = sqlite3.connect(dbfilename)
        self.cursor = self.connection.cursor()

        # Создаём таблицы, если их не существует
        self.cursor.execute('''
        CREATE TABLE IF NOT EXISTS UserTable (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        login TEXT NOT NULL,
        password_hash TEXT NOT NULL,
        fullname TEXT NOT NULL,
        post TEXT NOT NULL,
        is_admin BOOLEAN NOT NULL
        )
        ''')

        self.cursor.execute('''CREATE TABLE IF NOT EXISTS ArchiveTable (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        
        registration_num TEXT NOT NULL,
        
        volume_num TEXT NOT NULL,
        
        bool_num TEXT NOT NULL,
        
        content_quantity INTEGER NOT NULL,
        
        inventory_date DATE NOT NULL,
        inventory_num TEXT NOT NULL,
        
        object_index TEXT NOT NULL,
        object_name TEXT NOT NULL,
        
        rack INTEGER NOT NULL,
        shelf INTEGER NOT NULL,
        
        expiring_in TEXT NOT NULL,
        
        documents_date DATE NOT NULL,
        
        case_num TEXT NOT NULL,
        
        destruct_act_num TEXT NOT NULL,
        destruct_act_date DATE NOT NULL,
        
        struct_division TEXT NOT NULL,
        
        gived_post TEXT NOT NULL,
        gived_fullname TEXT NOT NULL,
        achieved_post TEXT NOT NULL,
        achieved_fullname TEXT NOT NULL
        )''')

        self.cursor.execute('''
        CREATE TABLE IF NOT EXISTS HistoryTable (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        archive_id INTEGER NOT NULL,
        user_id INTEGER NOT NULL,
        date_taken DATE NOT NULL,
        time_taken TIME NOT NULL,
        date_returned DATE NOT NULL,
        time_returned TIME NOT NULL
        )
        ''')

        self.connection.commit()
