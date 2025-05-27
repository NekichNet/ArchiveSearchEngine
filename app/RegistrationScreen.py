from kivymd.uix.screen import MDScreen
from kivymd.uix.button import MDButton, MDButtonIcon, MDButtonText, MDFabButton
from kivymd.uix.stacklayout import MDStackLayout
from kivymd.uix.label import MDLabel
from kivymd.uix.fitimage import FitImage

from kivymd.uix.textfield import (
    MDTextField,
    MDTextFieldLeadingIcon,
    MDTextFieldHintText,
    MDTextFieldMaxLengthText,
)

from database.UserTable import UserTable


class RegistrationScreen(MDScreen):
    def __init__(self, table_user: UserTable):
        super().__init__()
        self.name = 'Registration'
        self.md_bg_color = self.theme_cls.surfaceColor
        self.table_user = table_user
        self.back_to = 'Idle'
        self.admin = False

        background_image = FitImage(source="app/res/background.png")
        self.add_widget(background_image)

        main_layout = MDStackLayout(size_hint=(.7, .7),
                                    pos_hint={"center_x": 0.5, "center_y": 0.5},
                                    radius=(50, 15, 50, 15),
                                    orientation='tb-lr',
                                    padding=40,
                                    spacing=10,
                                    md_bg_color=self.theme_cls.surfaceColor)
        self.add_widget(main_layout)

        back_button = MDFabButton(style='small',
                                  icon='arrow-left',
                                  pos_hint={'center_x': .9, 'center_y': .92})
        back_button.bind(on_press=self.back)
        self.add_widget(back_button)

        self.login_input = MDTextField(MDTextFieldHintText(text="Логин"),
                                       multiline=False)
        main_layout.add_widget(self.login_input)

        self.password_first_input = MDTextField(MDTextFieldHintText(text="Пароль"),
                                                multiline=False)
        main_layout.add_widget(self.password_first_input)
        self.password_second_input = MDTextField(MDTextFieldHintText(text="Повторите пароль"),
                                                 multiline=False)
        main_layout.add_widget(self.password_second_input)

        self.fullname_input = MDTextField(MDTextFieldHintText(text="ФИО"),
                                          multiline=False)
        main_layout.add_widget(self.fullname_input)

        self.post_input = MDTextField(MDTextFieldHintText(text="Должность"),
                                      multiline=False)
        main_layout.add_widget(self.post_input)

        registrate_button = MDButton(MDButtonText(text="Подтвердить"),
                                     style='tonal')
        registrate_button.bind(on_press=self.registrate)
        main_layout.add_widget(registrate_button)

    def back(self, *args):
        self.manager.current = self.back_to
        return 0

    def registrate(self, *args):
        if self.password_first_input.text == self.password_second_input.text and len(self.password_first_input.text) > 0 and len(self.login_input.text) > 0 and len(self.fullname_input.text) > 0 and len(self.post_input.text) > 0:
            self.table_user.new_user(self.admin, self.login_input.text, self.password_first_input.text, self.fullname_input.text, self.post_input.text)
            self.manager.current = self.back_to

        return 0
