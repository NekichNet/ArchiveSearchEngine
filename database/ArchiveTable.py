from .DatabaseConnection import DatabaseConnection


class ArchiveTable:
    def __init__(self, connection: DatabaseConnection):
        self.connection = connection.connection
        self.cursor = connection.cursor
