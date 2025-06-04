from kivymd.uix.screen import MDScreen
from kivymd.uix.button import MDButton, MDButtonIcon, MDButtonText
from kivymd.uix.stacklayout import MDStackLayout
from kivymd.uix.fitimage import FitImage

from database.ArchiveTable import ArchiveTable

from .User import User


class MenuScreen(MDScreen):
    def __init__(self, table_archive: ArchiveTable):
        super().__init__()
        self.name = 'Menu'
        self.md_bg_color = self.theme_cls.surfaceColor
        self.user = None

        background_image = FitImage(source="app/res/background1.png")
        self.add_widget(background_image)

        self.main_layout = MDStackLayout(size_hint=(.7, .7),
                                         pos_hint={"center_x": .5, "center_y": .5},
                                         radius=(50, 15, 50, 15),
                                         padding=20,
                                         spacing=10,
                                         orientation='tb-lr',
                                         md_bg_color=self.theme_cls.surfaceColor)
        self.add_widget(self.main_layout)

        exit_button = MDButton(MDButtonText(text="Выйти"),
                               style='tonal',
                               size_hint=(.1, .1),
                               pos_hint={'center_x': .9, 'center_y': .92},
                               height="50dp")
        exit_button.bind(on_press=self.exit)
        self.main_layout.add_widget(exit_button)

    def update_buttons(self, is_admin: bool):
        self.main_layout.clear_widgets()

        search_button = MDButton(MDButtonText(text="Поиск документа"))
        search_button.bind(on_press=self.to_search)
        self.main_layout.add_widget(search_button)

        inventory_button = MDButton(MDButtonText(text="Сформировать опись"))
        inventory_button.bind(on_press=self.inventory_list)
        self.main_layout.add_widget(inventory_button)

        destroy_button = MDButton(MDButtonText(text="Сформировать акт об выделении к уничтожению"))
        destroy_button.bind(on_press=self.destroy_list)
        self.main_layout.add_widget(destroy_button)

        if is_admin:  # Дальше только функции для администраторов

            add_doc_button = MDButton(MDButtonText(text="Добавить 1 документ"))
            add_doc_button.bind(on_press=self.add_doc)
            self.main_layout.add_widget(add_doc_button)

            add_docs_button = MDButton(MDButtonText(text="Добавить документы"))
            add_docs_button.bind(on_press=self.add_docs)
            self.main_layout.add_widget(add_docs_button)

            add_admin_button = MDButton(MDButtonText(text="Добавить нового админа"))
            add_admin_button.bind(on_press=self.add_docs)
            self.main_layout.add_widget(add_admin_button)

    def new_admin(self, *args):
        self.manager.get_screen('Registration').admin = True
        self.manager.get_screen('Registration').back_to = 'Menu'
        self.manager.current = 'Registration'
        return 0

    def destroy_list(self, *args):
        return 0

    def add_docs(self, *args):
        return 0

    def add_doc(self, *args):
        return 0

    def inventory_list(self, *args):
        return 0

    def to_search(self, *args):
        self.manager.current = 'Search'
        return 0

    def update_user(self, user: User):
        self.user = user
        self.update_buttons(user.is_admin)

    def exit(self, *args):
        self.manager.current = 'Idle'
        return 0
