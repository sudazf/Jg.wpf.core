using System.Windows.Media;

namespace Jg.wpf.core.Service.ThemeService
{
    public interface IThemeService
    {
        /// <summary>
        /// Change theme base style to black or light.
        /// </summary>
        /// <param name="isDark">is dark?</param>
        void ApplyBase(bool isDark);

        /// <summary>
        /// Change primary color
        /// </summary>
        /// <param name="color">new color</param>
        void ChangePrimaryColor(Color color);

        /// <summary>
        /// Change secondary color
        /// </summary>
        /// <param name="color">new color</param>
        void ChangeSecondaryColor(Color color);
    }
}
