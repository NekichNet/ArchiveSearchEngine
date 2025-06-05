from .DatabaseConnection import DatabaseConnection


class ArchiveTable:
    def __init__(self, connection: DatabaseConnection):
        self.connection = connection.connection
        self.cursor = connection.cursor

    def find(self, object_name: str):
        self.cursor.execute(f"SELECT * FROM ArchiveTable WHERE object_name REGEXP '.*{object_name}.*'")
        return self.cursor.fetchall()

    def add(self, values: dict):
        self.cursor.execute(
            f"INSERT INTO ArchiveTable ({', '.join(list(values.keys()))}) VALUES ({', '.join('?' * len(values.keys()))})",
            tuple([value for key, value in values.items()])
        )
