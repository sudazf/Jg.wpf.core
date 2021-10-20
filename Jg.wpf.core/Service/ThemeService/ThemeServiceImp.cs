using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;

namespace Jg.wpf.core.Service.ThemeService
{
    internal class ThemeServiceImp : IThemeService
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public ThemeServiceImp()
        {
            
        }

        public void ApplyBase(bool isDark)
        {
            var theme = _paletteHelper.GetTheme();
            var baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);
            _paletteHelper.SetTheme(theme);
        }

        public void ChangePrimaryColor(Color color)
        {
            var theme = _paletteHelper.GetTheme();

            theme.PrimaryLight = new ColorPair(color.Lighten());
            theme.PrimaryMid = new ColorPair(color);
            theme.PrimaryDark = new ColorPair(color.Darken());

            _paletteHelper.SetTheme(theme);
        }

        public void ChangeSecondaryColor(Color color)
        {
            var theme = _paletteHelper.GetTheme();

            theme.SecondaryLight = new ColorPair(color.Lighten());
            theme.SecondaryMid = new ColorPair(color);
            theme.SecondaryDark = new ColorPair(color.Darken());

            _paletteHelper.SetTheme(theme);
        }
    }
}
