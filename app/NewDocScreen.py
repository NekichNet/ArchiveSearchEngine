from kivymd.uix.screen import MDScreen
from kivymd.uix.button import MDButton, MDButtonIcon, MDButtonText, MDFabButton
from kivymd.uix.boxlayout import MDBoxLayout
from kivymd.uix.scrollview import MDScrollView
from kivymd.uix.label import MDLabel
from kivymd.uix.fitimage import FitImage

from kivymd.uix.textfield import (
    MDTextField,
    MDTextFieldLeadingIcon,
    MDTextFieldHintText,
    MDTextFieldMaxLengthText,
)

from database.Document import Document
from database.ArchiveTable import ArchiveTable


class NewDocScreen(MDScreen):
    def __init__(self, table_archive: ArchiveTable):
        super().__init__()
        self.name = 'NewDoc'
        self.md_bg_color = self.theme_cls.surfaceColor
        self.table_archive = table_archive

        background_image = FitImage(source="app/res/background2.png")
        self.add_widget(background_image)

        scrollview_layout = MDBoxLayout(pos_hint={"center_x": 0.5, "center_y": 0.5},
                                        radius=(50, 15, 50, 15),
                                        md_bg_color=self.theme_cls.surfaceColor,
                                        size_hint=(.8, .8),
                                        padding=20)
        self.add_widget(scrollview_layout)

        scrollview = MDScrollView(do_scroll_x=False,
                                  pos_hint={"center_x": 0.5, "center_y": 0.5},
                                  size_hint=(.9, .9))
        scrollview_layout.add_widget(scrollview)

        self.main_layout = MDBoxLayout(pos_hint={"center_x": 0.5, "center_y": 0.5},
                                       orientation='vertical',
                                       padding=20,
                                       spacing=10,
                                       md_bg_color=self.theme_cls.surfaceColor,
                                       adaptive_height=True)
        scrollview.add_widget(self.main_layout)

        for param, title in Document.names.items():
            self.main_layout.add_widget(MDTextField(MDTextFieldHintText(text=title), multiline=False, id=param))

        done_button = MDButton(MDButtonText(text="Добавить"),
                               style='tonal')
        done_button.bind(on_press=self.add_doc)
        self.main_layout.add_widget(done_button)

    def add_doc(self, *args):
        values = dict()
        for param in list(Document.names.keys()):
            values[param] = self.main_layout.get_ids()[param].text
            self.main_layout.ids[param].text = ""
        self.table_archive.add(values)
        self.manager.current = 'Menu'
