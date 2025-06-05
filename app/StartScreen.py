from kivymd.uix.floatlayout import MDFloatLayout
from kivymd.uix.label import MDLabel
from kivymd.uix.fitimage import FitImage
from kivymd.uix.screen import MDScreen
from kivy.clock import Clock

from random import shuffle
from time import sleep


class StartScreen(MDScreen):
    def __init__(self):
        super().__init__()
        self.name = 'Start'
        self.md_bg_color = self.theme_cls.surfaceColor

        self.queue = [i for i in range(1, 7, 1)]
        shuffle(self.queue)

        self.background_image = FitImage(source=f"app/res/background{self.queue[0]}.png")
        self.add_widget(self.background_image)

        self.main_layout = MDFloatLayout(md_bg_color=self.theme_cls.surfaceColor)
        self.add_widget(self.main_layout)

    def on_start(self):
        Clock.usleep(500)
        self.main_layout.size_hint = (0.4, 1.)

        Clock.usleep(300)
        self.main_layout.size_hint = (.4, .4)

        Clock.usleep(2000)
        self.manager.current = 'Idle'
