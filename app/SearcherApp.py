from kivymd.app import MDApp
from kivymd.uix.screenmanager import MDScreenManager

# Screens' files won't import without "."
from .StartScreen import StartScreen
from .IdleScreen import IdleScreen
from .LoginScreen import LoginScreen
from .RegistrationScreen import RegistrationScreen
from .SearchScreen import SearchScreen
from .MenuScreen import MenuScreen
from .NewDocScreen import NewDocScreen

from .ThemeConfig import tc

from database.ArchiveTable import ArchiveTable
from database.UserTable import UserTable
from database.HistoryTable import HistoryTable


class SearcherApp(MDApp):
    def __init__(self,
                 table_archive: ArchiveTable,
                 table_user: UserTable,
                 table_history: HistoryTable):
        self.table_archive = table_archive
        self.table_user = table_user
        self.table_history = table_history
        super().__init__()

    """
    build() just adding all screens to ScreenManager. This class does nothing else
    """
    def build(self):
        self.theme_cls.primary_palette = tc.palette
        self.theme_cls.theme_style = tc.theme_style
        self.theme_cls.md_bg_color = self.theme_cls.backgroundColor

        sm = MDScreenManager()
        # sm.add_widget(StartScreen())
        sm.add_widget(IdleScreen())
        sm.add_widget(LoginScreen(self.table_user))
        sm.add_widget(RegistrationScreen(self.table_user))
        sm.add_widget(SearchScreen(self.table_archive, self.table_history))
        sm.add_widget(MenuScreen(self.table_archive))
        sm.add_widget(NewDocScreen(self.table_archive))

        return sm
