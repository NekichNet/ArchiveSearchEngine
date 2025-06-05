from .DatabaseConnection import DatabaseConnection

from hashlib import sha256


class UserTable:
    def __init__(self, connection: DatabaseConnection):
        self.connection = connection.connection
        self.cursor = connection.cursor

    def new_user(self, is_admin: bool,
                 login: str, password,
                 fullname: str, post: str):
        self.cursor.execute(f'INSERT INTO UserTable (login, password_hash, fullname, post, is_admin) VALUES (?, ?, ?, ?, ?)', (login, sha256(password.encode()).hexdigest(), fullname, post, is_admin))
        self.connection.commit()

    def delete_user(self, login: str):
        self.cursor.execute(f'DELETE FROM UserTable WHERE login = ?', (login,))
        self.connection.commit()

    def check_user(self, login: str, password: str):
        self.cursor.execute('SELECT login, fullname, post, is_admin FROM UserTable WHERE login == ? AND password_hash == ?', (login, sha256(password.encode()).hexdigest()))
        return self.cursor.fetchall()
