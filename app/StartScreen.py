from kivymd.uix.floatlayout import MDFloatLayout
from kivymd.uix.label import MDLabel
from kivymd.uix.fitimage import FitImage
from kivymd.uix.screen import MDScreen

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

    def do_animation(self):
        sleep(0.5)
        self.main_layout.size_hint = (0.4, 1.)

        sleep(0.3)
        self.main_layout.size_hint = (.4, .4)

        sleep(2)
        self.manager.current = 'Idle'
