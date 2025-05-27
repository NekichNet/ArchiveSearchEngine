from kivymd.uix.screen import MDScreen
from kivymd.uix.button import MDButton, MDButtonIcon, MDButtonText
from kivymd.uix.floatlayout import MDFloatLayout
from kivymd.uix.fitimage import FitImage


class IdleScreen(MDScreen):
    def __init__(self):
        super().__init__()
        self.name = 'Idle'
        self.md_bg_color = self.theme_cls.surfaceColor

        background_image = FitImage(source="app/res/background.png")
        self.add_widget(background_image)

        main_layout = MDFloatLayout(size_hint=(.5, .5),
                                    pos_hint={"center_x": .5, "center_y": .5},
                                    radius=(50, 15, 50, 15),
                                    md_bg_color=self.theme_cls.surfaceColor)
        self.add_widget(main_layout)

        sign_in_button = MDButton(MDButtonText(text="Войти",
                                               pos_hint={"center_x": .5, "center_y": .5}),
                                  style='tonal',
                                  pos_hint={'center_x': .5, 'center_y': .4},
                                  theme_width="Custom",
                                  size_hint_x=.5,
                                  height="50dp")
        sign_in_button.bind(on_press=self.to_login_screen)
        main_layout.add_widget(sign_in_button)

        sign_up_button = MDButton(MDButtonText(text="Зарегистрироваться",
                                               pos_hint={"center_x": .5, "center_y": .5}),
                                  style='tonal',
                                  pos_hint={'center_x': .5, 'center_y': .6},
                                  theme_width="Custom",
                                  size_hint_x=.5,
                                  height="50dp")
        sign_up_button.bind(on_press=self.to_registration_screen)
        main_layout.add_widget(sign_up_button)

        logo_layout = MDFloatLayout(size_hint=(.3, .15),
                                    pos_hint={"center_x": .17, "center_y": .9},
                                    radius=(50, 15, 50, 15),
                                    md_bg_color=self.theme_cls.surfaceColor)
        logo_image = FitImage(source="app/res/logo.png",
                              pos_hint={"center_x": .5, "center_y": .5},
                              size_hint=(.9, .7),
                              fit_mode='scale-down')
        logo_layout.add_widget(logo_image)
        self.add_widget(logo_layout)

    def to_login_screen(self, *args):
        self.manager.current = 'Login'
        return 0

    def to_registration_screen(self, *args):
        self.manager.get_screen('Registration').back_to = 'Idle'
        self.manager.get_screen('Registration').admin = False
        self.manager.current = 'Registration'
        return 0
