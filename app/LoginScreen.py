from kivymd.uix.screen import MDScreen
from kivymd.uix.button import MDButton, MDButtonIcon, MDButtonText, MDFabButton
from kivymd.uix.stacklayout import MDStackLayout
from kivymd.uix.label import MDLabel
from kivymd.uix.fitimage import FitImage

from kivymd.uix.textfield import (
    MDTextField,
    MDTextFieldLeadingIcon,
    MDTextFieldHintText
)

from database.UserTable import UserTable

from .User import User


class LoginScreen(MDScreen):
    def __init__(self, table_user: UserTable):
        super().__init__()
        self.name = 'Login'
        self.md_bg_color = self.theme_cls.surfaceColor
        self.table_user = table_user

        background_image = FitImage(source="app/res/background4.png")
        self.add_widget(background_image)

        main_layout = MDStackLayout(size_hint=(.7, .7),
                                    pos_hint={"center_x": 0.5, "center_y": 0.5},
                                    radius=(50, 15, 50, 15),
                                    padding=40,
                                    spacing=10,
                                    md_bg_color=self.theme_cls.surfaceColor)
        self.add_widget(main_layout)
        back_button = MDFabButton(style='small',
                                  icon='arrow-left',
                                  pos_hint={'center_x': .9, 'center_y': .92})
        back_button.bind(on_press=self.back)
        self.add_widget(back_button)

        self.status_label = MDLabel(text="Введите ваш логин и пароль",
                                    pos_hint={'center_x': .25, 'center_y': .85},
                                    size_hint=(.4, .1),
                                    halign="left", valign="bottom")
        self.status_label.bind(size=self.status_label.setter('text_size'))
        main_layout.add_widget(self.status_label)

        self.login_input = MDTextField(MDTextFieldHintText(text="Логин"),
                                       pos_hint={'center_x': .5, 'center_y': .7},
                                       size_hint=(.9, .1),
                                       multiline=False)
        main_layout.add_widget(self.login_input)
        self.password_input = MDTextField(MDTextFieldHintText(text="Пароль"),
                                          pos_hint={'center_x': .5, 'center_y': .5},
                                          size_hint=(.9, .1),
                                          multiline=False,
                                          on_text_validate=self.login)
        main_layout.add_widget(self.password_input)

        self.forgot_pass_label = MDLabel(text="Забыли логин или пароль? Обратитесь к администратору",
                                         pos_hint={'center_x': .55, 'center_y': .33},
                                         size_hint=(1., 0.12),
                                         halign="left", valign="top")
        self.forgot_pass_label.bind(size=self.forgot_pass_label.setter('text_size'))
        main_layout.add_widget(self.forgot_pass_label)

        login_button = MDButton(MDButtonText(text="Подтвердить"),
                                style='tonal',
                                pos_hint={'center_x': .5, 'center_y': .2},
                                height="50dp")
        login_button.bind(on_press=self.login)
        main_layout.add_widget(login_button)

    def back(self, *args):
        self.manager.current = 'Idle'
        return 0

    def login(self, *args):
        results = self.table_user.check_user(self.login_input.text, self.password_input.text)
        if len(results) > 0:
            self.manager.get_screen('Menu').update_user(
                User(
                    login=results[0][0],
                    fullname=results[0][1],
                    post=results[0][2],
                    is_admin=results[0][3]
                )
            )
            self.manager.current = 'Menu'
        else:
            print("User not found")
        return 0
