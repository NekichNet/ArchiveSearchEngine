import sqlite3
from os.path import exists
from hashlib import sha256


class DatabaseConnection:
    def __init__(self, dbfilename: str):
        generate_admin = not exists(dbfilename)
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
        
        book_num TEXT NOT NULL,
        
        content_quantity INTEGER NOT NULL,
        
        inventory_date DATE,
        inventory_num TEXT,
        
        object_index TEXT NOT NULL,
        object_name TEXT NOT NULL,
        
        rack INTEGER NOT NULL,
        shelf INTEGER NOT NULL,
        
        expiring_in TEXT NOT NULL,
        
        documents_date DATE NOT NULL,
        
        case_num TEXT NOT NULL,
        
        destruct_act_num TEXT,
        destruct_act_date DATE,
        
        struct_division TEXT,
        
        gived_post TEXT,
        gived_fullname TEXT,
        achieved_post TEXT,
        achieved_fullname TEXT,
        
        note TEXT
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

        if generate_admin:  # Добавляем админа по умолчанию, если бд пересоздаётся
            self.cursor.execute('''
            INSERT INTO UserTable (login, password_hash, fullname, post, is_admin) VALUES (?, ?, ?, ?, ?)
            ''', ('admin', sha256('admin'.encode()).hexdigest(), "Администратор", "Администратор", True))

        self.connection.commit()
