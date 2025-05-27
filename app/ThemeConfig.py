class ThemeConfig:
    def __init__(self):
        with open("app/theme_config.txt", 'r', encoding='utf-8') as f:
            self.palette = f.readline()[:-1]
            self.theme_style = f.readline()[:-1]


tc = ThemeConfig()
