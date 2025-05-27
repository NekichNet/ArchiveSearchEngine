class User:
    def __init__(self, login: str, fullname: str, post: str, is_admin: bool):
        self.login = login
        self.fullname = fullname
        self.post = post
        self.is_admin = is_admin

