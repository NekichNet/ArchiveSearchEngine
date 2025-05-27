from kivymd.uix.screen import MDScreen
from kivymd.uix.button import MDButton, MDButtonText
from kivymd.uix.floatlayout import MDFloatLayout
from kivymd.uix.fitimage import FitImage

from kivymd.uix.textfield import (
    MDTextField,
    MDTextFieldLeadingIcon,
    MDTextFieldHintText
)

from database.ArchiveTable import ArchiveTable
from database.HistoryTable import HistoryTable


class SearchScreen(MDScreen):
    def __init__(self, table_archive: ArchiveTable, table_history: HistoryTable):
        super().__init__()
        self.name = 'Search'
        self.md_bg_color = self.theme_cls.surfaceColor

        background_image = FitImage(source="app/res/background2.png")
        self.add_widget(background_image)

        main_layout = MDFloatLayout(size_hint=(.8, .8),
                                    pos_hint={"center_x": 0.5, "center_y": 0.5},
                                    radius=(50, 15, 50, 15),
                                    md_bg_color=self.theme_cls.surfaceColor)
        self.add_widget(main_layout)

        back_button = MDButton(MDButtonText(text="Назад"),
                               style='tonal',
                               size_hint=(.1, .1),
                               pos_hint={'center_x': .9, 'center_y': .92},
                               height="50dp")
        back_button.bind(on_press=self.back)
        main_layout.add_widget(back_button)

        search_layout = MDFloatLayout(size_hint=(1., .2),
                                      pos_hint={'center_x': .55, 'center_y': .78})
        self.search_input = MDTextField(MDTextFieldLeadingIcon(icon="magnify"),
                                        MDTextFieldHintText(text="Наименование документации"),
                                        size_hint=(.75, 1.),
                                        pos_hint={'center_x': .37, 'center_y': .5},
                                        multiline=False)
        search_layout.add_widget(self.search_input)
        search_button = MDButton(MDButtonText(text="Найти"),
                                 style='tonal',
                                 pos_hint={'center_x': .85, 'center_y': 0.5},
                                 height="50dp")
        search_layout.add_widget(search_button)
        main_layout.add_widget(search_layout)

        self.list_layout = MDFloatLayout(pos_hint={"center_x": 0.5, "center_y": 0.4})
        main_layout.add_widget(self.list_layout)

    def back(self, *args):
        self.manager.current = 'Menu'
        return 0
