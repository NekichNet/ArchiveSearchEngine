from .DatabaseConnection import DatabaseConnection


class ArchiveTable:
    def __init__(self, connection: DatabaseConnection):
        self.connection = connection.connection
        self.cursor = connection.cursor

    def find(self, object_name: str):
        self.cursor.execute(f"SELECT * FROM ArchiveTable WHERE object_name REGEXP '.*{object_name}.*'")
        return self.cursor.fetchall()