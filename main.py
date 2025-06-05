from os.path import join
import sys
from kivy.resources import resource_add_path
from app.SearcherApp import SearcherApp
from kivy.core.window import Window

from database.DatabaseConnection import DatabaseConnection
from database.ArchiveTable import ArchiveTable
from database.UserTable import UserTable
from database.HistoryTable import HistoryTable


if __name__ == "__main__":
    # Создаём одно общее подключение к бд на SQLite3
    db_connection = DatabaseConnection('archive_search_engine.db')
    # И инициализируем классы по управлению таблицами, используя наше подключение
    table_archive = ArchiveTable(db_connection)
    table_user = UserTable(db_connection)
    table_history = HistoryTable(db_connection)

    Window.size = (1600, 900)

    try:
        if hasattr(sys, '_MEIPASS'):  # Нужно для корректной сборки KivyMD
            resource_add_path(join(sys._MEIPASS))
        SearcherApp(table_archive, table_user, table_history).run()
    except Exception as e:
        raise e
    finally:
        db_connection.connection.close()
